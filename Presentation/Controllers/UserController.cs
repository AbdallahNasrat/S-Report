using Application.DTOs.LookupDtos;
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

        [Authorize(Roles ="User")]
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

        [Authorize(Roles = "User")]
        [HttpPatch("toggle-notifications")]
        public async Task<IActionResult> ToggleNotifications(NotificationToggleDto dto) {
            var userCard = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userCard == null || !int.TryParse(userCard.Value, out int userId)) {
                return Unauthorized("You cannot do this");
            }           
            var result =await  _userService.ToggleNotificationsAsync(userId, dto);
            if(!result)
                return BadRequest("Your request has not been processed");
            return Ok("The notification status has changed.");
        }
        [Authorize(Roles ="User")]
        [HttpPatch("toggle-Volunteer")]
        public async Task<IActionResult> ToggleVolunter([FromBody] ToggleDto dto) {
            var userCard = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userCard == null || !int.TryParse(userCard.Value,out int userId)) return Unauthorized("Unauthorized action");
            var result = await _userService.ToggleVolunteerAsync(userId, dto);
            if (!result)
                return BadRequest("The request was not fulfilled.");
            return Ok("The request was fulfilled");

        }
        [Authorize(Roles ="Admin,Employee")]
        [HttpPatch("{userId}/block")]
        public async Task<IActionResult> BlockUser(int userId) {
            var result = await _userService.BlokUserAsync(userId);
            if (!result) return BadRequest("The request was not fulfilled.");
            return Ok("The user has been blocked.");
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPatch("{userId}/Unblock")]
        public async Task<IActionResult> UnBlockUser(int userId) {
            var result = await _userService.UnBlokUserAsync(userId);
            if (!result) return BadRequest("The request was not fulfilled.");
            return Ok("The user has been blocked.");
        }

        [Authorize(Roles ="Admin,Employee")]
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserProfileResponseDto>>> All([FromQuery] int page, [FromQuery] int size) {
            var users = await _userService.GetAllUsersAsync(page, size);
            if (users == null) return NotFound("No Users");
            return Ok(users);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("InCity")]
        public async Task<ActionResult<IEnumerable<UserProfileResponseDto>>> CityUsers([FromQuery] int page, [FromQuery] int size) {
            var cityIdClaim = User.FindFirstValue("cityId");
            if (string.IsNullOrEmpty(cityIdClaim) || !int.TryParse(cityIdClaim, out int cityId))
                return BadRequest("You cannot do this");

            var users = await _userService.GetAllUsersAsync(page, size,cityId);           
            return Ok(users);
        }

        


    }
}

