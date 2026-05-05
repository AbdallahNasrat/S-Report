using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "NationalId Is Required")]
        [Length(14,14,ErrorMessage = "Enter a valid NationalID")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "FirstName Is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "SecondName Is Required")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "HomeAddress Is Required")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage ="Enter a valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [Length(8,20,ErrorMessage = "Enter a valid password that is at least 8 characters and no more than 20. ") ]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Is Required")]
        [Length(11, 11, ErrorMessage = "Enter a valid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Gender Is Required")]
        public Genders Gender { get; set; }

        [Required(ErrorMessage = "Birthdate Is Required")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        public bool Volunteer { get; set; } = true;

        [Required(ErrorMessage = "CityId Is Required")]
        public int CityId { get; set; }


    }
}
