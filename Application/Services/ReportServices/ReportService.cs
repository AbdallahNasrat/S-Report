using Application.DTOs.ReportDTOs;
using Domain.Entites;
using Domain.Enums;
using Domain.Exceptions;
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
        public async Task<bool> AddAILayer(Report report)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddReportAsync(int userId , CreateReportDTO reportDTO)
        {
            throw new NotImplementedException();
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
        

        public async Task<IEnumerable<ReportSummaryDto>> GetMyReportsAsync(int userId, int pageNumber, int pageSize)
        {
            var MyReports = await _uow.ReportsRepo.GetReportsByUserIdAsync(userId,pageNumber,pageSize);
            var result = MyReports.Select(r => new ReportSummaryDto()
            {
                ReportId = r.Id,
                Description = r.Description,
                Date = r.Date,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                State = r.State.ToString(),
                ReportType = r.ReportType.ToString(),
                AttachedMedia = r.Medias.Select(m => new MediaResponseDto() {
                    FileURL = m.FilePath,
                    MediaType = m.Type.ToString()
                }).ToList()
            }).ToList();        
            return result;
        }

        public async Task<ReportDetailsDto> GetReportByIdAsync(int reportId)
        {
            var report = await _uow.ReportsRepo.GetReportWithDetailsAsync(reportId);
            if (report == null) {
                throw new NotFoundException("NotFoundException");
            }
            var result = new ReportDetailsDto()
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
