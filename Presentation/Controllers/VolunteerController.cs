using Application.DTOs.VolunteerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Services.NotificationServices;
using Application.DTOs;

namespace SReport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // عشان نضمن إن مفيش حد ينده الـ API من غير Token
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        // 1. جلب البلاغات القريبة (بياخد الـ GPS والمدينة في الـ Query)
        [Authorize(Roles ="User")]
        [HttpGet("nearby")]
        public async Task<ActionResult<IEnumerable<NearbyMissionResponseDto>>> GetNearbyMissions([FromQuery] int cityId, [FromQuery] decimal lat, [FromQuery] decimal lng)
        {
            var reports = await _volunteerService.GetNearbyMissions(cityId, lat, lng);
            return Ok(reports);
        }

        // 2. قبول البلاغ (بياخد رقم البلاغ من الـ URL، ورقم المتطوع من الـ Token)
        [HttpPost("accept/{reportId}")]
        public async Task<IActionResult> AcceptMission(int reportId)
        {
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int volunteerId))
                return Unauthorized(new { message = "Invalid Token" });

            var result = await _volunteerService.AcceptMission(reportId, volunteerId);

            return Ok(new { message = "The task has been successfully accepted, it is being worked on!" });
        }

        // 3. إنهاء البلاغ بنجاح
        [HttpPost("complete/{reportId}")]
        public async Task<IActionResult> CompleteMission(int reportId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int volunteerId))
                return Unauthorized(new { message = "Invalid Token" });

            var result = await _volunteerService.CompleteMission(reportId, volunteerId);
            if (!result)
                return BadRequest(new { message = "Failed to complete the task, make sure it was accepted beforehand." });

            return Ok(new { message = "The task has been completed and I successfully received the points!" });
        }

        // 4. جلب لوحة الشرف (أفضل المتطوعين)
        [HttpGet("leaderboard")]
        public async Task<ActionResult<IEnumerable< LeaderboardDto>>> GetLeaderboard()
        {
            var topVolunteers = await _volunteerService.GetTopVolunteers();
            return Ok(topVolunteers);
        }

        // 5. جلب بروفايل المتطوع نفسه
        [HttpGet("profile")]
        public async Task<ActionResult<VolunteerProfileDto>> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int volunteerId))
                return Unauthorized(new { message = "Invalid Token" });

            var profile = await _volunteerService.GetVolunteerProfile(volunteerId);
            return Ok(profile);
        }
    }
}