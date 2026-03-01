using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Handlers;

public class GetLeaseComplianceQueryHandler 
    : IRequestHandler<GetLeaseComplianceQuery, DocumentHealthDto>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetLeaseComplianceQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<DocumentHealthDto> Handle(
        GetLeaseComplianceQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetMissingDocumentsAsync((int)OwnerTypeEnums.Lease, request.LeaseId);
    }
}