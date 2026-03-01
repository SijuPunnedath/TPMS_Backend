using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RequiredDocuments.DTOs;
using TPMS.Application.Features.RequiredDocuments.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RequiredDocuments.Handlers;

public class GetRequiredDocumentsQueryHandler 
    : IRequestHandler<GetRequiredDocumentsQuery, List<RequiredDocumentDto>>
{
    private readonly TPMSDBContext _context;

    public GetRequiredDocumentsQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<List<RequiredDocumentDto>> Handle(
        GetRequiredDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.RequiredDocuments
            .AsNoTracking()
            .Include(x => x.DocumentType)
            .Include(x => x.OwnerType)
            .AsQueryable();

        if (request.OwnerTypeID.HasValue)
        {
            query = query.Where(x => x.OwnerTypeID == request.OwnerTypeID);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive);
        }

        return await query
            .Select(x => new RequiredDocumentDto
            {
                RequiredDocumentID = x.RequiredDocumentID,
                OwnerTypeID = x.OwnerTypeID,
                OwnerTypeName = x.OwnerType.Name,

                DocumentTypeID = x.DocumentTypeID,
               // DocumentTypeName = x.DocumentType.Name,

                IsMandatory = x.IsMandatory,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
