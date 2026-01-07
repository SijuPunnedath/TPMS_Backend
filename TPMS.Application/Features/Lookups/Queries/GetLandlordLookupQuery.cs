using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Lookups.DTOs;

namespace TPMS.Application.Features.Lookups.Queries;

public record GetLandlordLookupQuery() 
    : IRequest<List<LandlordLookupDto>>;

