using Application.DTOs.ReportDTOs;
using Domain.Entites;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ReportServices
{
    public  interface IReportService
    {
        public Task<bool> AddReportAsync(int userId, CreateReportDTO dto);
        public Task<SyncResultDto> SyncOfflineReports(int userId,IEnumerable<CreateReportDTO> reports);

        public Task<bool> DeleteReportAsync(int reportId);
        public Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status);
        public Task<IEnumerable<ReportSummaryDto>> GetMyReportsAsync(int userId, int pageNumber, int pageSize);
        public Task<ReportDetailsDto> GetReportByIdAsync(int reportId);
        public Task<ReportSummaryDto> GetReportSummaryByIdAsync(int reportId, bool tracked);
        public Task<bool> AddAILayer(Report report);
        public Task<bool> CancelReportAsync(int reportId);
        public Task<bool> AssignTeamToReportAsync(int reportId, int teamId);
        public Task<bool> ValidateReportAsync(int reportId, bool isValid);
        public Task<IEnumerable<ReportDetailsDto>> GetFilteredReportsAsync(int cityId, int pageNumber, int pageSize, bool excludeResolved);
    }
}
