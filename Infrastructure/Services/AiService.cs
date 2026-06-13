using Application.DTOs.ReportDTOs;
using Application.Services.AiServices;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AIService : IAIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUnitOfWork _uow;
    private readonly IWebHostEnvironment _env; // محتاجين ده عشان نجيب المسار الكامل للملف
    private readonly IServiceScopeFactory _scopeFactory;

    public AIService(IHttpClientFactory httpClientFactory, IUnitOfWork uow, IWebHostEnvironment env, IServiceScopeFactory scopeFactory)
    {
        _httpClientFactory = httpClientFactory;
        _uow = uow;
        _env = env;
        _scopeFactory = scopeFactory;
    }

    public async Task AnalyzeByImageAsync(int reportId, List<string> relativePaths)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            try
            {
                var imageClient = _httpClientFactory.CreateClient("ImageAIServer");

                using var content = new MultipartFormDataContent();

                // السيرفر مستني الصورة في خانة اسم "files"
                // لو الموديل بياخد صورة واحدة، هناخد أول مسار في اللستة
                var firstImagePath = relativePaths.FirstOrDefault();
                if (string.IsNullOrEmpty(firstImagePath)) return;

                var physicalPath = Path.Combine(_env.WebRootPath, firstImagePath.TrimStart('/'));

                if (File.Exists(physicalPath))
                {
                    var fileBytes = await File.ReadAllBytesAsync(physicalPath);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    // الكي هنا لازم يكون "files" زي ما الموديل محدد
                    content.Add(fileContent, "files", Path.GetFileName(physicalPath));

                    // نداء الـ Endpoint المسمى /predict
                    var response = await imageClient.PostAsync("/predict", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<AIResponseDto>();
                        await SaveAnalysisResult(reportId, result, unitOfWork);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Error: Token is invalid or expired.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Prediction Failed: {ex.Message}");
            }
        }
    }

    public async Task AnalyzeByTextAndVoiceAsync(int reportId, string description, string? voiceRelativePath)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                var voiceTextClient = _httpClientFactory.CreateClient("VoiceTextAIServer");
                HttpResponseMessage response;

                // ========== حالة الصوت ==========
                if (!string.IsNullOrEmpty(voiceRelativePath))
                {
                    using var content = new MultipartFormDataContent();
                    var physicalPath = Path.Combine(_env.WebRootPath, voiceRelativePath.TrimStart('/'));

                    if (File.Exists(physicalPath))
                    {
                        var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

                        // ⚠️ التعديل هنا: اسم الخانة بقى "file"
                        content.Add(fileContent, "file", Path.GetFileName(physicalPath));
                    }

                    // ⚠️ التعديل هنا: الـ Endpoint بقى "/predict"
                    response = await voiceTextClient.PostAsync("/predict", content);
                }
                // ========== حالة النص ==========
                else
                {
                    // ⚠️ التعديل هنا: الريكويست بقى JSON مش Multipart
                    var textPayload = new { text = description };

                    // ⚠️ التعديل هنا: الـ Endpoint بقى "/predict-text"
                    response = await voiceTextClient.PostAsJsonAsync("/predict-text", textPayload);
                }

                // ========== استلام النتيجة والـ Mapping ==========
                if (response.IsSuccessStatusCode)
                {
                    // بنقرا بالغلاف (Wrapper)
                    var apiResponse = await response.Content.ReadFromJsonAsync<VoiceTextAiWrapperDto>();

                    // نتأكد إن الـ status نجحت وإن فيه result رجع
                    if (apiResponse != null && apiResponse.Status == "success" && apiResponse.Result != null)
                    {
                        // نبعت الداتا اللي جوه (Result) للفانكشن بتاعتنا
                        await SaveAnalysisResult(reportId, apiResponse.Result, unitOfWork);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Voice/Text AI Failed: {ex.Message}");
            }
        }
    }

    private async Task SaveAnalysisResult(int reportId, AIResponseDto response , IUnitOfWork unitOfWork)
    {
        // بناخد أول نتيجة من اللي راجعين
        var aiResult = response.BatchResults.FirstOrDefault();

        if (aiResult != null)
        {
            // تجميع الوحدات في نص واحد (مثال: Police_Units: 1)
            var unitsText = string.Join(", ", aiResult.Recommendation.Units.Select(u => $"{u.Key}: {u.Value}"));

            var analysis = new ReportAnalysis
            {
                ReportId = reportId,
                
                ReportType = aiResult.Type, 
                ReportPriority = aiResult.Priority, // Medium

                Recomendations = $"plan: {aiResult.Recommendation.ActionPlan}. UnitsRequired: {unitsText}",

                ConfidenceScore = 1.0, // قيمة افتراضية            
            };  

            await unitOfWork.ReportAnalysisRepo.AddAsync(analysis);
            var mainReport = await unitOfWork.ReportsRepo.GetByIdAsync(reportId);
            if (mainReport != null)
            {               

                // تحديث النوع فقط لو كان فاضي أو other
                if (string.IsNullOrWhiteSpace(mainReport.Type) || mainReport.Type.Equals("other", StringComparison.OrdinalIgnoreCase))
                {
                    mainReport.Type = aiResult.Type;
                }

                unitOfWork.ReportsRepo.Update(mainReport);
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
    private async Task SaveAnalysisResult(int reportId, VoiceTextAiResponseDto response, IUnitOfWork unitOfWork)
    {
        if (response != null)
        {
            var analysis = new ReportAnalysis
            {
                ReportId = reportId,
                ReportType = response.Category,    
                ReportPriority = response.Priority,
                ConvertedVoiceText = response.Final_Text ?? string.Empty,              
                Recomendations = string.Empty,
                ConfidenceScore = 1.0, // قيمة افتراضية
            };

            await unitOfWork.ReportAnalysisRepo.AddAsync(analysis);
            var mainReport = await unitOfWork.ReportsRepo.GetByIdAsync(reportId);
            if (mainReport != null)
            {

                // تحديث النوع فقط لو كان فاضي أو other
                if (string.IsNullOrWhiteSpace(mainReport.Type) || mainReport.Type.Equals("other", StringComparison.OrdinalIgnoreCase))
                {
                    mainReport.Type = analysis.ReportType;
                }

                unitOfWork.ReportsRepo.Update(mainReport);
            }

            await unitOfWork.SaveChangesAsync();

        }
    }
}