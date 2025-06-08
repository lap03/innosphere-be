using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.ResumeModels;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _resumeService.GetResumeAsync(id);
            return Ok(result);
        }

        [HttpGet("worker/{workerId}")]
        public async Task<IActionResult> GetByWorker(int workerId)
        {
            var result = await _resumeService.GetResumesByWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateResumeModel model)
        {
            var result = await _resumeService.AddResumeAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateResumeModel model)
        {
            var result = await _resumeService.UpdateResumeAsync(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resumeService.DeleteResumeAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _resumeService.RestoreResumeAsync(id);
            return NoContent();
        }
    }
}
