using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Services;
using TPMS.Application.Features.Documents.DTOs;
using TPMS.Application.Features.Documents.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers;

public class DownloadDocumentByIdHandler : IRequestHandler<DownloadDocumentByIdQuery, DownloadDocumentResult>
{
    private readonly TPMSDBContext _db;
    private readonly IWebHostEnvironment _env;

    public DownloadDocumentByIdHandler(TPMSDBContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<DownloadDocumentResult> Handle(
        DownloadDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var document = await _db.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(d =>
                    d.DocumentID == request.DocumentID &&
                    d.IsActive &&
                    !d.IsDeleted,
                cancellationToken);

        if (document == null)
            throw new FileNotFoundException("Document not found");

        if (string.IsNullOrWhiteSpace(document.URL))
            throw new InvalidOperationException("Document path not available");

        //  Convert relative path to absolute path
        var physicalPath = Path.Combine(
            _env.WebRootPath,
            document.URL.TrimStart('/')
        );

        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("File not found on disk");

        var stream = new FileStream(
            physicalPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,
            useAsync: true
        );

        return new DownloadDocumentResult
        {
            Stream = stream,
            FileName = document.FileName ?? Path.GetFileName(physicalPath),
            ContentType = MimeTypes.GetMimeType(document.FileName)
        };
    }
}