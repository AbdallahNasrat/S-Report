using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class CreateReportDTO
    {
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }     
        public int ReportTypeId { get; set; }


    }
}
