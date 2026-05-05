using Application.DTOs.LookupDtos;
using Application.DTOs.UserDTOs;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        public Task<bool> UpdateFcmTokenAsync(int userId, string token);
       // public Task<bool> UpdateUserInfoAsync(UpdateUserInfoDTO dto);
        //public Task<UserProfileResponseDto>GetUserProfileAsync(int userId);
        public Task<bool> BlokUserAsync(int userId);
        public Task<bool> UnBlokUserAsync(int userId);
        public Task<IEnumerable<UserProfileResponseDto>> GetAllUsersAsync(int page , int size,int? cityId=null);
        public Task<bool> ToggleNotificationsAsync(int userId, NotificationToggleDto dto);
        public Task<bool> ToggleVolunteerAsync(int userId, ToggleDto dto);

        public Task<UserProfileResponseDto> GetUserProfile(int userId);
        public Task<bool> UpdateUserProfile(int userId , UpdateUserInfoDTO dto);


    }
}
