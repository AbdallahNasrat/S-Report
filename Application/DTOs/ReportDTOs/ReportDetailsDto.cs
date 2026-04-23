using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class ReportDetailsDto
    {
        public int ReportId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Priority { get; set; }
        public string? ReportType { get; set; }
        public ReportStatus ReportState { get; set; }
        public bool? IsValid { get; set; }
        public string? Recommendations { get; set; }
        public double? ConfidenceScore { get; set; }
        public string ReporterName { get; set; }
        public int ReporterId { get; set; }
        public int CityId { get; set; }
        public string TeamName { get; set; }
        public List<MediaResponseDto> AttachedMedia { get; set; }


    }
}
