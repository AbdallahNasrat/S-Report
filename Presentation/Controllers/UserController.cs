using Application.DTOs.UserDTOs;
using Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPatch("update-fcm-token")] 
        public async Task<IActionResult> UpdateFcmToken([FromBody] UpdateFcmTokenDTO dto)
        {
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User identity not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Invalid user ID format in token.");

            
            if (string.IsNullOrWhiteSpace(dto.Token))
                return BadRequest("FCM Token is required and cannot be empty.");

           
            var result = await _userService.UpdateFcmTokenAsync(userId, dto.Token);

            if (!result)
                return NotFound(new { message = "User not found or token update failed." });

            return Ok(new { message = "FCM Token updated successfully" });
        }
    }
}

