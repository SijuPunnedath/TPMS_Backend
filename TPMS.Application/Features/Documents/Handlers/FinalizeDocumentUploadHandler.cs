using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class FinalizeDocumentUploadHandler : IRequestHandler<FinalizeDocumentUploadCommand, DocumentDto>
    {
        private readonly TPMSDBContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IMediator _mediator;
        private readonly IOwnerTypeCacheService _ownerTypeCache;
        private readonly IDocumentTypeCacheService _docTypeCache;
        private readonly IFileStorageService _fileStorage;

        public FinalizeDocumentUploadHandler(
            TPMSDBContext db,
            IWebHostEnvironment env,
            IMediator mediator,
            IOwnerTypeCacheService ownerTypeCache,
            IDocumentTypeCacheService docTypeCache,
            IFileStorageService fileStorage)
        {
            _db = db;
            _env = env;
            _mediator = mediator;
            _ownerTypeCache = ownerTypeCache;
            _docTypeCache = docTypeCache;
            _fileStorage = fileStorage;
        }

        public async Task<DocumentDto> Handle(FinalizeDocumentUploadCommand request, CancellationToken cancellationToken)
        {
            var session = await _db.DocumentUploadSessions
                .FirstOrDefaultAsync(s => s.SessionId == request.SessionId, cancellationToken);

            if (session == null)
                throw new InvalidOperationException("Upload session not found.");

            if (session.UploadedChunks < session.TotalChunks)
                throw new InvalidOperationException("Upload incomplete. Not all chunks received.");

            if (session.IsCompleted)
                throw new InvalidOperationException("Session already finalized.");

            string tempFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Temp", session.SessionId.ToString());
            string mergedPath = Path.Combine(tempFolder, session.FileName);

            // Merge chunks in order
            using (var output = new FileStream(mergedPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                for (int i = 1; i <= session.TotalChunks; i++)
                {
                    string chunkPath = Path.Combine(tempFolder, $"{i:D6}.part");
                    if (!File.Exists(chunkPath))
                        throw new InvalidOperationException($"Missing chunk: {i}");

                    using (var chunk = new FileStream(chunkPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await chunk.CopyToAsync(output, cancellationToken);
                    }
                }
            }

            // Create IFormFile from merged file for reuse with existing handler
            var fileStream = new FileStream(mergedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IFormFile mergedFormFile = new FormFile(fileStream, 0, fileStream.Length, "file", session.FileName);

            // Resolve owner type name (we need to pass OwnerType string that UploadDocumentCommand expects)
            // We'll find the owner type name via IOwnerTypeCacheService by scanning the OwnerTypes table
            // But since your cache service maps name->id, we need reverse; we'll fetch from DB
            var ownerType = await _db.OwnerTypes.FirstOrDefaultAsync(o => o.OwnerTypeID == session.OwnerTypeID, cancellationToken);
            string ownerTypeName = ownerType?.Name ?? throw new InvalidOperationException("OwnerType not found.");

            // Build UploadDocumentDto for the centralized UploadDocumentCommand
            var uploadDto = new UploadDocumentDto
            {
                OwnerType = ownerTypeName,
                OwnerID = session.OwnerID,
                DocumentTypeID = session.DocumentTypeID,
                DocumentCategoryID = session.DocumentCategoryID ?? 0, // if null, Upload handler may resolve from type
                DocType = session.DocType,
                UploadedBy = session.UploadedBy,
                File = mergedFormFile,
                Description = session.Description,
                Version = null
            };

            // Call existing UploadDocumentCommand (reuse versioning & DB logic)
            var result = await _mediator.Send(new UploadDocumentCommand(uploadDto), cancellationToken);

            // Mark session completed & set timestamps
            session.IsCompleted = true;
            session.CompletedAt = DateTime.UtcNow;
            session.Status = "Completed";
            session.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            // Cleanup temp folder (close stream first)
            fileStream.Close();
            try
            {
                Directory.Delete(tempFolder, true);
            }
            catch
            {
                // log, but don't fail
            }

            return result;
        }
    }
}
