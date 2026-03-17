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
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string ReportState { get; set; }
        public bool? IsValid { get; set; }
        public string? AiResult { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public string TeamName { get; set; }


    }
}
