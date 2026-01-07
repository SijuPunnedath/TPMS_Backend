using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentTypes.DTOs;
using TPMS.Application.Features.DocumentTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers;

public class GetDocumentTypesByCategoryHandler
    : IRequestHandler<GetDocumentTypesByCategoryQuery, List<DocumentTypeDto>>
{
    private readonly TPMSDBContext _db;

    public GetDocumentTypesByCategoryHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<List<DocumentTypeDto>> Handle(GetDocumentTypesByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _db.DocumentTypes
            .Include(t => t.Category)
            .Where(t => t.DocumentCategoryID == request.CategoryID && t.IsActive)
            .Select(t => new DocumentTypeDto
            {
                DocumentTypeID = t.DocumentTypeID,
                DocumentCategoryID = t.DocumentCategoryID,
                TypeName = t.TypeName,
                Description = t.Description,
                IsActive = t.IsActive,
                CategoryName = t.Category.CategoryName
            })
            .ToListAsync(cancellationToken);
    }
}
