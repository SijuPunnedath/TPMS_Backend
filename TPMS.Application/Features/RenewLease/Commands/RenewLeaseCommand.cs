using System;
using MediatR;

namespace TPMS.Application.Features.RenewLease.Commands;

public record RenewLeaseCommand(
    int LeaseID,
    DateTime NewStartDate,
    DateTime NewEndDate,
    decimal NewRent,
    decimal? NewDeposit,
    string RenewalReason,
    int RenewedBy
) : IRequest<int>;