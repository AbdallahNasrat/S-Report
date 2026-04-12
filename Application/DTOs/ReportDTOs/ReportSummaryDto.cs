using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class ReportSummaryDto
    {
        public int ReportId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string State { get; set; }
        public string ReportType { get; set; }
        public List<MediaResponseDto> AttachedMedia { get; set; }

    }
}
