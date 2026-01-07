using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Application.Features.Deposit.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class GetDepositTransactionsHandler :
    IRequestHandler<GetDepositTransactionsQuery, List<DepositTransactionDto>>
{
    private readonly TPMSDBContext _db;
    public GetDepositTransactionsHandler(TPMSDBContext db) => _db = db;

    public async Task<List<DepositTransactionDto>> Handle(GetDepositTransactionsQuery request, CancellationToken token)
    {
        return await _db.DepositTransactions
            .Where(t => t.DepositMasterID == request.DepositMasterID)
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new DepositTransactionDto
            {
                DepositTransactionID = t.DepositTransactionID,
                DepositMasterID = t.DepositMasterID,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Type = t.Type,
                Notes = t.Notes
            })
            .ToListAsync(token);
    }
}
