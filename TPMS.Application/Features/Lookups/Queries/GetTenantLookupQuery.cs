using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Lookups.DTOs;

namespace TPMS.Application.Features.Lookups.Queries;

public record GetTenantLookupQuery() : IRequest<List<TenantLookupDto>>;