using MediatR;
using TPMS.Application.Features.Deposit.DTOs;

namespace TPMS.Application.Features.Deposit.Commands;

public record UpdateDepositMasterCommand(int DepositMasterID, decimal ExpectedAmount, string? Notes)
    : IRequest<DepositMasterDto>;