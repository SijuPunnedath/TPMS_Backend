using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Lookups.DTOs;

namespace TPMS.Application.Features.Lookups.Queries;

public record GetAvailableInboundPropertiesQuery() 
    : IRequest<List<PropertyLookupDto>>;