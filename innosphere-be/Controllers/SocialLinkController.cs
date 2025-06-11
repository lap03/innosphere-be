using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.SocialLinkModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialLinkController : ControllerBase
    {
            private readonly ISocialLinkService _socialLinkService;

            public SocialLinkController(ISocialLinkService socialLinkService)
            {
                _socialLinkService = socialLinkService;
            }

            // GET: api/sociallinks
            [HttpGet]
            public async Task<ActionResult<List<SocialLinkModel>>> GetAll()
            {
                var list = await _socialLinkService.GetAllAsync();
                return Ok(list);
            }

            // GET: api/sociallinks/active
            [HttpGet("active")]
            public async Task<ActionResult<List<SocialLinkModel>>> GetAllActive()
            {
                var list = await _socialLinkService.GetAllActiveAsync();
                return Ok(list);
            }

            // GET: api/sociallinks/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<SocialLinkModel>> GetById(int id)
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

            // GET: api/sociallinks/user/{userId}
            [HttpGet("user/{userId}")]
            public async Task<ActionResult<List<SocialLinkModel>>> GetByUserId(string userId)
            {
                try
                {
                    var list = await _socialLinkService.GetByUserIdAsync(userId);
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            // POST: api/sociallinks
            [HttpPost]
            public async Task<ActionResult<SocialLinkModel>> Create([FromBody] CreateSocialLinkModel model)
            {
                try
                {
                    var created = await _socialLinkService.CreateAsync(model);
                    return Ok(created);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            // PUT: api/sociallinks/{id}
            [HttpPut("{id}")]
            public async Task<ActionResult<SocialLinkModel>> Update(int id, [FromBody] UpdateSocialLinkModel model)
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

            // DELETE: api/sociallinks/{id} → SoftDelete
            [HttpDelete("{id}")]
            public async Task<ActionResult> Delete(int id)
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

            // PUT: api/sociallinks/restore/{id}
            [HttpPut("restore/{id}")]
            public async Task<ActionResult> Restore(int id)
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

            // DELETE: api/sociallinks/hard/{id}
            [HttpDelete("hard/{id}")]
            public async Task<ActionResult> HardDelete(int id)
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

