using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NearbyMissionResponseDto
    {
        public int ReportId { get; set; }
        public string Description { get; set; } // أو العنوان بتاع البلاغ
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal DistanceInMeters { get; set; } // هدية لبتاع فلاتر عشان يعرضها
        public ReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
