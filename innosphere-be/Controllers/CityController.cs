using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.CityModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        // Lấy tất cả (cả bản ghi đã xóa mềm)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cityService.GetAllAsync();
            return Ok(result);
        }

        // Lấy bản ghi chưa xóa mềm
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _cityService.GetAllActiveAsync();
            return Ok(result);
        }

        // Lấy theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _cityService.GetByIdAsync(id);
            return Ok(result);
        }

        // Tạo mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityModel dto)
        {
            var result = await _cityService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // Cập nhật
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCityModel dto)
        {
            var result = await _cityService.UpdateAsync(id, dto);
            return Ok(result);
        }

        // Xóa mềm
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _cityService.DeleteAsync(id);
            return NoContent();
        }

        // Phục hồi bản ghi đã xóa mềm
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _cityService.RestoreAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        // Xóa cứng
        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _cityService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
