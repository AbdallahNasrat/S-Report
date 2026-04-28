using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.NotficationDTOs
{
    public class AllNotificationsInSystemDto
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public DateTime date { get; set; }
        public int? userId { get; set; }
        public List<string>? cityName { get; set; }

    }
}
