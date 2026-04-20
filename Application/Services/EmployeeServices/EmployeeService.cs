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
        private EmployeeDTO MapToDTO(Employee emp)
        {
            return new EmployeeDTO
            {
                EmployeeId = emp.Id,
                FirstName = emp.User.FName,
                SecondName = emp.User.SName,
                CityId = emp.User.CityId,
                Birthdate = emp.User.Birthdate,
                NationalId = emp.User.NationalId,
                Gender = emp.User.Gender,
                Email = emp.User.Email,               
                HomeAddress = emp.User.Address,
                Phone = emp.User.Phone,
                RoleId = emp.User.RoleId,
                Salary = emp.Salary,
                TeamId = emp.TeamId
            };
        }

        public async Task<bool> EditEmployeeDataAsync(EmployeeDTO dto)
        {
            var existEmployee = await _uow.EmployeesRepo.GetEmployeeByIdAsync(dto.EmployeeId);
            if (existEmployee == null) {
                return false;
            }
            existEmployee.User.FName = dto.FirstName;
            existEmployee.User.SName = dto.SecondName;
            existEmployee.User.Address = dto.HomeAddress;
            existEmployee.User.Phone = dto.Phone;
            existEmployee.User.Email = dto.Email;
            existEmployee.User.Birthdate = dto.Birthdate;
            existEmployee.User.NationalId = dto.NationalId;
            existEmployee.User.Gender = dto.Gender;
            existEmployee.User.CityId = dto.CityId;
            existEmployee.User.RoleId = dto.RoleId;

            existEmployee.TeamId = dto.TeamId;
            existEmployee.Salary = dto.Salary;

            return await _uow.SaveChangesAsync() > 0 ;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _uow.EmployeesRepo.GetAllEmployeesAsync();
            if (employees == null) return null;
            return employees.Select(emp => MapToDTO(emp));
        }

        public async Task<EmployeeDTO> GetEmployeeAsync(int id)
        {
            var emp = await _uow.EmployeesRepo.GetEmployeeByIdAsync(id);
            if (emp == null) return null;
            var result = new EmployeeDTO()
            {
                EmployeeId = emp.Id,
                FirstName = emp.User.FName,
                SecondName = emp.User.SName,
                CityId = emp.User.CityId,
                Birthdate = emp.User.Birthdate,
                NationalId = emp.User.NationalId,
                Gender = emp.User.Gender,
                Email = emp.User.Email,
                HomeAddress = emp.User.Address,
                Phone = emp.User.Phone,
                RoleId = emp.User.RoleId,

                Salary = emp.Salary,
                TeamId = emp.TeamId
            };
            return result;

        }
        public async Task<bool> RegisterEmployeeAsync(RegisterEmployeeDto dto, int role = 2)
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
                EmployeeProfile = new Employee { Salary = dto.Salary, TeamId = dto.TeamId }
            };
            await _uow.UsersRepo.AddAsync(user);

            return await _uow.SaveChangesAsync() > 0;
        }
    }
}
