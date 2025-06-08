using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.WorkerRatingModels;
using Service.Services;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerRatingController : ControllerBase
    {
        private readonly WorkerRatingService _service;

        public WorkerRatingController(WorkerRatingService service)
        {
            _service = service;
        }

        [HttpGet("worker/{workerId}")]
        public async Task<IActionResult> GetAllRatingsByWorker(int workerId)
        {
            var result = await _service.GetAllRatingsByWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpGet("{workerRatingId}/details")]
        public async Task<IActionResult> GetRatingDetails(int workerRatingId)
        {
            var result = await _service.GetRatingDetailsAsync(workerRatingId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkerRatingModel model)
        {
            await _service.CreateWorkerRatingAsync(model);
            return NoContent();
        }
    }
}
