using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Queries;

public class GetLeasesByTypeQuery : IRequest<List<LeaseWithSearchTermDto>>
{
    public LeaseType LeaseType { get; set; }  // or "Inbound"
    public string? SearchTerm { get; set; }
    
   
}