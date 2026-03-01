using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Tenants.Queries;

public class GetTenantComplianceQuery : IRequest<DocumentHealthDto>
{
    public int TenantId { get; }

    public GetTenantComplianceQuery(int tenantId)
    {
        TenantId = tenantId;
    }
}