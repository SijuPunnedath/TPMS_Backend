using MediatR;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Commands;

public class CreateLeaseSettlementCommand : IRequest<LeaseSettlementDto>
{
    public int LeaseId { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal DamageCharges { get; set; }
}