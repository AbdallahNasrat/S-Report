using Application.DTOs.NotficationDTOs;
using Application.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize(Roles ="Admin,Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
            public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmaitNotificationToUser(SubmitNotificationDto dto) {
            var employee = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(employee) || !int.TryParse(employee, out int employeeId)) { return Forbid("you cannot do this."); }
            var result = await _notificationService.SendNotificationToUser(employeeId, dto);
            if (!result) return BadRequest("Your notification was not sent");
            return Ok("Your notification has been sent");
        }

    }
}
