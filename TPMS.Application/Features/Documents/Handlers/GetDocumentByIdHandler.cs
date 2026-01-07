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
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto?>
    {
        private readonly TPMSDBContext _db;

        public GetDocumentByIdHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<DocumentDto?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var doc = await _db.Documents
                .Include(d => d.OwnerType)
                .FirstOrDefaultAsync(d => d.DocumentID == request.DocumentID, cancellationToken);

            if (doc == null) return null;

            return new DocumentDto
            {
                DocumentID = doc.DocumentID,
                OwnerTypeID = doc.OwnerTypeID,
                OwnerID = doc.OwnerID,
                DocumentTypeID = doc.DocumentTypeID,
                DocType = doc.DocType,
                DocumentCategoryID = doc.DocumentCategoryID,
                DocumentCategoryName = doc.DocumentCategoryName,
                FileName = doc.FileName,
                URL = doc.URL,
                UploadedBy = doc.UploadedBy,
                UploadedAt = doc.UploadedAt,
                Version = doc.Version,
                Description = doc.Description
            };
        }
    }
}
