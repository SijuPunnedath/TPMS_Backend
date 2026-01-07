using MediatR;
using TPMS.Application.Features.Deposit.DTOs;

namespace TPMS.Application.Features.Deposit.Commands;

public record AddDepositTransactionCommand(
    int DepositMasterID,
    decimal Amount,
    string Type,
    string? Notes
) : IRequest<DepositTransactionDto>;
