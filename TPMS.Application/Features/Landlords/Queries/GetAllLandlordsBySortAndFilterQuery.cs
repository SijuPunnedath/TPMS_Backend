using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Infrastructure.Common.Behaviours;

namespace TPMS.Application.Features.Landlords.Queries
{
    public  class GetAllLandlordsBySortAndFilterQuery : IRequest<PagedResult<LandlordDto>>
    {
        public int Page { get; }
        public int PageSize { get; }
        public string? Search { get; }
        public string? SortBy { get; }
        public string? SortOrder { get; }

        public GetAllLandlordsBySortAndFilterQuery(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            Search = search;
            SortBy = sortBy;
            SortOrder = sortOrder?.ToLower() ?? "asc";
        }
    }
}
