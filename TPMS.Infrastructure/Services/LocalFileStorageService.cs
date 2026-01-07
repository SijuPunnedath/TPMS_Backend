using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Infrastructure.Services
{
    public class LocalFileStorageService :IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalFileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveFileAsync(IFormFile file, string ownerType, int ownerId, CancellationToken cancellationToken)
        {
            string rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string folderPath = Path.Combine(rootPath, "uploads", ownerType.ToLower(), ownerId.ToString());
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, file.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return $"/uploads/{ownerType.ToLower()}/{ownerId}/{file.FileName}";
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
