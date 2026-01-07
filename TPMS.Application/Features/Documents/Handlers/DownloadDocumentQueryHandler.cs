using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class DownloadDocumentQueryHandler : IRequestHandler<DownloadDocumentQuery, DownloadDocumentDto>
    {

        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;
        private readonly IDocumentTypeCacheService _docTypeCache;
        private readonly IFileStorageService _fileStorage;

        public DownloadDocumentQueryHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache,
            IDocumentTypeCacheService docTypeCache,
            IFileStorageService fileStorage)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
            _docTypeCache = docTypeCache;
            _fileStorage = fileStorage;
        }


        public async Task<DownloadDocumentDto> Handle(DownloadDocumentQuery request, CancellationToken cancellationToken)
        {
            Document? document = null;

            // 1️⃣ Resolve by DocumentID
            if (request.DocumentID.HasValue)
            {
                document = await _db.Documents.FirstOrDefaultAsync(d => d.DocumentID == request.DocumentID.Value, cancellationToken);
            }
            else
            {
                // 2️⃣ Resolve by OwnerType + OwnerID + DocumentType + Version
                if (string.IsNullOrEmpty(request.OwnerType) || !request.OwnerID.HasValue || string.IsNullOrEmpty(request.DocumentTypeName))
                    throw new ArgumentException("Either DocumentID or (OwnerType, OwnerID, DocumentTypeName) must be provided.");

                int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(request.OwnerType);
                int docTypeId = _docTypeCache.GetDocumentTypeId(request.DocumentTypeName);

                var query = _db.Documents
                    .Where(d => d.OwnerTypeID == ownerTypeId && d.OwnerID == request.OwnerID && d.DocumentTypeID == docTypeId);

                if (!string.IsNullOrEmpty(request.Version))
                    query = query.Where(d => d.Version == request.Version);
                else
                    query = query.Where(d => d.IsActive); // latest version

                document = await query.OrderByDescending(d => d.UploadedAt).FirstOrDefaultAsync(cancellationToken);
            }

            if (document == null)
                throw new FileNotFoundException("Document not found.");

            // 3️⃣ Retrieve file
            var fileBytes = await _fileStorage.GetFileBytesAsync(document.URL!, cancellationToken);

            //  Log access (for audit trail)
            _db.DocumentAccessLogs.Add(new Domain.Entities.DocumentAccessLog
            {
                DocumentID = document.DocumentID,
                AccessedBy = "system-user", // Replace with logged-in user later
                AccessedAt = DateTime.UtcNow,
                AccessType = "Download"
            });
            await _db.SaveChangesAsync(cancellationToken);

            // 5️⃣ Return
            return new DownloadDocumentDto
            {
                DocumentID = document.DocumentID,
                FileName = document.FileName ?? $"Document_{document.DocumentID}",
                FileBytes = fileBytes,
                ContentType = GetMimeType(document.FileName),
                Version = document.Version,
                URL = document.URL
            };
        }


        private static string GetMimeType(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "application/octet-stream";
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }


}
