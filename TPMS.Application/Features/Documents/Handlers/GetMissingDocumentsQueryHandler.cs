using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;
using DateTime = System.DateTime;

namespace TPMS.Application.Features.Documents.Handlers;

public class GetMissingDocumentsQueryHandler : IRequestHandler<GetMissingDocumentsQuery, DocumentHealthDto>
{
 private readonly TPMSDBContext _context;

        public GetMissingDocumentsQueryHandler(TPMSDBContext context)
        {
            _context = context;
        }

        public async Task<DocumentHealthDto> Handle(
            GetMissingDocumentsQuery request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Required documents for this OwnerType
            var requiredDocs = await _context.RequiredDocuments
                .Where(r => r.OwnerTypeID == request.OwnerTypeID && r.IsActive)
                .Include(r => r.DocumentType)
                    .ThenInclude(dt => dt.Category)
                .ToListAsync(cancellationToken);

            // 2️⃣ Uploaded + valid documents
            var uploadedDocTypeIds = await _context.Documents
                .Where(d => d.OwnerTypeID == request.OwnerTypeID
                         && d.OwnerID == request.OwnerID
                         && d.IsActive
                         && !d.IsArchived
                         && !d.IsDeleted
                         && (d.ValidTo == null || d.ValidTo >= DateTime.UtcNow))
                .Select(d => d.DocumentTypeID)
                .Distinct()
                .ToListAsync(cancellationToken);

            // 3️⃣ Missing documents
            var missingDocs = requiredDocs
                .Where(r => !uploadedDocTypeIds.Contains(r.DocumentTypeID))
                .Select(r => new MissingDocumentDto
                {
                    DocumentTypeID = r.DocumentTypeID,
                    DocumentTypeName = r.DocumentType!.TypeName,
                    CategoryName = r.DocumentType.Category!.CategoryName,
                    IsMandatory = r.IsMandatory
                })
                .ToList();

            return new DocumentHealthDto
            {
                OwnerTypeID = request.OwnerTypeID,
                OwnerID = request.OwnerID,
                TotalRequired = requiredDocs.Count,
                Uploaded = uploadedDocTypeIds.Count,
                Missing = missingDocs.Count,
                MissingDocuments = missingDocs
            };
        }    
}