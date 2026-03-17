using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class RegisterDto
    {
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string SecoundName { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Genders Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Volunteer { get; set; }
        public int CityId { get; set; }


    }
}
