using Application.DTOs.NotficationDTOs;
using Application.DTOs.NotificationDTOs;
using Application.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
            public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("user")]

        public async Task<IActionResult> SubmaitNotificationToUser(SubmitNotificationDto dto) {
            var employee = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(employee) || !int.TryParse(employee, out int employeeId)) { return Forbid("you cannot do this."); }
            var result = await _notificationService.SendNotificationToUser(employeeId, dto);
            if (!result) return BadRequest("Your notification was not sent");
            return Ok("Your notification has been sent");
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost("Multiple_Cities")]
        public async Task<IActionResult>SubmaitToMultibleCities(SubmaitCityNotificationDto dto)

        {
            var employee = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(employee) || !int.TryParse(employee, out int employeeId)) { return Forbid("you cannot do this."); }
            var result = await _notificationService.SendNotificationToMultipleCities(employeeId, dto);
            if (!result) return BadRequest("Your notification was not sent");
            return Ok("Your notification has been sent");
        }

        [Authorize(Roles = "Admin,Employee,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MyNotificationDto>> GetNotificationById(int id) {
            var result = await _notificationService.GetNotificationById(id);
            if (result == null) return NotFound("the Notification does not exist");
            return Ok(result);
        }
        [Authorize(Roles ="User")]
        [HttpGet("MyNotifications")]
        public async Task<ActionResult<IEnumerable<MyNotificationDto>>> MyNotifications() {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(user== null || !int.TryParse(user,out int userId)) return BadRequest("You cannot do this");

            var cityIdClaim = User.FindFirstValue("cityId");
            if (string.IsNullOrEmpty(cityIdClaim) || !int.TryParse(cityIdClaim, out int cityId))
                return BadRequest("You cannot do this");

            var notification = await _notificationService.GetMyNotifications(userId, cityId);
            return Ok(notification);
        }

        [Authorize(Roles ="Admin,Employee")]
        [HttpGet("city")]
        public async Task<ActionResult<IEnumerable<CityNotificationDto>>> GetCityNotifications() {
            var city = User.FindFirstValue("cityId");
            if (string.IsNullOrEmpty(city) || !int.TryParse(city, out int cityId)) return BadRequest("cityId does not exist");
            var result = await _notificationService.GetCityNotifications(cityId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-Notifications")]
        public async Task<ActionResult<IEnumerable<AllNotificationsInSystemDto>>> GetAllNotifications(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            
            var result = await _notificationService.GetAllNotifications(page, size);

            
            return Ok(result);
        }
    }
}
