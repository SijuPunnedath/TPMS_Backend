using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Services;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Tenants.Handlers;

public class GetTenantComplianceQueryHandler 
    : IRequestHandler<GetTenantComplianceQuery, DocumentHealthDto>
{
    private readonly IDocumentQueryService _documentQueryService;

    public GetTenantComplianceQueryHandler(
        IDocumentQueryService documentQueryService)
    {
        _documentQueryService = documentQueryService;
    }

    public async Task<DocumentHealthDto> Handle(
        GetTenantComplianceQuery request,
        CancellationToken cancellationToken)
    {
        return await _documentQueryService
            .GetMissingDocumentsAsync((int)OwnerTypeEnums.Tenant, request.TenantId);
    }
}