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

namespace Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IAIService _aiService;
        public ReportService(IUnitOfWork uow, IFileService fileService , IAIService aiService ) {
            _uow = uow;
            _fileService = fileService;
            _aiService = aiService;
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

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReportDetailsDto>> GetFilteredReportsAsync(int cityId, int pageNumber, int pageSize, bool excludeResolved)
        {
            var reports = await _uow.ReportsRepo.GetFilteredReportsAsync(cityId, pageNumber, pageSize, excludeResolved);

            var result = reports.Select(report => new ReportDetailsDto
            {
                ReportId = report.Id,
                Date = report.Date,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                Priority = report.ReportAnalysis.ReportPriority,
                Description = report.Description,
                ReportState = report.State.ToString(),
                IsValid = report.IsValid,
                ReportType = report.ReportAnalysis.ReportType,
                Recommendations = report.ReportAnalysis.Recomendations,
                ConfidenceScore= report.ReportAnalysis.ConfidenceScore,
                ReporterName = $"{report.User.FName} {report.User.SName}",
                ReporterId = report.User.Id,
                City = report.City.Name,
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
                State = r.State.ToString(),
                AttachedMedia = r.Medias.Select(m => new MediaResponseDto()
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            }).ToList();
            return result;
        }

        public async Task<ReportDetailsDto> GetReportByIdAsync(int reportId)
        {
            var report = await _uow.ReportsRepo.GetReportWithDetailsAsync(reportId);
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
                Priority = report.ReportAnalysis.ReportPriority,
                Description = report.Description,
                ReportState = report.State.ToString(),
                IsValid = report.IsValid,
                ReportType = report.ReportAnalysis.ReportType,
                Recommendations = report.ReportAnalysis.Recomendations,
                ConfidenceScore = report.ReportAnalysis.ConfidenceScore,
                ReporterName = $"{report.User.FName} {report.User.SName}",
                ReporterId = report.User.Id,
                City = report.City.Name,
                TeamName = report.Team?.Name ?? "No Team",
                AttachedMedia = report.Medias.Select(m => new MediaResponseDto
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            };
            return result;


        }


        public async Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus newState) {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
            var user = await _uow.UsersRepo.GetByIdAsync(report.UserId);
            if (report == null) {
                throw new NotFoundException("Report does not found");
            }
            if (user == null)
            {
                throw new NotFoundException("user does not found");
            }
            if (report.State == newState) return true;

            if (report.State == ReportStatus.Resolved && newState != ReportStatus.Resolved)
            {
                throw new Exception("Sorry, the status of a report that has already been resolved cannot be changed.");
            }
            report.State = newState;            
            return await _uow.SaveChangesAsync() > 0;
        }
        public async Task<bool> CancelReportAsync(int reportId) {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
            if (report == null) {
                throw new NotFoundException("Report Not Found");
            }
            report.State = ReportStatus.Closed;
            return await _uow.SaveChangesAsync()>0;


        }

        public async Task<bool> AssignTeamToReportAsync(int reportId, int teamId)
        {
            var report = await _uow.ReportsRepo.GetByIdAsync(reportId);

            var team = await _uow.TeamRepo.GetByIdAsync(teamId);

            if (report == null)
            {
                throw new NotFoundException(" the report is does exist");
            }
            if (team == null)
            {
                throw new NotFoundException(" the team is does exist");
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



    }
}
