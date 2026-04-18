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

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] RegisterEmployeeDto dto) {          
            var newEmployee = await _adminService.RegisterEmployeeAsync(dto);
            if (newEmployee)
                return Ok("The employee has been successfully added.");
            return BadRequest("An error occurred");

        }
    }
}
