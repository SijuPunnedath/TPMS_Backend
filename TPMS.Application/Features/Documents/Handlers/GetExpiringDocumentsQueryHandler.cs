using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;

public class GetExpiringDocumentsQueryHandler : IRequestHandler<GetExpiringDocumentsQuery, List<ExpiringDocumentDto>>
{
    private readonly TPMSDBContext _context;

    public GetExpiringDocumentsQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<List<ExpiringDocumentDto>> Handle(
        GetExpiringDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var targetDate = today.AddDays(request.DaysAhead);

        // 1 Fetch from DB (EF-safe)
        var documents = await _context.Documents
            .Where(d => d.ValidTo != null
                        && d.ValidTo >= today
                        && d.ValidTo <= targetDate
                        && d.IsActive
                        && !d.IsArchived
                        && !d.IsDeleted)
            .Select(d => new
            {
                d.DocumentID,
                d.DocumentName,
                d.OwnerTypeID,
                d.OwnerID,
                d.ValidTo
            })
            .ToListAsync(cancellationToken);

        // 2 In-memory calculation
        return documents
            .Select(d => new ExpiringDocumentDto
            {
                DocumentID = d.DocumentID,
                DocumentName = d.DocumentName,
                OwnerTypeID = d.OwnerTypeID,
                OwnerID = d.OwnerID,
                ValidTo = d.ValidTo!.Value,
                DaysRemaining = (d.ValidTo!.Value.Date - today).Days
            })
            .OrderBy(d => d.DaysRemaining)
            .ToList();
    }

}