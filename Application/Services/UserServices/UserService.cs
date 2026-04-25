using Application.Constants;
using Application.DTOs.LookupDtos;
using Application.DTOs.UserDTOs;
using Domain.Entites;
using Domain.Exceptions;
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

        //public Task<IEnumerable<UserProfileResponseDto>> GetAllCityUsersAsync(int cityId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<UserProfileResponseDto>> GetAllUsersAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<UserProfileResponseDto> GetUserProfileAsync(int userId)
        //{
        //    throw new NotImplementedException();
        //}

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

        //public Task<bool> UpdateUserInfoAsync(UpdateUserInfoDTO dto)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
