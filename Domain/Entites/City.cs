using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relationships
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<City> Cities { get; set; } = new HashSet<City>();

    }
}
