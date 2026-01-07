using System;
using MediatR;

namespace TPMS.Application.Features.RenewLease.Commands;

public record TerminateLeaseCommand(
    int LeaseID,
    DateTime TerminationDate,
    DateTime EffectiveEndDate,
    string TerminationType,      // Mutual | Tenant | Landlord | System
    string TerminationReason,
    decimal PenaltyAmount,
    decimal DamageCharges,
    string? Notes,
    int CreatedBy
) : IRequest<int>;