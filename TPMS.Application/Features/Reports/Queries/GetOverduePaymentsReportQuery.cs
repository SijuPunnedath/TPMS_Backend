using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Reports.DTOs;

namespace TPMS.Application.Features.Reports.Queries;

public record GetOverduePaymentsReportQuery(int PageNumber, int PageSize)
    : IRequest<PagedResult<OverduePaymentsReportDto>>;
