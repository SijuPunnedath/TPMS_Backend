using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Deposit.Commands;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class AddDepositTransactionHandler :
    IRequestHandler<AddDepositTransactionCommand, DepositTransactionDto>
{
    private readonly TPMSDBContext _db;
    public AddDepositTransactionHandler(TPMSDBContext db) => _db = db;

    public async Task<DepositTransactionDto> Handle(AddDepositTransactionCommand request, CancellationToken token)
    {
        var master = await _db.DepositMasters
            .FirstOrDefaultAsync(d => d.DepositMasterID == request.DepositMasterID, token);

        if (master == null)
            throw new Exception("DepositMaster not found");

        // Create transaction
        var tx = new DepositTransaction
        {
            DepositMasterID = request.DepositMasterID,
            Amount = request.Amount,
            TransactionDate = DateTime.UtcNow,
            Type = request.Type,
            Notes = request.Notes
        };

        _db.DepositTransactions.Add(tx);

        //  Update totals
        master.PaidAmount += request.Amount;
        master.BalanceAmount = master.ExpectedAmount - master.PaidAmount;

        //  Update status
        if (master.PaidAmount == 0)
            master.Status = "Pending";
        else if (master.PaidAmount < master.ExpectedAmount)
            master.Status = "PartiallyPaid";
        else if (master.PaidAmount == master.ExpectedAmount)
            master.Status = "Paid";
        else if (master.PaidAmount > master.ExpectedAmount)
            master.Status = "OverPaid";

        await _db.SaveChangesAsync(token);

        return new DepositTransactionDto
        {
            DepositTransactionID = tx.DepositTransactionID,
            DepositMasterID = tx.DepositMasterID,
            Amount = tx.Amount,
            TransactionDate = tx.TransactionDate,
            Type = tx.Type,
            Notes = tx.Notes
        };
    }
}
