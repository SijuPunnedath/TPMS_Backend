using MediatR;

namespace TPMS.Application.Features.OwnerTypes.Commands;

public record SoftDeleteOwnerTypeCommand(int OwnerTypeID, int UpdatedBy) : IRequest<bool>;