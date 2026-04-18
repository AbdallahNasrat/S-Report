using Application.Constants;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.UserDTOs;
using Domain.Entites;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;

        public EmployeeService(IUnitOfWork uow )
        {
            _uow = uow;
        }
        public async Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto)
        {
            User userCheck = await _uow.UsersRepo.GetUserByEmailAsync(dto.Email);
            if(userCheck != null) throw new Exception("Email Address is exist");



            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // hashing
                NationalId = dto.NationalId,
                FName = dto.FirstName,
                SName = dto.SecoundName,
                Phone = dto.Phone,
                CityId = dto.CityId,
                Birthdate = dto.Birthdate,
                RoleId = AppRoles.Employee, //employee
                Volunteer = true,
                Address= dto.HomeAddress,      
                Gender = dto.Gender,
            };
            

            await _uow.UsersRepo.AddAsync(user);
            await _uow.SaveChangesAsync(); 


            var employee = new Employee
            {
                UserId = user.Id, 
                Salary = int.Parse(dto.Salary),
                TeamId = dto.TeamId
            };

            await _uow.EmployeesRepo.AddAsync(employee);
            await _uow.SaveChangesAsync() ;
            return true;
        }

        public Task<bool> RegisterEmployeeAsync(RegisterUserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
