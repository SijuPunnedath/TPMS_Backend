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
    public class GetDocumentsByOwnerHandler : IRequestHandler<GetDocumentsByOwnerQuery, List<GetDocsByOwnerIdDto>>
    {
        private readonly TPMSDBContext _db;

        public GetDocumentsByOwnerHandler(TPMSDBContext db)
        {
            _db = db;
        }
        
        public async Task<List<GetDocsByOwnerIdDto>> Handle(
            GetDocumentsByOwnerQuery request,
            CancellationToken cancellationToken)
        {
            var query = _db.Documents
                .AsNoTracking()
                .Include(d => d.DocumentType)
                .Include(d => d.DocumentCategory)
                .Where(d =>
                    d.OwnerTypeID == request.OwnerTypeID &&
                    d.OwnerID == request.OwnerID);

            if (request.DocumentTypeID.HasValue)
            {
                query = query.Where(d =>
                    d.DocumentTypeID == request.DocumentTypeID.Value);
            }

            return await query
                .OrderByDescending(d => d.UploadedAt)
                .Select(d => new GetDocsByOwnerIdDto
                {
                    DocumentID = d.DocumentID,
                    FileName = d.FileName,
                    //FilePath = d.FilePath,
                    //FileSize = d.FileSize,

                    OwnerTypeID = d.OwnerTypeID,
                    OwnerID = d.OwnerID,

                    DocumentTypeID = d.DocumentTypeID,
                    DocumentTypeName = d.DocumentType!.TypeName,

                    DocumentCategoryID = d.DocumentCategoryID,
                    DocumentCategoryName = d.DocumentCategory!.CategoryName,

                    UploadedAt = d.UploadedAt
                })
                .ToListAsync(cancellationToken);
        }

    }
}
