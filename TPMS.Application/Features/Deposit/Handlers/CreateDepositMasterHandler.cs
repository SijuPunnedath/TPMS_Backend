using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Deposit.Commands;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class CreateDepositMasterHandler :
    IRequestHandler<CreateDepositMasterCommand, DepositMasterDto>
{
    private readonly TPMSDBContext _db;
    public CreateDepositMasterHandler(TPMSDBContext db) => _db = db;

    public async Task<DepositMasterDto> Handle(CreateDepositMasterCommand request, CancellationToken token)
    {
        var deposit = new DepositMaster
        {
            LeaseID = request.LeaseID,
            ExpectedAmount = request.ExpectedAmount,
            PaidAmount = 0,
            BalanceAmount = request.ExpectedAmount,
            Status = "Pending",
            Notes = request.Notes
        };

        _db.DepositMasters.Add(deposit);
        await _db.SaveChangesAsync(token);

        return new DepositMasterDto
        {
            DepositMasterID = deposit.DepositMasterID,
            LeaseID = request.LeaseID,
            ExpectedAmount = deposit.ExpectedAmount,
            PaidAmount = 0,
            BalanceAmount = deposit.ExpectedAmount,
            Status = "Pending",
            Notes = deposit.Notes
        };
    }
}
