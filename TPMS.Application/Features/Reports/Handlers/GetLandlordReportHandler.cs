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

public class GetLandlordReportHandler : IRequestHandler<GetLandlordReportQuery, PagedResult<LandlordReportDto>>
{
    private readonly TPMSDBContext _db;
    public GetLandlordReportHandler(TPMSDBContext db) => _db = db;

    public async Task<PagedResult<LandlordReportDto>> Handle(GetLandlordReportQuery request, CancellationToken cancellationToken)
    {
        var q = _db.Landlords.AsNoTracking().Where(l => !l.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim().ToLower();
            q = q.Where(l => (l.Name ?? "").ToLower().Contains(s));
        }

        var landlords = await q.ToListAsync(cancellationToken);
        var ids = landlords.Select(l => l.LandlordID).ToList();

        var ownerTypeId = await _db.OwnerTypes.Where(o => o.Name == "Landlord")
            .Select(o => o.OwnerTypeID).FirstAsync(cancellationToken);

        var addresses = await _db.Addresses
            .Where(a => a.OwnerTypeID == ownerTypeId && a.IsPrimary)
            .ToListAsync(cancellationToken);
        
        var landlordIds = ids.ToList(); // Make sure it's List<int>

        var propertyCounts = await _db.Properties
            .Where(p => landlordIds.Contains((int)p.LandlordID))   // force primitive cast
            .GroupBy(p => p.LandlordID)
            .Select(g => new { LandlordID = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);


       // var activeLeases = await _db.Leases
         //   .Where(l => ids.Contains(l.LandlordID) && !l.IsDeleted)
           // .GroupBy(l => l.LandlordID)
            //.Select(g => new { g.Key, Count = g.Count() })
            //.ToListAsync(cancellationToken);

            var activeLeases = await _db.Leases
                .Where(l =>
                    l.LandlordID.HasValue &&
                    ids.Contains(l.LandlordID.Value) &&
                    !l.IsDeleted)
                .GroupBy(l => l.LandlordID.Value)
                .Select(g => new
                {
                    LandlordID = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

        var rentCollected = await _db.RentSchedules
            .Where(rs => rs.IsPaid)
            .GroupBy(rs => rs.Lease.LandlordID)
            .Select(g => new { g.Key, Amount = g.Sum(x => x.Amount + (x.Penalty ?? 0)) })
            .ToListAsync(cancellationToken);

        var result = landlords.Select(l =>
        {
            var addr = addresses.FirstOrDefault(a => a.OwnerID == l.LandlordID);
            var pc = propertyCounts.FirstOrDefault(x => x.LandlordID == l.LandlordID);
            var al = activeLeases.FirstOrDefault(x => x.LandlordID == l.LandlordID);
            var rc = rentCollected.FirstOrDefault(x => x.Key == l.LandlordID);

            return new LandlordReportDto
            {
                LandlordID = l.LandlordID,
                Name = l.Name,
                PrimaryEmail = addr?.Email,
                PropertyCount = pc?.Count ?? 0,
                ActiveLeasesCount = al?.Count ?? 0,
                TotalRentCollected = rc?.Amount ?? 0
            };
        }).ToList();

        // Pagination
        var total = result.Count;
        var paged = result.Skip((request.PageNumber - 1) * request.PageSize)
                          .Take(request.PageSize)
                          .ToList();

        return new PagedResult<LandlordReportDto>(paged, total, request.PageNumber, request.PageSize);
    }
}