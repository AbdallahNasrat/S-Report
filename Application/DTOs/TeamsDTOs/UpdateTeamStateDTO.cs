using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TeamsDTOs
{
    public class UpdateTeamStateDTO
    {
        public int TeamId { get; set; }
        public TeamState State { get; set; }

    }
}
