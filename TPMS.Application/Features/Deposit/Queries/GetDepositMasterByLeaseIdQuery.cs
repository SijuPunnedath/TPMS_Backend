using MediatR;
using TPMS.Application.Features.Deposit.DTOs;

namespace TPMS.Application.Features.Deposit.Queries;

public record GetDepositMasterByLeaseIdQuery(int LeaseID) : IRequest<DepositMasterDto>;
