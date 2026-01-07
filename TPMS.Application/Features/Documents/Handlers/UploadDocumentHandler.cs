using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, DocumentDto>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;
        private readonly IDocumentTypeCacheService _docTypeCache;
        private readonly IFileStorageService _fileStorage;

        public UploadDocumentHandler(
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

        public async Task<DocumentDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Document;

            // ---------------------------------------------------
            // 1️⃣ Resolve OwnerTypeID
            // ---------------------------------------------------
            int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(dto.OwnerType);

            // ---------------------------------------------------
            // 2️⃣ Resolve and Validate DocumentType + Category
            // ---------------------------------------------------
            int documentTypeId = dto.DocumentTypeID ?? 0;
            string docTypeName = dto.DocType ?? "";

            // If only name given → resolve ID
            if (documentTypeId == 0 && !string.IsNullOrWhiteSpace(docTypeName))
                documentTypeId = _docTypeCache.GetDocumentTypeId(docTypeName);

            // If ID given but name empty → fetch name from cache
            if (documentTypeId != 0 && string.IsNullOrWhiteSpace(docTypeName))
                docTypeName = _docTypeCache.GetDocumentTypeName(documentTypeId);

            if (!_docTypeCache.IsValidType(documentTypeId))
                throw new InvalidOperationException($"Invalid DocumentType ID '{documentTypeId}'.");

            if (!_docTypeCache.IsActiveType(documentTypeId))
                throw new InvalidOperationException($"DocumentType '{docTypeName}' is inactive.");

            // Fetch type including category validation
            var docType = await _db.DocumentTypes
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.DocumentTypeID == documentTypeId, cancellationToken);

            if (docType == null)
                throw new InvalidOperationException("DocumentType not found in DB.");

            if (docType.DocumentCategoryID != dto.DocumentCategoryID)
                throw new InvalidOperationException(
                    $"DocumentType '{docType.TypeName}' does not belong to CategoryID {dto.DocumentCategoryID}");

            
            // ---------------------------------------------------
            // 3️⃣ Check for previous active version (same type + owner + filename)
            // ---------------------------------------------------
            var existingDocs = await _db.Documents
                .Where(d =>
                    d.OwnerTypeID == ownerTypeId &&
                    d.OwnerID == dto.OwnerID &&
                    d.DocumentTypeID == documentTypeId &&
                    d.FileName == dto.File.FileName)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            Document? previousActiveDoc = existingDocs.FirstOrDefault(d => d.IsActive);
            string newVersion = previousActiveDoc == null
                ? "v1.0"
                : IncrementVersion(previousActiveDoc.Version ?? "v1.0");

            if (previousActiveDoc != null)
                previousActiveDoc.IsActive = false;

            string? fileUrl = null;

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // ---------------------------------------------------
                // 4️⃣ Upload file to storage
                // ---------------------------------------------------
                fileUrl = await _fileStorage.SaveFileAsync(
                    dto.File,
                    dto.OwnerType,
                    dto.OwnerID,
                    cancellationToken);

                // ---------------------------------------------------
                // 5️⃣ Create new Document record
                // ---------------------------------------------------
                var document = new Document
                {
                    OwnerTypeID = ownerTypeId,
                    OwnerID = dto.OwnerID,
                    DocumentTypeID = documentTypeId,   // Linked to category
                    DocType = docTypeName,
                   // DocumentTypeName = dto.DocumentTypeName! ;
                    DocumentCategoryID = dto.DocumentCategoryID, // Added by siju
                    DocumentCategoryName = dto.DocumentCategoryName, // Added by siju
                    FileName = dto.File.FileName,
                    URL = fileUrl,
                    UploadedBy = dto.UploadedBy,
                    UploadedAt = DateTime.UtcNow,
                    Version = newVersion,
                    Description = dto.Description,
                    IsActive = true,
                };

                if (previousActiveDoc != null)
                    _db.Documents.Update(previousActiveDoc);

                _db.Documents.Add(document);
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // ---------------------------------------------------
                // 6️⃣ Return response DTO
                // ---------------------------------------------------
                return new DocumentDto
                {
                    DocumentID = document.DocumentID,
                    OwnerTypeID = document.OwnerTypeID,
                    OwnerID = document.OwnerID,
                    DocumentTypeID = document.DocumentTypeID,
                    FileName = document.FileName,
                    URL = document.URL,
                    UploadedBy = document.UploadedBy,
                    UploadedAt = document.UploadedAt,
                    Version = document.Version,
                    Description = document.Description,
                    IsActive = document.IsActive,

                    DocumentCategoryID = docType.DocumentCategoryID,
                    
                    DocumentTypeName = docType.TypeName,
                    DocumentCategoryName = docType.Category?.CategoryName
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                // restore previous version
                if (previousActiveDoc != null)
                {
                    previousActiveDoc.IsActive = true;
                    _db.Documents.Update(previousActiveDoc);
                    await _db.SaveChangesAsync(cancellationToken);
                }

                // delete file if it was uploaded
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    try
                    {
                        await _fileStorage.DeleteFileAsync(fileUrl, cancellationToken);
                    }
                    catch { }
                }

                throw new InvalidOperationException($"Document upload failed: {ex.Message}", ex);
            }
        }

        private static string IncrementVersion(string version)
        {
            version = version.Trim().ToLower().Replace("v", "");
            var parts = version.Split('.');

            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int major) &&
                int.TryParse(parts[1], out int minor))
            {
                minor++;
                return $"v{major}.{minor}";
            }

            return "v1.0";
        }
    }
}
