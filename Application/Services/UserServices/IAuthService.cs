using Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public interface IAuthService
    {
        public Task<LoginResponseDTO> LoginAsync(string email , string password);
        public Task<bool> LogUp(RegisterUserDto dto, int roleId);



    }
}
