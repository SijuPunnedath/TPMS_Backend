
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

namespace TPMS.Application.Features.Documents.Handlers;

public class UploadDocumentStreamHandler
    : IRequestHandler<UploadDocumentStremCommand, DocumentDto>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _ownerTypeCache;
    private readonly IDocumentTypeCacheService _docTypeCache;
    private readonly IFileStorageService _fileStorage;

    public UploadDocumentStreamHandler(
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

    public async Task<DocumentDto> Handle(
        UploadDocumentStremCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Document;

        // ---------------------------------------------------
        // 1 Resolve OwnerTypeID
        // ---------------------------------------------------
        if (string.IsNullOrWhiteSpace(dto.OwnerType))
            throw new InvalidOperationException("OwnerType is required.");

        int ownerTypeId =
            _ownerTypeCache.GetOwnerTypeId(dto.OwnerType.Trim());

        // ---------------------------------------------------
        // 2 Resolve DocumentType
        // ---------------------------------------------------
        int documentTypeId = dto.DocumentTypeID ?? 0;
        string docTypeName = dto.DocType ?? string.Empty;

        if (documentTypeId == 0 && !string.IsNullOrWhiteSpace(docTypeName))
            documentTypeId =
                _docTypeCache.GetDocumentTypeId(docTypeName);

        if (documentTypeId != 0 && string.IsNullOrWhiteSpace(docTypeName))
            docTypeName =
                _docTypeCache.GetDocumentTypeName(documentTypeId);

        if (documentTypeId == 0)
            throw new InvalidOperationException(
                "Either DocumentTypeID or DocType must be provided.");

        if (!_docTypeCache.IsValidType(documentTypeId))
            throw new InvalidOperationException("Invalid DocumentType.");

        if (!_docTypeCache.IsActiveType(documentTypeId))
            throw new InvalidOperationException("Inactive DocumentType.");

        var documentType = await _db.DocumentTypes
            .Include(t => t.Category)
            .FirstOrDefaultAsync(
                t => t.DocumentTypeID == documentTypeId,
                cancellationToken);

        if (documentType == null)
            throw new InvalidOperationException("DocumentType not found.");

        if (documentType.DocumentCategoryID != dto.DocumentCategoryID)
            throw new InvalidOperationException(
                "DocumentType does not belong to the given category.");

        // ---------------------------------------------------
        // 3 Versioning Logic
        // ---------------------------------------------------
        var existingDocs = await _db.Documents
            .Where(d =>
                d.OwnerTypeID == ownerTypeId &&
                d.OwnerID == dto.OwnerID &&
                d.DocumentTypeID == documentTypeId &&
                d.FileName == dto.File.FileName)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync(cancellationToken);

        var previousActiveDoc =
            existingDocs.FirstOrDefault(d => d.IsActive);

        string newVersion = previousActiveDoc == null
            ? "v1.0"
            : IncrementVersion(previousActiveDoc.Version ?? "v1.0");

        if (previousActiveDoc != null)
            previousActiveDoc.IsActive = false;

        string? fileUrl = null;

        // ---------------------------------------------------
        // 4 Transaction Start
        // ---------------------------------------------------
        using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // ---------------------------------------------------
            // 5 Upload File (Streaming)
            // ---------------------------------------------------
            fileUrl = await _fileStorage.SaveFileAsync(
                dto.File,
                dto.OwnerType,
                dto.OwnerID,
                cancellationToken);

            // ---------------------------------------------------
            // 6 Create Document Entity
            // ---------------------------------------------------
            var document = new Document
            {
                DocumentName = dto.DocumentName,
                DocumentNumber = dto.DocumentNumber,
                OwnerTypeID = ownerTypeId,
                OwnerID = dto.OwnerID,
                DocumentTypeID = documentTypeId,
                DocType = docTypeName,
                DocumentCategoryID = dto.DocumentCategoryID,
                FileName = dto.File.FileName,
                URL = fileUrl,
                UploadedBy = dto.UploadedBy,
                UploadedAt = DateTime.UtcNow,
                Version = newVersion,
                Description = dto.Description,
                IsActive = true,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo
                
            };

            if (previousActiveDoc != null)
                _db.Documents.Update(previousActiveDoc);

            _db.Documents.Add(document);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // ---------------------------------------------------
            // 7 Return DTO
            // ---------------------------------------------------
            return new DocumentDto
            {
                DocumentID = document.DocumentID,
                DocumentName = document.DocumentName,
                DocumentNumber = document.DocumentNumber,
                OwnerTypeID = document.OwnerTypeID,
                OwnerID = document.OwnerID,
                DocumentTypeID = document.DocumentTypeID,
                DocumentTypeName = documentType.TypeName,
                DocumentCategoryID = documentType.DocumentCategoryID,
                DocumentCategoryName =
                    documentType.Category?.CategoryName,
                FileName = document.FileName,
                URL = document.URL,
                UploadedBy = document.UploadedBy,
                UploadedAt = document.UploadedAt,
                Version = document.Version,
                Description = document.Description,
                IsActive = document.IsActive,
                ValidFrom =document.ValidFrom,
                ValidTo = document.ValidTo 
                
            };
        }
        catch
        {
            // ---------------------------------------------------
            // 8 Rollback Safety
            // ---------------------------------------------------
            await transaction.RollbackAsync(cancellationToken);

            if (previousActiveDoc != null)
            {
                previousActiveDoc.IsActive = true;
                _db.Documents.Update(previousActiveDoc);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (!string.IsNullOrEmpty(fileUrl))
            {
                try
                {
                    await _fileStorage.DeleteFileAsync(
                        fileUrl,
                        cancellationToken);
                }
                catch { }
            }

            throw;
        }
    }

    private static string IncrementVersion(string version)
    {
        version = version.Replace("v", "").Trim();
        var parts = version.Split('.');

        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int major) &&
            int.TryParse(parts[1], out int minor))
        {
            return $"v{major}.{minor + 1}";
        }

        return "v1.0";
    }
}
