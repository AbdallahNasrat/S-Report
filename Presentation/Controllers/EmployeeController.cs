using Application.Constants;
using Application.DTOs.EmployeeDTOs;
using Application.Services.EmployeeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Roles ="Admin,Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployess() {
            var employees = await _employeeService.GetAllEmployeesAsync();
            if (employees == null || !employees.Any()) return NotFound("There are no employees. ");
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id) {
            var emp = await _employeeService.GetEmployeeAsync(id);
            if (emp == null) return NotFound("There are no employee");
            return Ok(emp);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployeeData([FromRoute] int id, [FromBody] EmployeeDTO dto)
        {
            if (id != dto.EmployeeId) return BadRequest("There is a difference in Employee ID");
            var result = await _employeeService.EditEmployeeDataAsync(dto);
            if (!result)
                return BadRequest("Employee data has not been modified");
            return Ok("Employee data has been updated");         
        }
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] RegisterEmployeeDto dto)
        {
            var newEmployee = await _employeeService.RegisterEmployeeAsync(dto, AppRoles.Employee);
            if (newEmployee)
                return Ok("The employee has been Successfully Added.");
            return BadRequest("An error occurred");

        }
    }
}
