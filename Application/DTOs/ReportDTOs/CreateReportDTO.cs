using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class CreateReportDTO
    {
        [Required(ErrorMessage ="Description Is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Latitude Is Required")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude Is Required")]

        public decimal Longitude { get; set; }

        [Required(ErrorMessage ="Description Is Required")]

        public string? ReportType { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public IFormFile? VoiceFile { get; set; }

        [Required(ErrorMessage = "CityId Is Required")]
        public int CityId { get; set; }

    }
}   
