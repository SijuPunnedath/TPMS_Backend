using Amazon.Runtime.Internal;
using MediatR;
using TPMS.Application.Features.OwnerTypes.DTOs;

namespace TPMS.Application.Features.OwnerTypes.Commands;

public record UpdateOwnerTypeCommand(OwnerTypeDto OwnerType, int UpdatedBy) : IRequest<bool>;