using MediatR;

namespace TPMS.Application.Features.OwnerTypes.Commands;

public record DeleteOwnerTypeCommand(int OwnerTypeID) : IRequest<bool>;