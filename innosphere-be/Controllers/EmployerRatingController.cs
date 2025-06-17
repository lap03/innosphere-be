using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.EmployerRatingModels;
using Service.Services;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerRatingController : ControllerBase
    {
        private readonly IEmployerRatingService _service;

        public EmployerRatingController(IEmployerRatingService service)
        {
            _service = service;
        }

        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetAllRatingsByEmployer(int employerId)
        {
            var result = await _service.GetAllRatingsByEmployerAsync(employerId);
            return Ok(result);
        }

        [HttpGet("{employerRatingId}/details")]
        public async Task<IActionResult> GetRatingDetails(int employerRatingId)
        {
            var result = await _service.GetRatingDetailsAsync(employerRatingId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployerRatingModel model)
        {
            await _service.CreateEmployerRatingAsync(model);
            return NoContent();
        }
    }
}
