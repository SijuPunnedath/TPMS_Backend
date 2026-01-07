using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Documents.Handlers
{

    public class CompleteDocumentUploadHandler : IRequestHandler<CompleteDocumentUploadCommand, string>
    {
        private readonly TPMSDBContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IOwnerTypeCacheService _ownerTypeCache;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<CompleteDocumentUploadHandler> _logger;

        public CompleteDocumentUploadHandler(
            TPMSDBContext db,
            IWebHostEnvironment env,
            IOwnerTypeCacheService ownerTypeCache,
            IFileStorageService fileStorage,
            ILogger<CompleteDocumentUploadHandler> logger)
        {
            _db = db;
            _env = env;
            _ownerTypeCache = ownerTypeCache;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        public async Task<string> Handle(CompleteDocumentUploadCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UploadDto;
            int ownerTypeId = _ownerTypeCache.GetOwnerTypeId(dto.OwnerType);

            var session = await _db.DocumentUploadSessions.FindAsync(dto.SessionId);
            if (session == null)
                throw new InvalidOperationException("Upload session not found.");

            if (session.UploadedChunks < session.TotalChunks)
                throw new InvalidOperationException("Upload session incomplete.");

            string tempFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Temp", dto.SessionId.ToString());
            string mergedFolder = Path.Combine(_env.ContentRootPath, "Uploads", "Merged");
            Directory.CreateDirectory(mergedFolder);
            string finalPath = Path.Combine(mergedFolder, dto.FileName);

            string fileUrl = string.Empty;

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                //  Merge all chunks
                _logger.LogInformation("Merging chunks for session {SessionId}", dto.SessionId);
                var chunkFiles = Directory.GetFiles(tempFolder).OrderBy(f => f);

                using (var finalStream = new FileStream(finalPath, FileMode.Create))
                {
                    foreach (var chunkFile in chunkFiles)
                    {
                        using var chunkStream = new FileStream(chunkFile, FileMode.Open);
                        await chunkStream.CopyToAsync(finalStream, cancellationToken);
                    }
                }

                //  Version Control
                var existingDocs = await _db.Documents
                    .Where(d => d.OwnerTypeID == ownerTypeId &&
                                d.OwnerID == dto.OwnerID &&
                                d.DocType == dto.DocType &&
                                d.FileName == dto.FileName)
                    .OrderByDescending(d => d.UploadedAt)
                    .ToListAsync(cancellationToken);

                string newVersion = "v1.0";
                if (existingDocs.Any())
                {
                    var latest = existingDocs.First();
                    newVersion = IncrementVersion(latest.Version ?? "v1.0");
                    //-- Mark previous as inactive if desired
                    foreach (var old in existingDocs)
                        old.IsActive = false;
                }

                string versionedFileName = $"{Path.GetFileNameWithoutExtension(dto.FileName)}_{newVersion}{Path.GetExtension(dto.FileName)}";

                //-- Upload to permanent storage
                fileUrl = await _fileStorage.SaveFileAsync(finalPath, dto.OwnerType, dto.OwnerID, cancellationToken);

                //-- Create Document Record
                var document = new Document
                {
                    OwnerTypeID = ownerTypeId,
                    OwnerID = dto.OwnerID,
                    DocType = dto.DocType,
                    FileName = versionedFileName,
                    URL = fileUrl,
                    UploadedAt = DateTime.UtcNow,
                    Version = newVersion,
                    IsActive = true
                };

                _db.Documents.Add(document);

                session.IsCompleted = true;
                session.CompletedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Document upload completed successfully: {FileName}", versionedFileName);

                //-- Cleanup temp folder
                Directory.Delete(tempFolder, true);
                if (File.Exists(finalPath)) File.Delete(finalPath);

                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during file merge or save for session {SessionId}", dto.SessionId);

                // -- Rollback DB changes
                await transaction.RollbackAsync(cancellationToken);

                //-- Mark session as failed
                session.IsCompleted = false;
                session.CompletedAt = DateTime.UtcNow;
                session.Status = "Failed";
                session.ErrorMessage = ex.Message;
                await _db.SaveChangesAsync(cancellationToken);

                //-- Cleanup partial files
                SafeDeleteDirectory(tempFolder);
                SafeDeleteFile(finalPath);

                throw new InvalidOperationException($"File upload failed for session {dto.SessionId}. Error: {ex.Message}");
            }
        }

        private static string IncrementVersion(string currentVersion)
        {
            var match = Regex.Match(currentVersion, @"v(\d+)\.(\d+)");
            if (!match.Success) return "v1.0";

            int major = int.Parse(match.Groups[1].Value);
            int minor = int.Parse(match.Groups[2].Value);

            minor++;
            if (minor >= 10)
            {
                major++;
                minor = 0;
            }

            return $"v{major}.{minor}";
        }

        private static void SafeDeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch { /* log later */ }
        }

        private static void SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch { /* log later */ }
        }
    }



}
