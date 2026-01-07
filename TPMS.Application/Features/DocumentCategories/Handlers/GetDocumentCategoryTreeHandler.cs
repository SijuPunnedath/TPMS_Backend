
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentCategories.Queries;
using TPMS.Application.Features.DocumentCategory.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Application.Features.DocumentTypes.DTOs;
namespace TPMS.Application.Features.DocumentCategories.Handlers;


public class GetDocumentCategoryTreeHandler 
    : IRequestHandler<GetDocumentCategoryTreeQuery, List<DocumentCategoryTreeDto>>
{
    private readonly TPMSDBContext _db;

    public GetDocumentCategoryTreeHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<List<DocumentCategoryTreeDto>> Handle(
        GetDocumentCategoryTreeQuery request,
        CancellationToken cancellationToken)
    {
        // Load categories with their types
        var categories = await _db.DocumentCategories
            .Include(c => c.DocumentTypes)
            .OrderBy(c => c.CategoryName)
            .Select(c => new DocumentCategoryTreeDto
            {
                DocumentCategoryID = c.DocumentCategoryID,
                CategoryName = c.CategoryName,

                Types = c.DocumentTypes
                    .OrderBy(t => t.TypeName)
                    .Select(t => new DocumentTypeNodeDto
                    {
                        DocumentTypeID = t.DocumentTypeID,
                        TypeName = t.TypeName,
                        Description = t.Description,
                        IsActive = t.IsActive
                    }).ToList()
            })
            .ToListAsync(cancellationToken);

        return categories;
    }
}
