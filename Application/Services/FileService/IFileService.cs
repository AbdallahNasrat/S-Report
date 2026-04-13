using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FileService
{
    public interface IFileService
    {
        Task<string> SaveReportMediaAsync(IFormFile file, string MediaType);
        bool DeleteFile(string relativePath);
    }
}
