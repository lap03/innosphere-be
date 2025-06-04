using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.JobTagModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTagController : ControllerBase
    {
        private readonly IJobTagService _jobTagService;

        public JobTagController(IJobTagService jobTagService)
        {
            _jobTagService = jobTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobTagService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _jobTagService.GetAllActiveAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobTagService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobTagModel dto)
        {
            var result = await _jobTagService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobTagModel dto)
        {
            var result = await _jobTagService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _jobTagService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _jobTagService.RestoreAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _jobTagService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
