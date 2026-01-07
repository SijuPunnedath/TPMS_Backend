using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Queries
{
    public class GetAllLeasesBySearchTermQuery :IRequest<List<LeaseWithSearchTermDto>>
    {
        public string? SearchTerm { get; set; }
    }
}
