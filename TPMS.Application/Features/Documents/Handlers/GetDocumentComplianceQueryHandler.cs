using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;

public class GetDocumentComplianceQueryHandler : IRequestHandler<GetDocumentComplianceQuery, DocumentComplianceDto>
{
    private readonly TPMSDBContext _context;

    public GetDocumentComplianceQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<DocumentComplianceDto> Handle(
        GetDocumentComplianceQuery request,
        CancellationToken cancellationToken)
    {
        // 1 Total required documents
        var totalRequired = await _context.RequiredDocuments
            .Where(r => r.OwnerTypeID == request.OwnerTypeID && r.IsActive)
            .CountAsync(cancellationToken);

        if (totalRequired == 0)
        {
            return new DocumentComplianceDto
            {
                OwnerTypeID = request.OwnerTypeID,
                OwnerID = request.OwnerID,
                TotalRequired = 0,
                Uploaded = 0,
                CompliancePercentage = 100
            };
        }

        // 2 Uploaded + valid documents
        var uploadedCount = await _context.Documents
            .Where(d => d.OwnerTypeID == request.OwnerTypeID
                        && d.OwnerID == request.OwnerID
                        && d.IsActive
                        && !d.IsArchived
                        && !d.IsDeleted
                        && (d.ValidTo == null || d.ValidTo >= DateTime.UtcNow))
            .Select(d => d.DocumentTypeID)
            .Distinct()
            .CountAsync(cancellationToken);

        // 3 Compliance calculation
        var compliance = Math.Round(
            (decimal)uploadedCount / totalRequired * 100, 2);

        return new DocumentComplianceDto
        {
            OwnerTypeID = request.OwnerTypeID,
            OwnerID = request.OwnerID,
            TotalRequired = totalRequired,
            Uploaded = uploadedCount,
            CompliancePercentage = compliance
        };
    } 
}