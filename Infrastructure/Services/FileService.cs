
using Application.Services.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;


namespace Infrastructure.Services
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
                throw new ArgumentException("file does not exist or bad");
            }
            var extension = Path.GetExtension(file.FileName).ToLower();

            string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg" };
            string[] allowedAudioExtensions = { ".mp3", ".wav", ".m4a", ".aac", ".ogg", ".amr" };

            // دمج اللستتين مع بعض
            var allAllowedExtensions = allowedImageExtensions.Concat(allowedAudioExtensions).ToArray();

            if (!allAllowedExtensions.Contains(extension))
            {
                throw new Exception($"File Type ({extension}) Does not support Now, only audio and video.");
            }
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            var dynamicFolder = Path.Combine("uploads", "reports", year, month, day, mediaType);
            var physicalPath = Path.Combine(_env.WebRootPath, dynamicFolder);

            if (!Directory.Exists(physicalPath)) {
                Directory.CreateDirectory(physicalPath);
            }
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
