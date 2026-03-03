using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class City
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        //Relationships
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
        public ICollection<User> Users { get; set; } = new HashSet<User>();

    }
}
