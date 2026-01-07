using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;



public class ViewDocumentHandler
    : IRequestHandler<ViewDocumentQuery, ViewDocumentResult>
{
    private readonly TPMSDBContext _db;
    private readonly IFileStorageService _fileStorage;

    public ViewDocumentHandler(
        TPMSDBContext db,
        IFileStorageService fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<ViewDocumentResult> Handle(
        ViewDocumentQuery request,
        CancellationToken cancellationToken)
    {
        var document = await _db.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.DocumentID == request.DocumentID &&
                     d.IsDeleted == false,
                cancellationToken);

        if (document == null)
            throw new FileNotFoundException("Document not found.");

        if (string.IsNullOrWhiteSpace(document.URL))
            throw new InvalidOperationException("Document file path missing.");

        //  Resolve absolute file path
        var stream = await _fileStorage.OpenReadAsync(
            document.URL, cancellationToken);

        //  Detect content type
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(
                document.FileName ?? string.Empty,
                out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return new ViewDocumentResult
        {
            Stream = stream,
            ContentType = contentType
        };
    }
}
