using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.NotficationDTOs
{
    public class SubmaitCityNotificationDto
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public List<int> CityIDs { get; set; }
    }
}
