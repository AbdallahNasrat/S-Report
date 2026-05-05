using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class UpdateUserInfoDTO
    {
        [Required(ErrorMessage = "enter a first Name ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "enter a Second Name ")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "enter a Home Address ")]

        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "enter a Email ")]

        public string Email { get; set; }

        [Required(ErrorMessage = "enter a Password ")]
        [Length(8,20,ErrorMessage = "Enter a valid password that is at least 8 characters and no more than 20. ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "enter a Phone Number")]
        [Length(11,11,ErrorMessage ="Enter a valid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "enter a CityId ")]
        public int CityId { get; set; }
    }
}
