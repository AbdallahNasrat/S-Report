using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.VolunteerDTOs
{
    public class CurrentMissionDto
    {
        public int ReportId { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CityName { get; set; }
        public DateTime AcceptedAt { get; set; }
    }
}
