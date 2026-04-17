using Application.Constants;
using Application.DTOs.UserDTOs;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> LogUp(RegisterUserDto dto, int roleId = AppRoles.User)
        {          
           var oldUser= await _uow.UsersRepo.GetUserByEmailAsync(dto.Email);
            if (oldUser != null) {
                throw new Exception("Used Email Address ");
            }
            var newUser = new User()
            {
                NationalId = dto.NationalId,
                FName = dto.FirstName,
                SName = dto.SecoundName,
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
