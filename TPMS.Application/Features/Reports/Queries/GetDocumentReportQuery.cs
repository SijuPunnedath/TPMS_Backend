using System;
using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Reports.DTOs;

namespace TPMS.Application.Features.Reports.Queries;

public class GetDocumentReportQuery : IRequest<PagedResult<DocumentReportDto>>
{
    public string? SearchTerm { get; set; }
    public int? OwnerTypeID { get; set; }
    public int? OwnerID { get; set; }
    public string? DocType { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}