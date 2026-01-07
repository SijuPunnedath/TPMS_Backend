using MediatR;
using TPMS.Application.Features.RenewLease.DTOs;

namespace TPMS.Application.Features.RenewLease.Queries;

public record GetLeaseTerminationSummaryQuery(int LeaseTerminationID)
    : IRequest<LeaseTerminationSummaryDto>;