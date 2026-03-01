using MediatR;
using System.Collections.Generic;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Tenants.Queries;

public class GetTenantDocumentsQuery : IRequest<List<DocumentDto>>
{
    public int TenantId { get; }

    public GetTenantDocumentsQuery(int tenantId)
    {
        TenantId = tenantId;
    }
}