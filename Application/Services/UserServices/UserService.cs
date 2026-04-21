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

        public async Task<bool> UpdateFcmTokenAsync(int userId, string token)
        {
            var user = await _uow.UsersRepo.GetByIdAsync(userId);
            if(user == null)
                return false;
            user.FcmToken = token;
            return await _uow.SaveChangesAsync() > 0 ;
        }
    }
}
