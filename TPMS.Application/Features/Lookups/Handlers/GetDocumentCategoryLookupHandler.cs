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

public class GetDocumentCategoryLookupHandler : IRequestHandler<GetDocumentCategoryLookupQuery, List<DocumentCategoryLookupDto>>
{
    private readonly TPMSDBContext _db;

    public GetDocumentCategoryLookupHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<List<DocumentCategoryLookupDto>> Handle(
        GetDocumentCategoryLookupQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.DocumentCategories
            .Where(c => !c.IsDeleted && c.IsActive)
            .OrderBy(c => c.CategoryName)
            .Select(c => new DocumentCategoryLookupDto
            {
                DocumentCategoryID = c.DocumentCategoryID,
                CategoryName = c.CategoryName
            })
            .ToListAsync(cancellationToken);
    }
}