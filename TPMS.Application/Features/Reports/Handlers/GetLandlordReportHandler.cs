using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Reports.DTOs;
using TPMS.Application.Features.Reports.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Reports.Handlers;

public class GetLandlordReportHandler 
    : IRequestHandler<GetLandlordReportQuery, PagedResult<LandlordReportDto>>
{
    private readonly TPMSDBContext _db;

    public GetLandlordReportHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<LandlordReportDto>> Handle(
        GetLandlordReportQuery request,
        CancellationToken cancellationToken)
    {
        // Resolve OwnerTypeID for Landlord
        var ownerTypeId = await _db.OwnerTypes
            .Where(o => o.Name == "Landlord")
            .Select(o => o.OwnerTypeID)
            .FirstAsync(cancellationToken);

        // Base query (single SQL)
        IQueryable<LandlordReportDto> query =
            from l in _db.Landlords.AsNoTracking()
            where !l.IsDeleted

            join a in _db.Addresses.AsNoTracking()
                on new { OwnerID = l.LandlordID, OwnerTypeID = ownerTypeId }
                equals new { OwnerID = a.OwnerID, OwnerTypeID = a.OwnerTypeID }
                into addressJoin

            from addr in addressJoin
                .Where(x => x.IsPrimary)
                .DefaultIfEmpty()

            select new LandlordReportDto
            {
                LandlordID = l.LandlordID,
                Name = l.Name,

                PrimaryEmail = addr.Email,

                AddressLine1 = addr.AddressLine1,
                AddressLine2 = addr.AddressLine2,
                City = addr.City,
                State = addr.State,
                PostalCode = addr.PostalCode,
                Country = addr.Country,

                PropertyCount = _db.Properties
                    .Count(p => p.LandlordID == l.LandlordID),

                ActiveLeasesCount = _db.Leases
                    .Count(ls =>
                        ls.LandlordID == l.LandlordID &&
                        !ls.IsDeleted),

                TotalRentCollected =
                    _db.RentSchedules
                        .Where(rs =>
                            rs.IsPaid &&
                            rs.Lease.LandlordID == l.LandlordID)
                        .Sum(rs =>
                            (decimal?)(rs.Amount + (rs.Penalty ?? 0))) ?? 0
            };

        // Search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.Name.Contains(search));
        }

        // Total count (before pagination)
        var total = await query.CountAsync(cancellationToken);

        // Pagination
        var data = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<LandlordReportDto>(
            data,
            total,
            request.PageNumber,
            request.PageSize);
    }
}
