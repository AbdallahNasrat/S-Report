using Application.DTOs.ReportDTOs;
using Application.Services.ReportServices;
using Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
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

        [Authorize(Roles ="Admin,Employee")]
        [HttpGet("CityReports")]
        public async Task<ActionResult<IEnumerable<ReportDetailsDto>>> CityReports([FromQuery]int page , [FromQuery] int size , [FromQuery] bool excludeResolved = true) {
            var cityIdClaim = User.FindFirstValue("cityId");

            if (string.IsNullOrEmpty(cityIdClaim) || !int.TryParse(cityIdClaim, out int cityId))
            {
                
                return Forbid("This account is not linked to a specific city.");
            }
            if (page <= 0) page = 1;
            if (size <= 0 || size > 50) size = 10;

            var reports = await _reportService.GetFilteredReportsAsync(cityId, page, size, excludeResolved);
            return Ok(reports);
        }
        [Authorize(Roles ="User")]
        [HttpGet("MyReports")]
        public async Task<ActionResult<IEnumerable<ReportSummaryDto>>> MyReports([FromQuery] int pageNumber,int pageSize) {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId)) {
                return Forbid("This account is not linked to a app.");
            }
            if (pageSize <= 0) pageNumber = 1;
            if (pageSize <= 0 || pageSize > 50) pageSize = 10;
            var reports = await _reportService.GetMyReportsAsync(userId, pageNumber, pageSize);
            return Ok(reports);
        }
        [Authorize(Roles ="Admin,Employee")]
        [HttpGet("ReportDetails{id}")]
        public async Task<ActionResult<ReportDetailsDto>> ReportDetails(int id) {

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var cityIdClaim = User.FindFirstValue("cityId");

            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
                return NotFound("The Report does not exist");
            if (userRole == "Employee")
            {

                if (int.TryParse(cityIdClaim, out int cityId) && report.CityId != cityId) {
                    return Forbid("You are not allowed to access reports outside your city");
                }
            }
            return Ok(report);
        }
        [HttpGet("ReportSummary{id}")]
        public async Task<ActionResult<ReportDetailsDto>> ReportSummary(int id) {

            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _reportService.GetReportSummaryByIdAsync(id, false);
            if(report == null) {
                return NotFound("The Report Does Not Exist");
            }
            if (!int.TryParse(userClaim, out int userId) && report.UserId != userId) {
                return Forbid("You are not allowed to access reports not for you");
            }
            return Ok(report);
        }



      
    }
}
