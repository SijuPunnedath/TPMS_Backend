using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Queries
{
    public class GetAllLeasesExtendedQuery : IRequest<PagedResult<LeaseDto>>
    {
        // Filters
        public int? LandlordId { get; set; }
        public int? TenantId { get; set; }
        public int? PropertyId { get; set; }
       // public string? Status { get; set; }
       public string PropertyName { get; set; }
       public LeaseStatus? Status { get; set; } = LeaseStatus.Active;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Keyword Search (Tenant Name, Landlord Name, Property Address)
        public string? SearchTerm { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        // Sorting
        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortDirection { get; set; } = "desc";
    }
}
