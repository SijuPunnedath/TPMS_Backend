using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Tenants.Handlers;

public class GetTenantDocumentsQueryHandler 
    : IRequestHandler<GetTenantDocumentsQuery, List<DocumentDto>>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetTenantDocumentsQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<List<DocumentDto>> Handle(
        GetTenantDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetByOwnerAsync((int)OwnerTypeEnums.Tenant, request.TenantId);
    }
}