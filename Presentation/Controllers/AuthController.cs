using Application.Constants;
using Application.DTOs.UserDTOs;
using Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("LogUp")]
        public async Task<IActionResult> LogUp([FromBody] RegisterUserDto dto) {
            var result = _authService.LogUp(dto,AppRoles.User);
            if(result.Result == true) {
                return Ok("User registered successfully!");

            }
            return BadRequest("Something went wrong while saving the user.");

        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody]LoginDTO dto) {
            var result = await _authService.LoginAsync(dto.Email, dto.Password);
            return Ok(result);

        }
   
    }
}
