using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relationships
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
