using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.VolunteerDTOs
{
    public class VolunteerProfileDto
    {
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public int TotalPoints { get; set; }
        public int CompletedMissionsCount { get; set; } // عدد البلاغات اللي خلصها

        public List<string> EarnedAchievements { get; set; }
    }
}
