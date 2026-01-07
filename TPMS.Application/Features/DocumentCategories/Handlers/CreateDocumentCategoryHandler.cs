using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.DocumentCategory.Commands;
using TPMS.Infrastructure.Persistence.Configurations;
namespace TPMS.Application.Features.DocumentCategories.Handlers;

public class CreateDocumentCategoryHandler 
    : IRequestHandler<CreateDocumentCategoryCommand, int>
{
    private readonly TPMSDBContext _db;

    public CreateDocumentCategoryHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(CreateDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        var cat = new Domain.Entities.DocumentCategory
        { 
            CategoryName = request.Category.CategoryName,
            Description = request.Category.Description,
            IsDeleted = false,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
            
        };

        _db.DocumentCategories.Add(cat);
        await _db.SaveChangesAsync(cancellationToken);

        return cat.DocumentCategoryID;
    }
}
