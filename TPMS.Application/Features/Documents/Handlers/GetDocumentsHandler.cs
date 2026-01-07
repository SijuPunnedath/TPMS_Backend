using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;

public class GetDocumentsHandler : IRequestHandler<GetDocumentsQuery, List<DocumentListItemDto>>
{

    private readonly TPMSDBContext _db;

    public GetDocumentsHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<List<DocumentListItemDto>> Handle(
        GetDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Documents
            .AsNoTracking()
            .Include(d => d.DocumentType)
            .ThenInclude(dt => dt.Category)
            .Where(d =>
                d.OwnerTypeID == request.OwnerTypeID &&
                d.OwnerID == request.OwnerID &&
                !d.IsDeleted);

        // 🔹 Category filter
        if (request.DocumentCategoryID.HasValue)
        {
            query = query.Where(d =>
                d.DocumentCategoryID == request.DocumentCategoryID.Value);
        }

        // 🔹 Type filter
        if (request.DocumentTypeID.HasValue)
        {
            query = query.Where(d =>
                d.DocumentTypeID == request.DocumentTypeID.Value);
        }

        // 🔹 Active / archived filter
        if (!request.IncludeArchived)
        {
            query = query.Where(d => d.IsActive);
        }

        return await query
            .OrderByDescending(d => d.IsActive)
            .ThenByDescending(d => d.UploadedAt)
            .Select(d => new DocumentListItemDto
            {
                DocumentID = d.DocumentID,
                DocumentName = d.DocumentName,

                DocumentTypeID = d.DocumentTypeID,
                DocumentTypeName = d.DocumentType!.TypeName,

                DocumentCategoryID = d.DocumentCategoryID,
                DocumentCategoryName = d.DocumentType!.Category!.CategoryName,

                Version = d.Version,
                IsActive = d.IsActive,

                UploadedAt = d.UploadedAt,
                UploadedBy = d.UploadedBy,

                FileName = d.FileName
            })
            .ToListAsync(cancellationToken);
    }
}