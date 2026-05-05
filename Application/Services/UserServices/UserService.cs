using Application.Constants;
using Application.DTOs.LookupDtos;
using Application.DTOs.UserDTOs;
using Domain.Entites;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> BlokUserAsync(int userId)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if (user == null)  throw new NotFoundException("user not found");
            user.Rate = 1; 
            return await _uow.SaveChangesAsync()>0;
        }
        public async Task<bool> UnBlokUserAsync(int userId)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if (user == null)  throw new NotFoundException("user not found");
            user.Rate = 5; 
            return await _uow.SaveChangesAsync()>0;
        }



        public async Task<IEnumerable<UserProfileResponseDto>> GetAllUsersAsync(int page , int size, int? cityId = null)
        {
            var users = await _uow.UsersRepo.GetUsers(page, size, cityId);
            if (users == null) return null;
            var result = users.Select(u => new UserProfileResponseDto()
            {
                FirstName = u.FName,
                SecundName = u.SName,
                Email = u.Email,
                NationalId = u.NationalId,
                Address = u.Address,
                Phone = u.Phone,
                Gender = u.Gender,
                Volunteer = u.Volunteer,
                Notification = u.IsNotificationEnabled,
                Birthdate = u.Birthdate,
                Rate = u.Rate

            });
            return result;
        }



        public async Task<bool> ToggleNotificationsAsync(int userId, NotificationToggleDto dto)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if (user == null) { throw new NotFoundException("User not found"); }
            if (dto == null || dto.IsEnabled == null) return false;
            user.IsNotificationEnabled = dto.IsEnabled;
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleVolunteerAsync(int userId, ToggleDto dto)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if (user == null || dto == null) throw new NotFoundException("user not found");
            user.Volunteer = dto.IsEnabled;
            return await _uow.SaveChangesAsync()>0;

        }

        public async Task<bool> UpdateFcmTokenAsync(int userId, string token)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if(user == null)
                return false;
            user.FcmToken = token;
            return await _uow.SaveChangesAsync() > 0 ;
        }

        public async Task<UserProfileResponseDto> GetUserProfile(int userId)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId, false);
            if (user == null) throw new NotFoundException("user does not exist");

            var result = new UserProfileResponseDto()
            {
                FirstName = user.FName,
                SecundName = user.SName,
                Address =   user.Address,
                Birthdate = user.Birthdate,
                Email =     user.Email,
                Gender =    user.Gender,
                NationalId = user.NationalId,
                Phone =     user.Phone,
                Rate =      user.Rate,
                Notification = user.IsNotificationEnabled,
                Volunteer =     user.Volunteer
            };
            return result;

        }

        public async Task<bool> UpdateUserProfile(int userId, UpdateUserInfoDTO dto)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if(user == null) throw new NotFoundException("user does not exist");
            if(dto == null) throw new ArgumentNullException("enter valid data");

            user.FName = dto.FirstName;
            user.SName = dto.SecondName;
            user.Address = dto.HomeAddress;
            user.Phone = dto.Phone;
            user.Email = dto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CityId = dto.CityId;
            return await _uow.SaveChangesAsync() > 0; 


        }    
    }       
}          
