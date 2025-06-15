using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.SocialLinkModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SocialLinkController : ControllerBase
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinkController(ISocialLinkService socialLinkService)
        {
            _socialLinkService = socialLinkService;
        }

        // ✅ GET /api/sociallinks/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMySocialLinks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not authenticated");

            var list = await _socialLinkService.GetByUserIdAsync(userId);
            return Ok(list);
        }

        // ✅ GET /api/sociallinks/active
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var list = await _socialLinkService.GetAllActiveAsync();
            return Ok(list);
        }

        // ✅ GET /api/sociallinks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _socialLinkService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ✅ POST /api/sociallinks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSocialLinkModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not authenticated");

            try
            {
                var created = await _socialLinkService.CreateAsync(model, userId);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ PUT /api/sociallinks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSocialLinkModel model)
        {
            try
            {
                var updated = await _socialLinkService.UpdateAsync(id, model);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ DELETE /api/sociallinks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _socialLinkService.DeleteAsync(id);
                return Ok(new { message = "Soft delete successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ PUT /api/sociallinks/restore/{id}
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                await _socialLinkService.RestoreAsync(id);
                return Ok(new { message = "Restore successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ DELETE /api/sociallinks/hard/{id}
        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                await _socialLinkService.HardDeleteAsync(id);
                return Ok(new { message = "Hard delete successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
