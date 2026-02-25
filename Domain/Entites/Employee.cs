using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    internal class Employee
    {
        public int ID { get; set; }
        public string NationalID { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public DateTime Birthdate { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }

        //Relationship
        public int CityID { get; set; }
        public int TeamID { get; set; }
        public int RoleID { get; set; }
    }
}
