using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.AdvertisementPackageModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementPackageController : ControllerBase
    {
        private readonly IAdvertisementPackageService _advertisementPackageService;

        public AdvertisementPackageController(IAdvertisementPackageService advertisementPackageService)
        {
            _advertisementPackageService = advertisementPackageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _advertisementPackageService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _advertisementPackageService.GetAllActiveAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _advertisementPackageService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdvertisementPackageModel dto)
        {
            var result = await _advertisementPackageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAdvertisementPackageModel dto)
        {
            var result = await _advertisementPackageService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _advertisementPackageService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _advertisementPackageService.RestoreAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _advertisementPackageService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
