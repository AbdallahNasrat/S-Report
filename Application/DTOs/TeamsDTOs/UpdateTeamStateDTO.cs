using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TeamsDTOs
{
    public class UpdateTeamStateDTO
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public TeamState State { get; set; }

    }
}
