using Application.Constants;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public int Salary { get; set; }
        public string Phone { get; set; }
        public Genders Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public int CityId { get; set; }
        public int? TeamId { get; set; }

    }
}
