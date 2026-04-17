using Application.SecurityService;
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
        public async Task<string> LoginAsync(string email, string password)
        {
            var user =  await _uow.UsersRepo.GetUserByEmailAsync(email);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (user == null || !isPasswordValid) throw new UnauthorizedAccessException("Incorrect email address or password");
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

    }
}
