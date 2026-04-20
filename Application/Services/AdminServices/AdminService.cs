using Application.Constants;
using Application.DTOs.EmployeeDTOs;
using Application.Services.EmployeeServices;
using Application.Services.UserServices;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeService _employeeService;
        public AdminService(IUnitOfWork uow ,IEmployeeService employeeService)
        {
            _uow = uow;
            _employeeService  = employeeService;
        }

        public async Task<bool> AddAdminAsync(RegisterEmployeeDto dto)
        {
            var newAdmin = await _employeeService.RegisterEmployeeAsync(dto, AppRoles.Admin);
            return newAdmin;
        }

        

    }
}
