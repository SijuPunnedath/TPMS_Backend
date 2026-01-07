using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class GetDeletedDocumentsHandler : IRequestHandler<GetDeletedDocumentsQuery, List<DocumentDto>>
    {
        private readonly TPMSDBContext _db;

        public GetDeletedDocumentsHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<List<DocumentDto>> Handle(GetDeletedDocumentsQuery request, CancellationToken cancellationToken)
        {
            var deletedDocs = await _db.Documents
                .Where(d => d.IsDeleted)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            return deletedDocs.Select(d => new DocumentDto
            {
                DocumentID = d.DocumentID,
                OwnerTypeID = d.OwnerTypeID,
                OwnerID = d.OwnerID,
                DocType = d.DocType,
                DocumentTypeID = d.DocumentTypeID,
                DocumentCategoryID = d.DocumentCategoryID,
                DocumentCategoryName = d.DocumentCategoryName,
                FileName = d.FileName,
                URL = d.URL,
                UploadedAt = d.UploadedAt,
                Version = d.Version,
                IsActive = d.IsActive,
                IsDeleted = d.IsDeleted
            }).ToList();
        }
    }
}
