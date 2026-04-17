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
        public Task<bool> LogUp(RegisterUserDto dto , int roleId);

    }
}
