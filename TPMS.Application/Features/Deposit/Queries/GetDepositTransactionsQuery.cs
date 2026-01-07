using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Deposit.DTOs;

namespace TPMS.Application.Features.Deposit.Queries;

public record GetDepositTransactionsQuery(int DepositMasterID)
    : IRequest<List<DepositTransactionDto>>;
