using System;
using TPMS.Application.Features.Reports.DTOs;

namespace TPMS.Application.Features.Reports.Queries;

using MediatR;
using TPMS.Application.Common.Models;

public class GetLeaseExpiryReportQuery : IRequest<PagedResult<LeaseExpiryReportDto>>
{
    public DateTime? FromDate { get; set; }      // e.g. Today
    public DateTime? ToDate { get; set; }        // e.g. Today + 30 days

    public int? TenantId { get; set; }
    public int? LandlordId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
