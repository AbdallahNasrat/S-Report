using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class LoginResponseDTO
    {
        public string Token;
        public string FirstName;
        public string Role;
        public int UserId;
        public int CityId;
        public int? EmployeeId;
    }
}
