
using Application.Services.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;


namespace Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
      

        public async Task<string> SaveReportMediaAsync(IFormFile file, string mediaType)
        {
            if (file == null || file.Length == 0) {
                throw new ArgumentException("file is not exist or bad");
            }
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            var dynamicFolder = Path.Combine("uploads", "reports", year, month, day, mediaType);
            var physicalPath = Path.Combine(_env.WebRootPath, dynamicFolder);

            if (!Directory.Exists(physicalPath)) {
                Directory.CreateDirectory(physicalPath);
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPhysicalPath=Path.Combine(physicalPath, uniqueFileName);

            //Save file on hard
            using (var fileStream = new FileStream(fullPhysicalPath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
            var finalPath = $"/{dynamicFolder.Replace("\\", "/")}/{uniqueFileName}";
            return finalPath;
        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) {
                throw new Exception("not found");
            }
            var physicalPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);              
            }
            return true;
        }
    }
}
