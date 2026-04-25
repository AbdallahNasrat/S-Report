using Application.DTOs.LookupDtos;
using Application.DTOs.ReportDTOs;
using Application.DTOs.TeamsDTOs;
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
        public async Task<IActionResult> AddReportAsync([FromForm] CreateReportDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            int userId = int.Parse(userIdClaim.Value);

            var result = await _reportService.AddReportAsync(userId, dto);
            if (!result)
                return BadRequest("The report has not been added");
            return Ok("The report has  been added successfully");
        }

        [Authorize(Roles="Admin")]
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<ReportDetailsDto>>> AllReports([FromQuery] int page, [FromQuery] int size, [FromQuery] bool excludeResolved = true) {
            var reports = await _reportService.GetFilteredReportsAsync(pageNumber: page, pageSize: size, excludeResolved: excludeResolved);
            if (reports == null || !reports.Any()) { return NotFound("No Reports"); }
            return Ok(reports);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("CityReports")]
        public async Task<ActionResult<IEnumerable<ReportDetailsDto>>> CityReports([FromQuery] int page, [FromQuery] int size, [FromQuery] bool excludeResolved = true) {
            var cityIdClaim = User.FindFirstValue("cityId");

            if (string.IsNullOrEmpty(cityIdClaim) || !int.TryParse(cityIdClaim, out int cityId))
            {

                return Forbid("This account is not linked to a specific city.");
            }
            if (page <= 0) page = 1;
            if (size <= 0 || size > 50) size = 10;

            var reports = await _reportService.GetFilteredReportsAsync( page, size, excludeResolved, cityId);
            return Ok(reports);
        }
        [Authorize(Roles = "User")]
        [HttpGet("MyReports")]
        public async Task<ActionResult<IEnumerable<ReportSummaryDto>>> MyReports([FromQuery] int pageNumber, int pageSize) {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId)) {
                return Forbid("This account is not linked to a app.");
            }
            if (pageSize <= 0) pageNumber = 1;
            if (pageSize <= 0 || pageSize > 50) pageSize = 10;
            var reports = await _reportService.GetMyReportsAsync(userId, pageNumber, pageSize);
            return Ok(reports);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("ReportDetails/{id}")]
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
        [HttpGet("ReportSummary/{id}")]
        public async Task<ActionResult<ReportDetailsDto>> ReportSummary(int id) {

            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _reportService.GetReportSummaryByIdAsync(id, false);
            if (report == null) {
                return NotFound("The Report Does Not Exist");
            }
            if (!int.TryParse(userClaim, out int userId) && report.UserId != userId) {
                return Forbid("You are not allowed to access reports not for you");
            }
            return Ok(report);
        }

        [Authorize(Roles = "Employee")]
        [HttpPatch("{reportId}/status")]
        public async Task<IActionResult> UpdateReportState([FromRoute] int reportId, [FromBody] UpdateReportStatusDto dto) {
            if (dto == null || reportId == 0) {
                return NotFound("The report is not found or the report status has not changed.");
            }
            var result = await _reportService.UpdateReportStatusAsync(reportId, dto.Status);
            if (!result) {
                return NotFound("the report status has not changed");
            }
            return Ok("the report status has been changed");

        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReport([FromRoute] int id) {
            if (id <= 0)
                return BadRequest("Enter Valid Number");
            var result = await _reportService.CancelReportAsync(id);
            if (!result) { return BadRequest("The report has not been cancelled."); }
            return Ok("The report has been cancelled.");
        }

        [HttpPost("SynOfflineReports")]
        public async Task<ActionResult<SyncResultDto>> SyncOfflineReports([FromBody] IEnumerable<CreateReportDTO> dto) {
            if (dto == null || !dto.Any()) { return BadRequest("the Reports list is empty"); }
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            if (user == null || !int.TryParse(user.Value, out int userId)) { return Unauthorized("User identification failed"); }
            var result = await _reportService.SyncOfflineReports(userId, dto);
            return Ok(result);
        }


        [Authorize(Roles ="Employee")]
        [HttpPatch("{reportId}/Team")]
        public async Task<IActionResult> AssignTeam(int reportId, [FromBody] AssignTeamDto dto){
            if (dto == null )
                return BadRequest("Enter Valid TeamId");
            var result = await _reportService.AssignTeamToReportAsync(reportId, dto.TeamId);
            if (!result) { return BadRequest("The team was not assigned to the report."); }
            return Ok("The team received the report");
        }
        [Authorize(Roles ="Employee,Admin")]
        [HttpPatch("{reportId}/Type")]
        public async Task<IActionResult> CorrectReportType(int reportId, [FromBody] ReportTypeDto dto) {
            var result = await _reportService.CorrectReportTypeAsync(reportId, dto.categoryId);
            if (!result) {
                return BadRequest("The report type has not changed.");
            }       
            return Ok("The report type has been changed.");
        }
        [Authorize(Roles =("Admin,Employee"))]
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> Categories() {
            var categories = await _reportService.GetCategories();
            if (categories == null || !categories.Any()) {
                return NotFound("No Categories"); }
            return Ok(categories);
            }
        }







    }


