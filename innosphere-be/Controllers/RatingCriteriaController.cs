using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.RatingCriteriaModels;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingCriteriaController : ControllerBase
    {
        private readonly IRatingCriteriaService _service;

        public RatingCriteriaController(IRatingCriteriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("worker")]
        public async Task<IActionResult> GetWorkerCriteria()
        {
            var result = await _service.GetByTypeAsync("Worker");
            return Ok(result);
        }

        [HttpGet("employer")]
        public async Task<IActionResult> GetEmployerCriteria()
        {
            var result = await _service.GetByTypeAsync("Employer");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRatingCriteriaModel model)
        {
            var result = await _service.AddAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateRatingCriteriaModel model)
        {
            var result = await _service.UpdateAsync(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}