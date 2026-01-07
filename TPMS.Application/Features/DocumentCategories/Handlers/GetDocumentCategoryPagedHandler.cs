using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.DocumentCategories.Queries;
using TPMS.Application.Features.DocumentCategory.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentCategories.Handlers;

public class GetDocumentCategoryPagedHandler : IRequestHandler<GetDocumentCategoryPagedQuery, PagedResult<DocumentCategoryDto>>
{
    private readonly TPMSDBContext _db;

    public GetDocumentCategoryPagedHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<PagedResult<DocumentCategoryDto>> Handle(GetDocumentCategoryPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _db.DocumentCategories
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.ToLower().Trim();
            query = query.Where(c => c.CategoryName.ToLower().Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.CategoryName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new DocumentCategoryDto
            {
                DocumentCategoryID = c.DocumentCategoryID,
                CategoryName = c.CategoryName,
                Description = c.Description,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<DocumentCategoryDto>(
            items, totalCount, request.PageNumber, request.PageSize
        );
    }
}