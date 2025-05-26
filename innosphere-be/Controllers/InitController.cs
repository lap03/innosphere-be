using i.Models.Requests.InitRequest;
using innosphere_be.Models.Requests.InitRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IInitService _initService;

        public InitController(IInitService initService)
        {
            _initService = initService;
        }

        // POST: api/seed/users
        [HttpPost("users")]
        public async Task<IActionResult> SeedUsers()
        {
            await _initService.SeedUsersAsync();
            return Ok("Seeded hardcoded users successfully.");
        }

        // POST: api/seed/user
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserInitReuest model)
        {
            var result = await _initService.CreateUserWithRoleAsync(model.Email, model.Password, model.RoleName);
            if (result.Succeeded)
                return Ok("User created successfully.");
            return BadRequest(result.Errors);
        }

        // PUT: api/seed/user/assign-role
        [HttpPut("user/assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest model)
        {
            var result = await _initService.AssignRoleAsync(model.Email, model.RoleName);
            if (result.Succeeded)
                return Ok("Role assigned successfully.");
            return BadRequest(result.Errors);
        }
    }
}
