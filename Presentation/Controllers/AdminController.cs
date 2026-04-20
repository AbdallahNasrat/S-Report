using Application.Constants;
using Application.DTOs.EmployeeDTOs;
using Application.Services.AdminServices;
using Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterEmployeeDto dto) {
            var newAdmin = await _adminService.AddAdminAsync(dto);
            if (newAdmin) {
                return Ok("The admin has been Successfully Added");
            }
            return BadRequest("An error occurred");
        }
    }
}
