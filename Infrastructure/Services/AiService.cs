using Application.DTOs.ReportDTOs;
using Application.Services.AiServices;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _uow;
    private readonly IWebHostEnvironment _env; // محتاجين ده عشان نجيب المسار الكامل للملف

    public AIService(HttpClient httpClient, IUnitOfWork uow, IWebHostEnvironment env)
    {
        _httpClient = httpClient;
        _uow = uow;
        _env = env;
    }

    public async Task AnalyzeByImageAsync(int reportId, List<string> relativePaths)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(reportId.ToString()), "report_id");

            foreach (var path in relativePaths)
            {
                // تحويل المسار النسبي لمسار حقيقي على الهارد ديسك
                var physicalPath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));

                if (File.Exists(physicalPath))
                {
                    var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);

                    // تحديد نوع الملف (Image/png أو jpeg)
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/" + Path.GetExtension(physicalPath).Replace(".", ""));

                    content.Add(fileContent, "images", Path.GetFileName(physicalPath));
                }
            }

            var response = await _httpClient.PostAsync("http://ai-server/analyze-image", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AiDTO>();
                await SaveAnalysisResult(reportId, result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image AI Upload Failed: {ex.Message}");
        }
    }

    public async Task AnalyzeByTextAndVoiceAsync(int reportId, string description, string? voiceRelativePath)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(reportId.ToString()), "report_id");
            content.Add(new StringContent(description), "description");

            if (!string.IsNullOrEmpty(voiceRelativePath))
            {
                var physicalPath = Path.Combine(_env.WebRootPath, voiceRelativePath.TrimStart('/'));
                if (File.Exists(physicalPath))
                {
                    var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg"); // أو حسب الامتداد

                    content.Add(fileContent, "voice", Path.GetFileName(physicalPath));
                }
            }

            var response = await _httpClient.PostAsync("http://ai-server/analyze-text-voice", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AiDTO>();
                await SaveAnalysisResult(reportId, result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Voice AI Upload Failed: {ex.Message}");
        }
    }

    private async Task SaveAnalysisResult(int reportId, AiDTO result)
    {
        var analysis = new ReportAnalysis
        {
            ReportId = reportId,
            ReportType = result.Type,
            ReportPriority = result.Priority,
            ConfidenceScore = result.ConfidenceScore,

        };
        await _uow.ReportAnalysisRepo.AddAsync(analysis);
        await _uow.SaveChangesAsync();
    }
}