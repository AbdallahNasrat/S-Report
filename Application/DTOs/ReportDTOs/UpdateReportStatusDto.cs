using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReportDTOs
{
    public class UpdateReportStatusDto
    {
        [Required]
        public ReportStatus Status { get; set; }
    }
}
