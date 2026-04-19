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

        public async Task<bool> AddAdminAsync(RegisterEmployeeDto dto)
        {
            var newAdmin = await RegisterEmployeeAsync(dto, AppRoles.Admin);
            return newAdmin;
        }

        public async Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto , int role=2)
        {
            User userCheck = await _uow.UsersRepo.GetUserByEmailAsync(dto.Email);
            if (userCheck != null) throw new Exception("Email Address is exist");
            userCheck = await _uow.UsersRepo.GetFirstOrDefaultAsync(u => u.NationalId == dto.NationalId);
            if (userCheck != null) { throw new Exception("the National ID is used"); }
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
                RoleId = role, //employee
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
