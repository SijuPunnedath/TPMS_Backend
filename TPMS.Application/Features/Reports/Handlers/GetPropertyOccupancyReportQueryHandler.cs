using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Reports.DTOs;
using TPMS.Application.Features.Reports.Queries;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Reports.Handlers;

public class GetPropertyOccupancyReportQueryHandler
    : IRequestHandler<GetPropertyOccupancyReportQuery, PropertyOccupancyReportDto>
{
    private readonly TPMSDBContext _context;

    public GetPropertyOccupancyReportQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<PropertyOccupancyReportDto> Handle(
        GetPropertyOccupancyReportQuery request,
        CancellationToken cancellationToken)
    {
        // Base query: Properties
        IQueryable<Property> propertiesQuery = _context
            .Set<Property>()
            .AsNoTracking();

        if (request.LandlordId.HasValue)
        {
            propertiesQuery = propertiesQuery
                .Where(p => p.LandlordID == request.LandlordId.Value);
        }

        var totalProperties = await propertiesQuery
            .CountAsync(cancellationToken);

        // Occupied = has at least one ACTIVE lease
        var occupiedProperties = await propertiesQuery
            .CountAsync(p =>
                    _context.Leases.Any(l =>
                        l.Property.PropertyID == p.PropertyID &&
                        l.StartDate <= DateTime.UtcNow &&
                        l.EndDate >= DateTime.UtcNow
                    ),
                cancellationToken
            );

        return new PropertyOccupancyReportDto
        {
            TotalProperties = totalProperties,
            OccupiedProperties = occupiedProperties,
            VacantProperties = totalProperties - occupiedProperties
        };
    }
}
