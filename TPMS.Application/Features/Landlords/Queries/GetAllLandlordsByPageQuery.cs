using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Infrastructure.Common.Behaviours;    

namespace TPMS.Application.Features.Landlords.Queries
{
    public class GetAllLandlordsByPageQuery : IRequest<PagedResult<LandlordDto>>
    {
        public int Page { get; }
        public int PageSize { get; }

        public GetAllLandlordsByPageQuery(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize <= 0 ? 10 : pageSize;
        }
    }
}
