using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.UserModels;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // PUT: api/user
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
        {
            var result = await _userService.UpdateAsync(model);
            if (!result) return NotFound();
            return NoContent();
        }

        // PUT: api/user/ban/{id}
        [HttpPut("ban/{id}")]
        public async Task<IActionResult> Ban(string id)
        {
            var result = await _userService.BanAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // PUT: api/user/unban/{id}
        [HttpPut("unban/{id}")]
        public async Task<IActionResult> Unban(string id)
        {
            var result = await _userService.UnbanAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}