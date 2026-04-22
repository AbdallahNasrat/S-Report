using Application.DTOs.ReportDTOs;
using Application.Services.ReportServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpPost]
        public async Task<IActionResult> AddReportAsync([FromForm]CreateReportDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            int userId = int.Parse(userIdClaim.Value);

            var result = await _reportService.AddReportAsync(userId, dto);
            if(!result)
                return BadRequest("The report has not been added");
            return Ok("The report has  been added successfully");
        }
    }
}
