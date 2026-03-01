using System;
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

public class GetDocumentReportHandler :
    IRequestHandler<GetDocumentReportQuery, PagedResult<DocumentReportDto>>
{
    private readonly TPMSDBContext _db;

    public GetDocumentReportHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<DocumentReportDto>> Handle(
        GetDocumentReportQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Documents
            .AsNoTracking()
            .AsQueryable();

        // --------------------
        //  FILTERING
        // --------------------
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            string term = request.SearchTerm.ToLower();

            query = query.Where(d =>
                d.FileName.ToLower().Contains(term) ||
                d.DocType.ToLower().Contains(term) ||
                d.URL.ToLower().Contains(term)
            );
        }

        if (request.OwnerTypeID.HasValue)
            query = query.Where(d => d.OwnerTypeID == request.OwnerTypeID);

        if (request.OwnerID.HasValue)
            query = query.Where(d => d.OwnerID == request.OwnerID);

        if (!string.IsNullOrWhiteSpace(request.DocType))
            query = query.Where(d => d.DocType == request.DocType);

        if (request.FromDate.HasValue)
            query = query.Where(d => d.UploadedAt >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(d => d.UploadedAt <= request.ToDate);

        // --------------------
        //  PAGINATION
        // --------------------
        int totalCount = await query.CountAsync(cancellationToken);

        var documents = await query
            .OrderByDescending(d => d.UploadedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // --------------------
        //  LOAD OWNER TYPE NAMES (Map OwnerTypeID → Name)
        // --------------------
        var ownerTypeMap = await _db.OwnerTypes
            .ToDictionaryAsync(o => o.OwnerTypeID, o => o.Name ?? "", cancellationToken);

        // --------------------
        //  LOAD USER NAMES FOR UploadedBy
        // --------------------
        var userIds = documents
            .Where(d => d.UploadedBy.HasValue)
            .Select(d => d.UploadedBy!.Value)
            .Distinct()
            .ToList();

        var userMap = await _db.Users
            .Where(u => userIds.Contains(u.UserID))
            .ToDictionaryAsync(u => u.UserID, u => u.Username, cancellationToken);

        // --------------------
        // MAP TO DTO
        // --------------------
        var items = documents.Select(d => new DocumentReportDto
        {
            DocumentID = d.DocumentID,
            DocumentNumber = d.DocumentNumber,
            ValidFrom = d.ValidFrom,
            ValidTo = d.ValidTo,
            OwnerType = ownerTypeMap.ContainsKey(d.OwnerTypeID)
                ? ownerTypeMap[d.OwnerTypeID]
                : "Unknown",
            OwnerID = d.OwnerID,
            DocType = d.DocType ?? "",
            FileName = d.FileName ?? "",
            URL = d.URL ?? "",
            Version = d.Version,
            UploadedAt = d.UploadedAt ,
            UploadedByUser = d.UploadedBy.HasValue && userMap.ContainsKey(d.UploadedBy.Value)
                ? userMap[d.UploadedBy.Value]
                : null
        }).ToList();

        return new PagedResult<DocumentReportDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}
