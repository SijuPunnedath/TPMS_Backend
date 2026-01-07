using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class GetDocumentHistoryQueryHandler : IRequestHandler<GetDocumentHistoryQuery, List<DocumentHistoryDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;
        private readonly IDocumentTypeCacheService _docTypeCache;

        public GetDocumentHistoryQueryHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache,
            IDocumentTypeCacheService docTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
            _docTypeCache = docTypeCache;
        }

        public async Task<List<DocumentHistoryDto>> Handle(GetDocumentHistoryQuery request, CancellationToken cancellationToken)
        {
            int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(request.OwnerType);

            // Base query
            var query = _db.Documents
                .Where(d => d.OwnerTypeID == ownerTypeId && d.OwnerID == request.OwnerID)
                .AsQueryable();

            // Optional filter by DocumentType
            if (!string.IsNullOrWhiteSpace(request.DocumentTypeName))
            {
                int docTypeId = _docTypeCache.GetDocumentTypeId(request.DocumentTypeName);
                query = query.Where(d => d.DocumentTypeID == docTypeId);
            }

            var docs = await query
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            return docs.Select(d => new DocumentHistoryDto
            {
                DocumentID = d.DocumentID,
                DocType = d.DocType,
                FileName = d.FileName,
                URL = d.URL,
                Version = d.Version,
                UploadedAt = d.UploadedAt,
                UploadedByName = d.UploadedBy.HasValue ? $"User-{d.UploadedBy}" : null, // Replace later with user join
                IsActive = d.IsActive,
                Description = d.Description
            }).ToList();
        }
    }
}   
