using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentCategories.Queries;
using TPMS.Application.Features.DocumentCategory.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentCategories.Handlers;

public class GetAllDocumentCategoriesHandler : IRequestHandler<GetAllDocumentCategoriesQuery, List<DocumentCategoryDto>>
{
    private readonly TPMSDBContext _db;

    public GetAllDocumentCategoriesHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<List<DocumentCategoryDto>> Handle(GetAllDocumentCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.DocumentCategories
            .Where(c => !c.IsDeleted);

     /*   if (!request.IncludeInactive)
            query = query.Where(c => c.IsActive); */

        return await query
            .OrderBy(c => c.CategoryName)
            .Select(c => new DocumentCategoryDto
            {
                DocumentCategoryID = c.DocumentCategoryID,
               CategoryName = c.CategoryName,
                Description = c.Description,
                IsActive = c.IsActive,
                IsDeleted = c.IsDeleted,
                CreatedDate = c.CreatedDate,
                UpdatedAt = c.UpdatedAt
                
            })
            .ToListAsync(cancellationToken);
    }
}