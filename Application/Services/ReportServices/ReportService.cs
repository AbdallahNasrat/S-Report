using Application.DTOs.ReportDTOs;
using Domain.Entites;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ReportServices
{
    public class ReportService : IReportService
    {
        IUnitOfWork _uow;
        public ReportService(IUnitOfWork uow) {
            _uow = uow;
        }
        public Task<bool> AddAILayer(Report report)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddReportAsync(CreateReportDTO reportDTO, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteReportAsync(int reportId)
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
                Priority = report.Priority.ToString(),
                Description = report.Description,
                ReportState = report.State.ToString(),
                IsValid = report.IsValid,
                AiResult = report.AiResult,
                ReporterName = $"{report.User.FName} {report.User.SName}", 
                ReporterId = report.User.Id,
                City = report.City.Name,
                TeamName = report.Team?.Name ?? "No Team",
                AttachedMedia = report.Medias.Select(m => new MediaResponseDto
                {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()}).ToList();
            return result;
        }
        

        public Task<IEnumerable<ReportSummaryDto>> GetMyReportsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ReportDetailsDto> GetReportById(int reportId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
