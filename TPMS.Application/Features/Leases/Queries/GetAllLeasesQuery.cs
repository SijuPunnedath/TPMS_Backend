using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Queries
{
    public class GetAllLeasesQuery :IRequest<List<LeaseDto>>
    {
        public int? LandlordId { get; set; }
        public int? TenantId { get; set; }
        public int? PropertyId { get; set; }
       // public string? Status { get; set; }
       public LeaseStatus? Status { get; set; } = LeaseStatus.Active;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
