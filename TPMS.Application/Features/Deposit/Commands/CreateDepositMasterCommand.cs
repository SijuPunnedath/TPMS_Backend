using MediatR;
using TPMS.Application.Features.Deposit.DTOs;

namespace TPMS.Application.Features.Deposit.Commands;

public record CreateDepositMasterCommand(int LeaseID, decimal ExpectedAmount, string? Notes)
    : IRequest<DepositMasterDto>;
