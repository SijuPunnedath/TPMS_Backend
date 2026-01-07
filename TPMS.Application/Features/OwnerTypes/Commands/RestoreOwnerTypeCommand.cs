using MediatR;

namespace TPMS.Application.Features.OwnerTypes.Commands;

public record RestoreOwnerTypeCommand(int OwnerTypeID, int UpdatedBy) : IRequest<bool>;