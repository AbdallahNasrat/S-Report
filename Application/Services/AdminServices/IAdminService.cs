using Application.DTOs.EmployeeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AdminServices
{
    public interface IAdminService
    {
        public Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto, int role);
        public Task<bool> AddAdminAsync(RegisterEmployeeDto dto);

    }
}
