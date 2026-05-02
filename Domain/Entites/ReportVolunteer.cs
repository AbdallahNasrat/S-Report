using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class ReportVolunteer
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int VolunteerId { get; set; }
        public User Volunteer { get; set; }
        public VolunteerMissionStatus Status { get; set; } = VolunteerMissionStatus.Joined;
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }
}
