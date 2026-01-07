using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentCategories.Queries;
using TPMS.Application.Features.DocumentCategory.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentCategories.Handlers;

public class GetDocumentCategoryByIdHandler : IRequestHandler<GetDocumentCategoryByIdQuery, DocumentCategoryDto?>
{
    private readonly TPMSDBContext _db;

    public GetDocumentCategoryByIdHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<DocumentCategoryDto?> Handle(GetDocumentCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _db.DocumentCategories
            .FirstOrDefaultAsync(c => c.DocumentCategoryID == request.CategoryID && !c.IsDeleted, cancellationToken);

        if (category == null)
            return null;

        return new DocumentCategoryDto
        {
            DocumentCategoryID = category.DocumentCategoryID,
            CategoryName = category.CategoryName,
            Description = category.Description,
            IsActive = category.IsActive,
            IsDeleted = category.IsDeleted,
            UpdatedAt = category.UpdatedAt
            
        };
    }
}