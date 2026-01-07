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
    public class GetActiveDocumentsHandler : IRequestHandler<GetActiveDocumentsQuery, List<DocumentDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetActiveDocumentsHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<List<DocumentDto>> Handle(GetActiveDocumentsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Documents.AsQueryable().Where(d => d.IsActive);

            // Filter by owner type if provided
            if (!string.IsNullOrEmpty(request.OwnerType))
            {
                int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(request.OwnerType);
                query = query.Where(d => d.OwnerTypeID == ownerTypeId);
            }

            // Filter by owner ID
            if (request.OwnerID.HasValue)
                query = query.Where(d => d.OwnerID == request.OwnerID.Value);

            // Filter by DocType
            if (!string.IsNullOrEmpty(request.DocType))
                query = query.Where(d => d.DocType == request.DocType);

            var docs = await query
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            return docs.Select(d => new DocumentDto
            {
                DocumentID = d.DocumentID,
                OwnerTypeID = d.OwnerTypeID,
                OwnerID = d.OwnerID,
                DocType = d.DocType,
                DocumentTypeID = d.DocumentTypeID,
                DocumentCategoryName = d.DocumentCategoryName,
                FileName = d.FileName,
                URL = d.URL,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Version = d.Version,
                Description = d.Description,
                IsActive = d.IsActive,
                IsDeleted = d.IsDeleted,
                
                
            }).ToList();
        }
    }
}
