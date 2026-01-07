using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Reports.DTOs;

namespace TPMS.Application.Features.Reports.Queries;

public record GetPropertyReportQuery(
    string? Search,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResult<PropertyReportDto>>;
