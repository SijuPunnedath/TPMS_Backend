using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Deposit.Commands;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class UpdateDepositMasterHandler :
    IRequestHandler<UpdateDepositMasterCommand, DepositMasterDto>
{
    private readonly TPMSDBContext _db;
    public UpdateDepositMasterHandler(TPMSDBContext db) => _db = db;

    public async Task<DepositMasterDto> Handle(UpdateDepositMasterCommand request, CancellationToken token)
    {
        var master = await _db.DepositMasters
            .Include(dm => dm.Transactions)
            .FirstOrDefaultAsync(dm => dm.DepositMasterID == request.DepositMasterID, token);

        if (master == null)
            throw new Exception("DepositMaster not found");

        master.ExpectedAmount = request.ExpectedAmount;
        master.Notes = request.Notes;

        // Recalculate
        master.PaidAmount = master.Transactions.Sum(t => t.Amount);
        master.BalanceAmount = master.ExpectedAmount - master.PaidAmount;

        await _db.SaveChangesAsync(token);

        return new DepositMasterDto
        {
            DepositMasterID = master.DepositMasterID,
            LeaseID = master.LeaseID,
            ExpectedAmount = master.ExpectedAmount,
            PaidAmount = master.PaidAmount,
            BalanceAmount = master.BalanceAmount,
            Status = master.Status,
            Notes = master.Notes
        };
    }
}
