using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Tenants.DTOs;

namespace TPMS.Application.Features.Tenants.Queries;

public record GetAllTenantsQuery(
    bool IncludeDeleted = false,
    string? SearchTerm = null
) : IRequest<List<TenantDto>>;
