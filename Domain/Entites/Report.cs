using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Description { get; set; }
        public ReportStatus State { get; set; }
        public bool? IsValid { get; set; }


        //Relationship
        public int UserId { get; set; }
        public User User { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public ICollection<Media> Medias { get; set; } = new List<Media>();

        public ReportAnalysis? ReportAnalysis { get; set; }

        public ICollection<ReportVolunteer> AssignedVolunteers { get; set; }

    }
}
