using MediatR;
using TPMS.Application.Features.OwnerTypes.DTOs;

namespace TPMS.Application.Features.OwnerTypes.Queries;

public record GetOwnerTypeByIdQuery(int OwnerTypeID) : IRequest<OwnerTypeDto?>;