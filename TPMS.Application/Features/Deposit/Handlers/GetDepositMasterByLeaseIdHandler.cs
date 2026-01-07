using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Application.Features.Deposit.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class GetDepositMasterByLeaseIdHandler :
    IRequestHandler<GetDepositMasterByLeaseIdQuery, DepositMasterDto>
{
    private readonly TPMSDBContext _db;
    public GetDepositMasterByLeaseIdHandler(TPMSDBContext db) => _db = db;

    public async Task<DepositMasterDto> Handle(GetDepositMasterByLeaseIdQuery request, CancellationToken token)
    {
        var master = await _db.DepositMasters
            .Include(d => d.Transactions)
            .FirstOrDefaultAsync(d => d.LeaseID == request.LeaseID, token);

        if (master == null)
            throw new Exception("Deposit record not found");

        return new DepositMasterDto
        {
            DepositMasterID = master.DepositMasterID,
            LeaseID = master.LeaseID,
            ExpectedAmount = master.ExpectedAmount,
            PaidAmount = master.PaidAmount,
            BalanceAmount = master.BalanceAmount,
            Status = master.Status,
            Notes = master.Notes,
            Transactions = master.Transactions.Select(t => new DepositTransactionDto
            {
                DepositTransactionID = t.DepositTransactionID,
                DepositMasterID = t.DepositMasterID,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Type = t.Type,
                Notes = t.Notes
            }).ToList()
        };
    }
}
