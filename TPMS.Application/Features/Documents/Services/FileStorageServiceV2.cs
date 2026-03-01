using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class FileStorageServiceV2 : IFileStorageServiceV2
{
    private readonly string _basePath;
    private readonly long _maxFileSizeBytes;
    private readonly HashSet<string> _allowedExtensions;
    private readonly HashSet<string> _allowedMimeTypes;


    public FileStorageServiceV2(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"]
                    ?? throw new FileValidationException("BasePath not configured.");

        var maxMb = int.Parse(configuration["FileStorage:MaxFileSizeMB"] ?? "10");
        _maxFileSizeBytes = maxMb * 1024L * 1024L;

        _allowedExtensions = configuration
            .GetSection("FileStorage:AllowedExtensions")
            .Get<string[]>()?
            .Select(x => x.ToLowerInvariant())
            .ToHashSet() ?? new();

        _allowedMimeTypes = configuration
            .GetSection("FileStorage:AllowedMimeTypes")
            .Get<string[]>()?
            .ToHashSet() ?? new();
    }

    public async Task<string> SaveFileAsync(
        IFormFile file,
        string ownerType,
        int ownerId,
        CancellationToken cancellationToken)
    {
        // Validate file exists
        if (file == null || file.Length == 0)
            throw new FileValidationException("File is empty.");

        //  Validate size
                if (file.Length > _maxFileSizeBytes)
                    throw new FileValidationException("File exceeds allowed size.");

        //  Validate extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedExtensions.Contains(extension))
                    throw new FileValidationException(
                        $"File type '{extension}' is not allowed.");

        //  Validate MIME type
                if (!_allowedMimeTypes.Contains(file.ContentType))
                    throw new FileValidationException(
                        $"MIME type '{file.ContentType}' is not allowed.");
                var safeOwnerType = ownerType.Replace(" ", "_");

                var folderPath = Path.Combine(
                    _basePath,
                    safeOwnerType,
                    ownerId.ToString());

                Directory.CreateDirectory(folderPath);

                var safeFileName = Path.GetFileName(file.FileName);

                var filePath = Path.Combine(folderPath, safeFileName);

                await using var stream = new FileStream(
                    filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    81920,
                    useAsync: true);

                await file.CopyToAsync(stream, cancellationToken);

                return filePath;
    }
    public Task DeleteFileAsync(
        string fileUrl,
        CancellationToken cancellationToken)
    {
        if (File.Exists(fileUrl))
            File.Delete(fileUrl);

        return Task.CompletedTask;
    }
}