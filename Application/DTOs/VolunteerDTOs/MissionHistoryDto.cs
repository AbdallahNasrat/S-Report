using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.VolunteerDTOs
{
    public class MissionHistoryDto
    {
        public int ReportId { get; set; }
        public string Description { get; set; }
        public DateTime CompletedAt { get; set; } 
        public int EarnedPoints { get; set; } 
    }
}
