using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Landlords.Queries
{
    public class ExportLandlordsQuery : IRequest<byte[]>
    {
        public string Format { get; }   // "csv" or "excel"
        public string? Search { get; }
        public string? SortBy { get; }
        public string? SortOrder { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }

        public ExportLandlordsQuery(string format, string? search, string? sortBy, string? sortOrder, DateTime? startDate, DateTime? endDate)
        {
            Format = format.ToLower();
            Search = search;
            SortBy = sortBy;
            SortOrder = sortOrder?.ToLower() ?? "asc";
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
