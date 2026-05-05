using Application.Constants;
using Application.DTOs.UserDTOs;
using Application.SecurityService;
using Domain.Entites;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(IUnitOfWork uow , IJwtProvider jwtProvider)
        {
            _uow = uow;
            _jwtProvider = jwtProvider;
        }
        public async Task<LoginResponseDTO> LoginAsync(string email, string password)
        {
            var user =  await _uow.UsersRepo.GetUserByEmailAsync(email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Incorrect email address or password");
            var token = _jwtProvider.GenerateToken(user,user.EmployeeProfile?.Id);
            var response = new LoginResponseDTO()
            {
                Token = token,
                UserId = user.Id,
                CityId = user.CityId,
                FirstName = user.FName,
                Role = user.Role?.Name
            };
            if (response.Role == "Employee") {
                var employee = await _uow.EmployeesRepo.GetFirstOrDefaultAsync(e => e.UserId == user.Id);
                if(employee != null)
                    response.EmployeeId = employee.Id;
            }
                return response;           
        }
        public async Task<bool> LogUp(RegisterUserDto dto, int roleId = AppRoles.User)
        {
            var oldUser = await _uow.UsersRepo.GetUserByEmailAsync(dto.Email);
            if (oldUser != null)
            {
                throw new Exception("Used Email Address ");
            }
            var newUser = new User()
            {
                NationalId = dto.NationalId,
                FName = dto.FirstName,
                SName = dto.SecondName,
                Address = dto.HomeAddress,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Gender = dto.Gender,
                Birthdate = dto.Birthdate,
                Volunteer = true, // default
                CityId = dto.CityId,
                RoleId = roleId
            };
            _uow.UsersRepo.Add(newUser);
            return await _uow.SaveChangesAsync() > 0;

        }


    }
}
