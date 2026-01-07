using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentCategory.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentCategory.Handlers;

public class SoftDeleteDocumentCategoryHandler 
    : IRequestHandler<SoftDeleteDocumentCategoryCommand, bool>
{
    private readonly TPMSDBContext _db;

    public SoftDeleteDocumentCategoryHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(SoftDeleteDocumentCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _db.DocumentCategories
            .FirstOrDefaultAsync(c => c.DocumentCategoryID == request.CategoryID, cancellationToken);

        if (category == null)
            return false;

        category.IsActive = false;
        category.IsDeleted = false;
        category.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
