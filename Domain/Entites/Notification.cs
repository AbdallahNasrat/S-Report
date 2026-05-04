using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; } = false;

        //Relationship
        public int? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<City>? Cities { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }



    }
}
