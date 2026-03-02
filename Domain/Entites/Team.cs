using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Relationship
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();

    }
}
