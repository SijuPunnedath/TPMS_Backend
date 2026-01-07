using MediatR;
using TPMS.Application.Features.OwnerTypes.DTOs;

namespace TPMS.Application.Features.OwnerTypes.Commands;

public record CreateOwnerTypeCommand(OwnerTypeDto OwnerType, int CreatedBy) : IRequest<int>;

