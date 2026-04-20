using Application.DTOs.TeamsDTOs;
using Application.Services.TeamService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Authorize(Roles ="Admin,Employee")]
        // GET: api/teams
        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetTeamsAsync();
            return Ok(teams); // ok(list) or EmptyList
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound(new { message = $"team wit id :  {id} does not exist" }); // 404

            return Ok(team);
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: api/teams/name/Tigers
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetTeamByName(string name)
        {
            var team = await _teamService.GetTeamByNameAsync(name);
            if (team == null)
                return NotFound(new { message = $"There is no team named {name}" });

            return Ok(team);
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: api/teams/city/3
        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetTeamsByCityId(int cityId)
        {
            var teams = await _teamService.GetTeamsByCityIdAsync(cityId);
            return Ok(teams);
        }

        [Authorize(Roles = "Admin")]
        // POST: api/teams
        [HttpPost]
        public async Task<IActionResult> AddTeam([FromBody] CreateTeamDTO dto)
        {
            
            var result = await _teamService.AddTeamAsync(dto);

            if (result)
                return Ok(new { message = "The team was added successfully." });

            return BadRequest(new { message = "An error occurred while adding the team" });
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDTO dto)
        {
            
            if (id != dto.Id)
                return BadRequest(new { message = "There is a difference in the team numbers" });

            var result = await _teamService.UpdateTeamAsync(dto);
            if (!result)
                return NotFound(new { message = $"team with id : {id} Not available for editing" });

            return Ok(new { message = "The team data was successfully modified" });
        }
        [Authorize(Roles = "Admin,Employee")]
        // PATCH: api/teams/5/state
        [HttpPatch("{id}/state")]
        public async Task<IActionResult> UpdateTeamState(int id, [FromBody] UpdateTeamStateDTO dto)
        {
            if (id != dto.TeamId)
                return BadRequest(new { message = "There is a difference in the team numbers" });

            var result = await _teamService.UpdateTeamStateAsync(dto);
            if (!result)
                return NotFound(new { message = "The team is not available." });

            return Ok(new { message = "The team status has been successfully changed." });
        }
        [Authorize(Roles = "Admin")]
        // PATCH: api/teams/5/block
        [HttpPatch("{id}/block")]
        public async Task<IActionResult> BlockTeam(int id)
        {
            var result = await _teamService.BlockTeamAsync(id);
            if (result)
                return Ok(new { message = "Team was successfully blocked" });

            return BadRequest(new { message = "The ban on the team failed" });
        }
        // PATCH: api/teams/assign-employee
        [HttpPatch("assign-employee")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> AssignEmployee([FromBody] AssignEmployeeToTeamDTO dto)
        {
            var result = await _teamService.AssignEmployeeToTeamAsync(dto);

            if (result)
                return Ok(new { message = "The employee was successfully added to the team" });

            return BadRequest(new { message = "Failure in the connection process" });
        }
    }
}