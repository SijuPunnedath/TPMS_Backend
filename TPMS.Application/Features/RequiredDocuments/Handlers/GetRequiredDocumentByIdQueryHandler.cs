using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RequiredDocuments.DTOs;
using TPMS.Application.Features.RequiredDocuments.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RequiredDocuments.Handlers;

public class GetRequiredDocumentByIdQueryHandler 
    : IRequestHandler<GetRequiredDocumentByIdQuery, RequiredDocumentDto>
{
    private readonly TPMSDBContext _context;

    public GetRequiredDocumentByIdQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<RequiredDocumentDto> Handle(
        GetRequiredDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.RequiredDocuments
            .AsNoTracking()
            .Include(x => x.DocumentType)
            .Include(x => x.OwnerType)
            .FirstOrDefaultAsync(x => x.RequiredDocumentID == request.Id, cancellationToken);

        if (entity == null)
            throw new Exception("RequiredDocument not found.");

        return new RequiredDocumentDto
        {
            RequiredDocumentID = entity.RequiredDocumentID,
            OwnerTypeID = entity.OwnerTypeID,
            OwnerTypeName = entity.OwnerType.Name,
            DocumentTypeID = entity.DocumentTypeID,
           // DocumentTypeName = entity.DocumentType.Name,
            IsMandatory = entity.IsMandatory,
            IsActive = entity.IsActive
        };
    }
}
