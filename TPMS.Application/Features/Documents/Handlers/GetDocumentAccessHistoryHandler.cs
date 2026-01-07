using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class GetDocumentAccessHistoryHandler : IRequestHandler<GetDocumentAccessHistoryQuery, List<DocumentAccessLogDto>>
    {
        private readonly TPMSDBContext _db;

        public GetDocumentAccessHistoryHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<List<DocumentAccessLogDto>> Handle(GetDocumentAccessHistoryQuery request, CancellationToken cancellationToken)
        {
            var query = _db.DocumentAccessLogs
                .Include(l => l.Document)
                .AsQueryable();

            // Filters
            if (request.DocumentID.HasValue)
                query = query.Where(l => l.DocumentID == request.DocumentID.Value);

            if (!string.IsNullOrWhiteSpace(request.AccessedBy))
                query = query.Where(l => l.AccessedBy!.ToLower() == request.AccessedBy.ToLower());

            if (request.FromDate.HasValue)
                query = query.Where(l => l.AccessedAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(l => l.AccessedAt <= request.ToDate.Value);

            var logs = await query
                .OrderByDescending(l => l.AccessedAt)
                .ToListAsync(cancellationToken);

            return logs.Select(l => new DocumentAccessLogDto
            {
                LogID = l.LogID,
                DocumentID = l.DocumentID,
                FileName = l.Document?.FileName,
                DocType = l.Document?.DocType,
                AccessedBy = l.AccessedBy,
                AccessedAt = l.AccessedAt,
                AccessType = l.AccessType,
                IPAddress = l.IPAddress,
                Device = l.Device,
                Notes = l.Notes,
            }).ToList();
        }

    }
}
