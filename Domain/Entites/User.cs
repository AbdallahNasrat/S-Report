using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class User
    {
        public int ID { get; set; }
        public string NationalID { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Genders Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Volunteer { get; set; }
        public int Rate { get; set; }

        //Relationship
        public int RoleID { get; set; }


    }
}
