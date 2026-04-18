using Application.Constants;
using Application.DTOs.EmployeeDTOs;
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
        public AdminService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto)
        {
            User userCheck = await _uow.UsersRepo.GetUserByEmailAsync(dto.Email);
            if (userCheck != null) throw new Exception("Email Address is exist");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // hashing
                NationalId = dto.NationalId,
                FName = dto.FirstName,
                SName = dto.SecondName,
                Phone = dto.Phone,
                CityId = dto.CityId,
                Birthdate = dto.Birthdate,
                RoleId = AppRoles.Employee, //employee
                Volunteer = true,
                Address = dto.HomeAddress,
                Gender = dto.Gender,
                EmployeeProfile = new Employee { Salary = dto.Salary , TeamId = dto.TeamId }
            };            
            await _uow.UsersRepo.AddAsync(user);
             
            return await _uow.SaveChangesAsync()>0;
        }

    }
}
