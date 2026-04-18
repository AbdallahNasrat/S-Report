using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
