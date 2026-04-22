using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class AiDTO
    {
        public string? Type { get; set; }
        public string? Priority { get; set; }
        public string? Recommendations { get; set; }
        public double? ConfidenceScore { get; set; }


    }
}
