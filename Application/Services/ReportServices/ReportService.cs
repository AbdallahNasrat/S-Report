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
    public class ReportService : IReportService
    {
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

        public Task<IEnumerable<ReportDetailsDto>> GetAllReportsAsync(int cityId)
        {
            throw new NotImplementedException();
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
