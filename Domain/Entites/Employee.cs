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
        public string NationalId { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public DateTime Birthdate { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Genders Gender { get; set; }

        //Relationship

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
