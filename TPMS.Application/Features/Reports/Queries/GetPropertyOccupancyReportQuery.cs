using MediatR;
using TPMS.Application.Features.Reports.DTOs;

namespace TPMS.Application.Features.Reports.Queries;

public class GetPropertyOccupancyReportQuery
    : IRequest<PropertyOccupancyReportDto>
{
    public int? LandlordId { get; set; }
}