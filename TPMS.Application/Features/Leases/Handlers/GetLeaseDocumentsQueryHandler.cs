using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Handlers;

public class GetLeaseDocumentsQueryHandler 
    : IRequestHandler<GetLeaseDocumentsQuery, List<DocumentDto>>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetLeaseDocumentsQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<List<DocumentDto>> Handle(
        GetLeaseDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetByOwnerAsync((int)OwnerTypeEnums.Lease, request.LeaseId);
    }
}