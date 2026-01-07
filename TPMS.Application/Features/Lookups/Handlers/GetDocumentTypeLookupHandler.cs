using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Application.Features.Lookups.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Handlers;

public class GetDocumentTypeLookupHandler 
    : IRequestHandler<GetDocumentTypeLookupQuery, List<DocumentTypeLookupDto>>
{
    private readonly TPMSDBContext _db;
    public GetDocumentTypeLookupHandler(TPMSDBContext db) => _db = db;

    public async Task<List<DocumentTypeLookupDto>> Handle(GetDocumentTypeLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.DocumentTypes
            .Include(dt => dt.Category)
            .Where(dt => dt.IsActive)                    // Only active types
            .OrderBy(dt => dt.Category.CategoryName)     // Category sorted first
            .ThenBy(dt => dt.TypeName)                   // Then by type name
            .Select(dt => new DocumentTypeLookupDto
            {
                DocumentTypeID = dt.DocumentTypeID,
                DocumentCategoryID = dt.DocumentCategoryID,
                TypeName = dt.TypeName,
                CategoryName = dt.Category.CategoryName
            })
            .ToListAsync(cancellationToken);
    }
}

