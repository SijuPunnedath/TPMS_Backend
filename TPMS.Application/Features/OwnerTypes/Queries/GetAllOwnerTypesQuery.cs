using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.OwnerTypes.DTOs;

namespace TPMS.Application.Features.OwnerTypes.Queries;

public record GetAllOwnerTypesQuery(bool IncludeInactive = false) : IRequest<List<OwnerTypeDto>>;