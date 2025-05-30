using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleService.AddRoleAsync(roleName);
            if (result)
                return Ok("Role created successfully.");
            return BadRequest("Role already exists.");
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRoleIfUnusedAsync(roleName);
            if (result)
                return Ok("Role deleted successfully.");
            return BadRequest("Role not found or is in use.");
        }
    }
}
