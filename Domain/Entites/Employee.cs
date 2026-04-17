using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Employee
    {
        public int Id { get; set; }
        public int Salary { get; set; }

        //Relationship
        public int UserId { get; set; }
        public User User { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
