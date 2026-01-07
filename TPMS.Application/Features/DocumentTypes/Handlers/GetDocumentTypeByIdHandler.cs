using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentTypes.DTOs;
using TPMS.Application.Features.DocumentTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentTypes.Handlers;

public class GetDocumentTypeByIdHandler
    : IRequestHandler<GetDocumentTypeByIdQuery, DocumentTypeDto>
{
    private readonly TPMSDBContext _db;

    public GetDocumentTypeByIdHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<DocumentTypeDto> Handle(GetDocumentTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var type = await _db.DocumentTypes
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.DocumentTypeID == request.DocumentTypeID);

        if (type == null)
            throw new Exception("DocumentType not found");

        return new DocumentTypeDto
        {
            DocumentTypeID = type.DocumentTypeID,
            DocumentCategoryID = type.DocumentCategoryID,
            TypeName = type.TypeName,
            Description = type.Description,
            IsActive = type.IsActive,
            CategoryName = type.Category.CategoryName
        };
    }
}
