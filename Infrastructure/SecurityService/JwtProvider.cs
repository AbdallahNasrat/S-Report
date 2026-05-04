using Application.SecurityService;
using Domain.Entites;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.SecurityService
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtProvider(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions= jwtOptions.Value;
        }
        public string GenerateToken(User user ,int? employeeId = null){
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("cityId",user.CityId.ToString())
            };
            if (employeeId.HasValue)
            {
                claims.Add(new Claim("EmployeeId", employeeId.Value.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.DurationInDays),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
