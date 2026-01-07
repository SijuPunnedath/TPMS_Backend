using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Properties.DTOs;

namespace TPMS.Application.Features.Properties.Queries
{
    public class GetAllPropertiesExtendedQuery :  IRequest<IEnumerable<PropertyDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Type { get; set; }
        public string? City { get; set; }
        public int? LandlordId { get; set; }

        public string? Search { get; set; } // free text search
        public string? SortBy { get; set; } // field name
        public bool SortDesc { get; set; } = false;
    }
}
