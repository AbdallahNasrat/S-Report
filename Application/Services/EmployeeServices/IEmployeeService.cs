using Application.DTOs.EmployeeDTOs;
using Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        public Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto);
    }
}
