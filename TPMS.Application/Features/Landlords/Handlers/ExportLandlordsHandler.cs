using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class ExportLandlordsHandler : IRequestHandler<ExportLandlordsQuery, byte[]>
    {
        private readonly TPMSDBContext _db;

        public ExportLandlordsHandler(TPMSDBContext db) => _db = db;

        public async Task<byte[]> Handle(ExportLandlordsQuery request, CancellationToken cancellationToken)
        {
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var query = _db.Landlords.AsNoTracking();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(l =>
                    (l.Name ?? "").ToLower().Contains(search) ||
                    (l.Notes ?? "").ToLower().Contains(search));
            }

            if (request.StartDate.HasValue)
                query = query.Where(l => l.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(l => l.CreatedAt <= request.EndDate.Value);

            // Sorting
            query = request.SortBy?.ToLower() switch
            {
                "name" => request.SortOrder == "desc" ? query.OrderByDescending(l => l.Name) : query.OrderBy(l => l.Name),
                "createdat" => request.SortOrder == "desc" ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt),
                "updatedat" => request.SortOrder == "desc" ? query.OrderByDescending(l => l.UpdatedAt) : query.OrderBy(l => l.UpdatedAt),
                _ => query.OrderBy(l => l.LandlordID)
            };

            var landlords = await query
                .Select(l => new
                {
                    l.LandlordID,
                    l.LandlordNumber,
                    l.Name,
                    l.Notes,
                    l.CreatedAt,
                    l.UpdatedAt,
                    Address = _db.Addresses
                        .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == l.LandlordID && a.IsPrimary)
                        .Select(a => a.AddressLine1 + ", " + a.City + ", " + a.State + ", " + a.Country + " " + a.PostalCode)
                        .FirstOrDefault() ?? ""
                })
                .ToListAsync(cancellationToken);

            // Export
            return request.Format switch
            {
                "csv" => ExportCsv(landlords),
                "excel" => ExportExcel(landlords),
                _ => throw new ArgumentException("Invalid export format. Use 'csv' or 'excel'.")
            };
        }

        private byte[] ExportCsv(IEnumerable<object> landlords)
        {
            var sb = new StringBuilder();
            sb.AppendLine("LandlordID,Name,Notes,CreatedAt,UpdatedAt,Address");
            foreach (var l in landlords)
            {
                var row = string.Join(",", l.GetType().GetProperties().Select(p => "\"" + (p.GetValue(l)?.ToString() ?? "") + "\""));
                sb.AppendLine(row);
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private byte[] ExportExcel(IEnumerable<object> landlords)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Landlords");

            // Header
            ws.Cell(1, 1).Value = "LandlordID";
            ws.Cell(1, 2).Value = "Landlord Number";
            ws.Cell(1, 3).Value = "Name";
            ws.Cell(1, 4).Value = "Notes";
            ws.Cell(1, 5).Value = "CreatedAt";
            ws.Cell(1, 6).Value = "UpdatedAt";
            ws.Cell(1, 7).Value = "Address";

            int row = 2;
            foreach (var l in landlords)
            {
                ws.Cell(row, 1).Value = l.GetType().GetProperty("LandlordID")?.GetValue(l)?.ToString();
                ws.Cell(row, 2).Value = l.GetType().GetProperty("LandlordNumber")?.GetValue(l)?.ToString();
                ws.Cell(row, 3).Value = l.GetType().GetProperty("Name")?.GetValue(l)?.ToString();
                ws.Cell(row, 4).Value = l.GetType().GetProperty("Notes")?.GetValue(l)?.ToString();
                ws.Cell(row, 5).Value = l.GetType().GetProperty("CreatedAt")?.GetValue(l)?.ToString();
                ws.Cell(row, 6).Value = l.GetType().GetProperty("UpdatedAt")?.GetValue(l)?.ToString();
                ws.Cell(row, 7).Value = l.GetType().GetProperty("Address")?.GetValue(l)?.ToString();
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

    }
}
