using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class UploadDocumentChunkHandler : IRequestHandler<UploadDocumentChunkCommand>
    {
        private readonly TPMSDBContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public UploadDocumentChunkHandler(
            TPMSDBContext db,
            IWebHostEnvironment env,
            IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _env = env;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task Handle(UploadDocumentChunkCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ChunkDto;

            // Basic validation
            if (dto.ChunkNumber <= 0) throw new ArgumentException("ChunkNumber must be >= 1");
            if (dto.TotalChunks <= 0) throw new ArgumentException("TotalChunks must be >= 1");
            if (dto.ChunkNumber > dto.TotalChunks) throw new ArgumentException("ChunkNumber cannot exceed TotalChunks");

            // Resolve owner type ID
            int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(dto.OwnerType);

            // Load or create session record
            var session = await _db.DocumentUploadSessions.FindAsync(new object[] { dto.SessionId }, cancellationToken);
            if (session == null)
            {
                session = new DocumentUploadSession
                {
                    SessionId = dto.SessionId,
                    FileName = dto.FileName,
                    OwnerTypeID = ownerTypeId,
                    OwnerID = dto.OwnerID,
                    DocType = dto.DocType,
                    TotalChunks = dto.TotalChunks,
                    UploadedChunks = 0,
                    Status = "Uploading",
                    UploadedBy = dto.UploadedBy,
                    Description = dto.Description,
                    DocumentCategoryID = dto.DocumentCategoryID,
                    DocumentTypeID = dto.DocumentTypeID
                };
                _db.DocumentUploadSessions.Add(session);
                await _db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // If session exists, ensure metadata matches (file name, totals) to avoid collisions
                if (!string.Equals(session.FileName, dto.FileName, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Session already exists with a different file name.");

                if (session.TotalChunks != dto.TotalChunks)
                    throw new InvalidOperationException("TotalChunks mismatch with existing session.");

                // update status if needed
                session.Status = "Uploading";
                session.UpdatedAt = DateTime.UtcNow;
            }

            // Save chunk to temp folder (one folder per session)
            string tempFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Temp", dto.SessionId.ToString());
            Directory.CreateDirectory(tempFolder);

            // Use zero-padded chunk names to preserve ordering
            string chunkPath = Path.Combine(tempFolder, $"{dto.ChunkNumber:D6}.part");

            using (var stream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await dto.File.CopyToAsync(stream, cancellationToken);
            }

            // Update session progress (ensure idempotency)
            // Only increment if file was newly created - simple approach: check file exists before increment
            session.UploadedChunks = Directory.GetFiles(tempFolder, "*.part").Length;
            session.UpdatedAt = DateTime.UtcNow;

            // If all chunks uploaded mark status
            if (session.UploadedChunks >= session.TotalChunks)
            {
                session.Status = "Uploaded";
            }

            await _db.SaveChangesAsync(cancellationToken);

          //  return Unit.Value;
        }
    }
}
