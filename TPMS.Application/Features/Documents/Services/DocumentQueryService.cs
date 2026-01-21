using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;
using DateTime = System.DateTime;

namespace TPMS.Application.Features.Documents.Services;

public class DocumentQueryService
{
   
        private readonly TPMSDBContext _context;

        public DocumentQueryService(TPMSDBContext context)
        {
            _context = context;
        }

        public async Task<DocumentHealthDto> GetMissingDocumentsAsync(int ownerTypeId, int ownerId)
        {
            // 1. Required documents for this owner type
            var requiredDocs = await _context.RequiredDocuments
                .Where(r => r.OwnerTypeID == ownerTypeId && r.IsActive)
                .Include(r => r.DocumentType)
                    .ThenInclude(dt => dt.Category)
                .ToListAsync();

            // 2. Uploaded & valid documents
            var uploadedDocTypeIds = await _context.Documents
                .Where(d => d.OwnerTypeID == ownerTypeId
                         && d.OwnerID == ownerId
                         && d.IsActive
                         && !d.IsArchived
                         && !d.IsDeleted
                         && (d.ValidTo == null || d.ValidTo >= DateTime.UtcNow))
                .Select(d => d.DocumentTypeID)
                .Distinct()
                .ToListAsync();

            // 3. Missing documents
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
                OwnerTypeID = ownerTypeId,
                OwnerID = ownerId,
                TotalRequired = requiredDocs.Count,
                Uploaded = uploadedDocTypeIds.Count,
                Missing = missingDocs.Count,
                MissingDocuments = missingDocs
            };
        }
    }
