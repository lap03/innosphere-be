using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.SubscriptionPackageModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPackageController : ControllerBase
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;

        public SubscriptionPackageController(ISubscriptionPackageService subscriptionPackageService)
        {
            _subscriptionPackageService = subscriptionPackageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _subscriptionPackageService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _subscriptionPackageService.GetAllActiveAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _subscriptionPackageService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionPackageModel dto)
        {
            var result = await _subscriptionPackageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubscriptionPackageModel dto)
        {
            var result = await _subscriptionPackageService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _subscriptionPackageService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _subscriptionPackageService.RestoreAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _subscriptionPackageService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
