using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Infrastructure.Services
{
    public class AzureBlobFileStorageService : IFileStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IWebHostEnvironment _env;

        public AzureBlobFileStorageService(IConfiguration config, IWebHostEnvironment env)
        {
            string connectionString = config["StorageSettings:Azure:ConnectionString"]!;
            string containerName = config["StorageSettings:Azure:ContainerName"]!;
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
            _env = env;
        }

        public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken)
        {
            try
            {
                var uri = new Uri(fileUrl);
                var blobName = uri.AbsolutePath.TrimStart('/');
                var blobClient = _containerClient.GetBlobClient(blobName);

                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"❌ Error deleting Azure blob: {ex.Message}");
                throw;
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string ownerType, int ownerId, CancellationToken cancellationToken)
        {
            string blobName = $"{ownerType.ToLower()}/{ownerId}/{file.FileName}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

            return blobClient.Uri.ToString();
        }

        public async Task<byte[]> GetFileBytesAsync(string fileUrl, CancellationToken cancellationToken)
        {
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found at {fileUrl}");
            return await File.ReadAllBytesAsync(fullPath, cancellationToken);
        }

        // Overload for merged file path
        public async Task<string> SaveFileAsync(string localFilePath, string ownerType, int ownerId, CancellationToken cancellationToken)
        {
            string fileName = Path.GetFileName(localFilePath);
            string destinationFolder = Path.Combine(_env.ContentRootPath, "Uploads", ownerType, ownerId.ToString());
            Directory.CreateDirectory(destinationFolder);

            string destinationPath = Path.Combine(destinationFolder, fileName);
            File.Copy(localFilePath, destinationPath, overwrite: true);

            // optional: delete local temp file after copy
            await Task.CompletedTask;
            return $"/Uploads/{ownerType}/{ownerId}/{fileName}";
        }
        
        public async Task<Stream> OpenReadAsync(string relativePath, CancellationToken ct)
        {
            var root = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");

            var fullPath = Path.Combine(
                root,
                relativePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found on disk.");

            return new FileStream(
                fullPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);
        }

    }
}
