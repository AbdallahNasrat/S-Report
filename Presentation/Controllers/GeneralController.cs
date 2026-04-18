using Application.DTOs.LookupDtos;
using Application.Services.GeneralServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly ILookupsService _lookupsService;
        public GeneralController(ILookupsService lookupsService)
        {
            _lookupsService = lookupsService;
        }
        [AllowAnonymous]
        [HttpGet("Cities")]       
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetAllCities() {
            var cities =  await _lookupsService.GetCitiesAsync();
            return Ok(cities);
        }   

    }
}
