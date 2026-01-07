using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentCategory.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentCategory.Handlers;

public class UpdateDocumentCategoryHandler
    : IRequestHandler<UpdateDocumentCategoryCommand, bool>
{
    private readonly TPMSDBContext _db;

    public UpdateDocumentCategoryHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(UpdateDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _db.DocumentCategories
            .FirstOrDefaultAsync(c => c.DocumentCategoryID == request.Category.DocumentCategoryID, cancellationToken);

        if (category == null)
            return false;

        category.CategoryName = request.Category.CategoryName;
        category.Description = request.Category.Description;
        category.IsActive = request.Category.IsActive;
        //category.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}