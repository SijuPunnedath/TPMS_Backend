using MediatR;

namespace TPMS.Application.Features.Deposit.Commands;

public record DeleteDepositMasterCommand(int DepositMasterID) : IRequest<bool>;
