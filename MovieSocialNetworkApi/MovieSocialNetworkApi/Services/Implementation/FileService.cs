using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger _logger;

        public FileService(
            ILogger<FileService> logger
        )
        {
            _logger = logger;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            try
            {
                if (file == null) return null;

                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return dbPath;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
