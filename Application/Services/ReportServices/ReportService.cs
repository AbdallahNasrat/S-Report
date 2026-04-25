using Application.DTOs.ReportDTOs;
using Application.Services.FileService;
using Application.Services.AiServices;

using Application.Services.AiServices;
using Domain.Entites;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Application.Services.NotficationServices;
using Application.DTOs.LookupDtos;

namespace Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IAIService _aiService;
        private readonly INotificationService _notificationService;
        public ReportService(IUnitOfWork uow, IFileService fileService , IAIService aiService  , INotificationService NotificationService) {
            _uow = uow;
            _fileService = fileService;
            _aiService = aiService;
            _notificationService = NotificationService;
        }
        public async Task<bool> AddAILayer(Report report)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddReportAsync(int userId , CreateReportDTO dto)
        {
            var report = new Report()
            {
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                CityId = dto.CityId,
                UserId = userId,
                Date = DateTime.UtcNow,
                State = ReportStatus.Pending,
                Medias = new List<Media>(),
                Type = dto.ReportType
            };

            if (dto.ImageFiles != null && dto.ImageFiles.Count>0)
            {
                foreach (var image in dto.ImageFiles)
                {
                    var path = await _fileService.SaveReportMediaAsync(image, "images");
                    report.Medias.Add(new Media
                    {
                        FilePath = path,
                        Type = MediaType.Image
                    });
                }
            }
            string? voiceUrl = null;
            
            if (dto.VoiceFile != null) {
                voiceUrl = await _fileService.SaveReportMediaAsync(dto.VoiceFile, "audio");
                report.Medias.Add(new Media
                {
                    FilePath = voiceUrl,
                    Type = MediaType.Audio
                });
            }
            await _uow.ReportsRepo.AddAsync(report);
            var result = await _uow.SaveChangesAsync() > 0;

            if (result) // لو البلاغ اتسيف بنجاح في الداتابيز
            {
                try
                {
                    // 1. حالة وجود صور
                    if (report.Medias.Any(m => m.Type == MediaType.Image) && report.Type == "other")
                    {
                        var imagePaths = report.Medias
                            .Where(m => m.Type == MediaType.Image)
                            .Select(m => m.FilePath).ToList();

                        // تشغيل تحليل الصور في الخلفية
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await _aiService.AnalyzeByImageAsync(report.Id, imagePaths);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Background AI Image Error: {ex.Message}");
                            }
                        }); 
                    }

                    // 2. حالة وجود صوت (عملناها في الخلفية هي كمان عشان الموبايل ميهنجش)
                    else if (report.Medias.Any(m => m.Type == MediaType.Audio) && report.Type == "other")
                    {
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await _aiService.AnalyzeByTextAndVoiceAsync(report.Id, report.Description, voiceUrl);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Background AI Audio Error: {ex.Message}");
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    // الـ Catch الخارجي ده هيصطاد أي خطأ يحصل "قبل" ما الـ Task.Run تشتغل 
                    // (مثلاً لو ضرب Error وهو بيعمل Select لمسارات الصور)
                    Console.WriteLine($"AI Service Setup Error: {ex.Message}");
                }
            }

            return true;
        }
        public async Task<SyncResultDto> SyncOfflineReports(int userId, IEnumerable<CreateReportDTO> reports)
        {
            var result = new SyncResultDto(); 

            foreach (var reportDto in reports)
            {
                try
                {
                    
                    var newReport = await AddReportAsync(userId, reportDto);

                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                
                    result.FailedReports.Add(new FailedReportInfo
                    {
                        Description = reportDto.Description,
                        Error = ex.Message
                    });
                    result.FailureCount++;
                }
            }

            return result;
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReportDetailsDto>> GetFilteredReportsAsync( int pageNumber, int pageSize, bool excludeResolved, int? cityId = null)
        {
            var reports = await _uow.ReportsRepo.GetFilteredReportsAsync( pageNumber, pageSize, excludeResolved, cityId);

            var result = reports.Select(report => new ReportDetailsDto
            {
                ReportId = report.Id,
                Date = report.Date,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                Priority = report.ReportAnalysis?.ReportPriority,
                Description = report.Description,
                ReportState = report.State,
                IsValid = report.IsValid,
                ReportType = report.ReportAnalysis?.ReportType,
                Recommendations = report.ReportAnalysis?.Recomendations,
                ConfidenceScore= report.ReportAnalysis?.ConfidenceScore,
                ReporterName = $"{report.User.FName} {report.User.SName}",
                ReporterId = report.User.Id,
                CityId = report.CityId,
                TeamName = report.Team?.Name ?? "No Team",
                AttachedMedia = report.Medias.Select(m => new MediaResponseDto
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            }).ToList();
            return result;
        }


        public async Task<IEnumerable<ReportSummaryDto>> GetMyReportsAsync(int userId, int pageNumber, int pageSize)
        {
            var MyReports = await _uow.ReportsRepo.GetReportsByUserIdAsync(userId, pageNumber, pageSize);
            var result = MyReports.Select(r => new ReportSummaryDto()
            {
                ReportId = r.Id,
                Description = r.Description,
                Date = r.Date,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                State = r.State,
                AttachedMedia = r.Medias.Select(m => new MediaResponseDto()
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            }).ToList();
            return result;
        }

        public async Task<ReportDetailsDto> GetReportByIdAsync(int reportId,bool tracked = true)
        {
            var report = await _uow.ReportsRepo.GetReportWithDetailsAsync(reportId,tracked);
            if (report == null)
            {
                throw new NotFoundException("NotFoundException");
            }
            var result = new ReportDetailsDto()
            {
                ReportId = report.Id,
                Date = report.Date,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                Priority = report.ReportAnalysis?.ReportPriority,
                Description = report.Description,
                ReportState = report.State,
                IsValid = report.IsValid,
                ReportType = report.ReportAnalysis?.ReportType,
                Recommendations = report.ReportAnalysis?.Recomendations,
                ConfidenceScore = report.ReportAnalysis?.ConfidenceScore,
                ReporterName = $"{report.User.FName} {report.User.SName}",
                ReporterId = report.User.Id,
                CityId = report.CityId,
                TeamName = report.Team?.Name ?? "No Team",
                AttachedMedia = report.Medias.Select(m => new MediaResponseDto
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            };
            return result;


        }
        public async Task<ReportSummaryDto> GetReportSummaryByIdAsync(int reportId,bool tracked) {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId, tracked);
            if (report == null) {
                return null;
            }
            var result = new ReportSummaryDto() {
                ReportId = reportId,
                UserId = report.UserId,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                Date = report.Date,
                Description = report.Description,
                ReportType = report.Type,
                State = report.State,
                AttachedMedia = report.Medias.Select(m => new MediaResponseDto
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            };
            return result;
        }



        public async Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus newState)
        {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
            if (report == null)
            {
                throw new NotFoundException("Report does not found");
            }
            var user = await _uow.UsersRepo.GetByIdAsync(report.UserId);

            if (user == null)
            {
                throw new NotFoundException("user does not found");
            }
            if (report.State == newState) return true;

            if (report.State == ReportStatus.Resolved && newState != ReportStatus.Resolved || report.State == ReportStatus.Closed)
            {
                throw new Exception("Sorry, the status of a report that has already been resolved cannot be changed.");
            }
            report.State = newState;
            var success = await _uow.SaveChangesAsync() > 0;

            if (success && !string.IsNullOrEmpty(user?.FcmToken))
            {
                string title = "Update regarding your report";
                string body = newState switch
                {
                    ReportStatus.InProgress => "We have started working on your report now.",
                    ReportStatus.Resolved => "Your report has been successfully resolved, thank you for your cooperation!",
                    ReportStatus.Closed => "The report has been closed..",
                    _ => $"Your report status is now: {newState}"
                };
                var notificationData = new Dictionary<string, string>{                        
                        { "reportId", report.Id.ToString() },
                        { "click_action", "FLUTTER_NOTIFICATION_CLICK" }, // ضرورية لبعض إصدارات أندرويد
                        { "type", "REPORT_DETAILS" }
                    };

                // نداء سيرفيس النوتفكيشن (بدون ما نعطل الـ Main Flow)
                _ = _notificationService.SendNotificationAsync(user.FcmToken, title, body, notificationData);
            }
            return success;
        }
        public async Task<bool> CancelReportAsync(int reportId) {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
            if (report == null) {
                throw new NotFoundException("Report Not Found");
            }
            if(report.State!= ReportStatus.Closed && report.State != ReportStatus.InProgress && report.State != ReportStatus.Resolved)
                report.State = ReportStatus.Closed;
            return await _uow.SaveChangesAsync()>0;


        }


        public async Task<bool> AssignTeamToReportAsync(int reportId, int teamId)
        {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);

            var team = await _uow.TeamRepo.GetByIdAsync(teamId);

            if (report == null)
            {
                throw new NotFoundException(" the report is does not exist");
            }
            if (team == null)
            {
                throw new NotFoundException(" the team is does not exist");
            }
            report.TeamId = teamId;
            report.State = ReportStatus.InProgress;
            return await _uow.SaveChangesAsync() > 0;
        }
        public async Task<bool> ValidateReportAsync(int reportId, bool isValid) {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
            if (report == null) throw new NotFoundException("The Report does not exist!");

            var user = await _uow.UsersRepo.GetByIdAsync(report.UserId);
            if (user == null) throw new NotFoundException("The User  does not exist!");

            report.IsValid = isValid;

            if (isValid)
            {
                user.Rate += 0.5m;
            }
            else {
                user.Rate -= 1;
            }
            user.Rate = Math.Clamp(user.Rate, 0.0m, 5);

            return await _uow.SaveChangesAsync()>0;
        }
        public async Task<bool> CorrectReportTypeAsync(int reportId, int categoryId)
        {
            var category = await _uow.ReportCategoryRepo.GetByIdAsync(categoryId);
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);

            if (report == null || category == null) return false;

            if (report.ReportAnalysis == null)
            {
                
                report.ReportAnalysis = new ReportAnalysis
                {
                    ReportType = category.Name,
                    ConfidenceScore = 1.0, 
                    ReportPriority = "Medium" 
                };
            }
            else
            {
                
                report.ReportAnalysis.ReportType = category.Name;
            
            }
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<LookupDto>> GetCategories()
        {
            var categories = await _uow.ReportCategoryRepo.GetAllAsync();
            if (categories == null || !categories.Any()) {
                return [];
            }
            var result = categories.Select(c => new LookupDto()
            {
                Id = c.Id,
                Name = c.Name
            });
            return result;
        }
    }
}
