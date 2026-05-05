using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.NotficationDTOs
{
    public class SubmaitCityNotificationDto
    {
        [Required(ErrorMessage = "title is required")]
        public string Title { get; set; }
        public string? Body { get; set; }
        [Required(ErrorMessage = "CityIDs is required")]
        public List<int> CityIDs { get; set; }
    }
}
