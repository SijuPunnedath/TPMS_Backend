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
    internal class GetDocumentVersionHistoryHandler : IRequestHandler<GetDocumentVersionHistoryQuery, List<DocumentDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetDocumentVersionHistoryHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<List<DocumentDto>> Handle(GetDocumentVersionHistoryQuery request, CancellationToken cancellationToken)
        {
            int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(request.OwnerType);

            var docs = await _db.Documents
                .Where(d =>
                    d.OwnerTypeID == ownerTypeId &&
                    d.OwnerID == request.OwnerID &&
                    d.DocType == request.DocType &&
                    d.FileName == request.FileName)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            return docs.Select(d => new DocumentDto
            {
                DocumentID = d.DocumentID,
                OwnerTypeID = d.OwnerTypeID,
                OwnerID = d.OwnerID,
                DocType = d.DocType,
                FileName = d.FileName,
                URL = d.URL,
                UploadedBy = d.UploadedBy,
                UploadedAt = d.UploadedAt,
                Version = d.Version,
                Description = d.Description,
                IsActive = d.IsActive
            }).ToList();
        }
    }
}
