using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.AdvertisementModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bảo vệ API, yêu cầu đăng nhập
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        // Lấy tất cả quảng cáo của employer
        [HttpGet("employer/{employerId}")]
        public async Task<ActionResult<List<AdvertisementModel>>> GetAllByEmployer(int employerId)
        {
            var ads = await _advertisementService.GetAllByEmployerAsync(employerId);
            return Ok(ads);
        }

        // Lấy tất cả quảng cáo active và còn hạn của employer
        [HttpGet("employer/{employerId}/active")]
        public async Task<ActionResult<List<AdvertisementModel>>> GetAllActiveByEmployer(int employerId)
        {
            var ads = await _advertisementService.GetAllActiveByEmployerAsync(employerId);
            return Ok(ads);
        }

        // Lấy quảng cáo theo Id (kiểm tra quyền)
        [HttpGet("{id}/employer/{employerId}")]
        public async Task<ActionResult<AdvertisementModel>> GetById(int id, int employerId)
        {
            var ad = await _advertisementService.GetByIdAsync(id, employerId);
            return Ok(ad);
        }

        // Tạo quảng cáo mới
        [HttpPost]
        public async Task<ActionResult<AdvertisementModel>> Create([FromBody] CreateAdvertisementModel dto)
        {
            var createdAd = await _advertisementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdAd.Id, employerId = createdAd.EmployerId }, createdAd);
        }

        // Cập nhật quảng cáo
        [HttpPut("{id}/employer/{employerId}")]
        public async Task<ActionResult<AdvertisementModel>> Update(int id, int employerId, [FromBody] UpdateAdvertisementModel dto)
        {
            var updatedAd = await _advertisementService.UpdateAsync(id, employerId, dto);
            return Ok(updatedAd);
        }

        // Soft delete quảng cáo
        [HttpDelete("{id}/employer/{employerId}")]
        public async Task<ActionResult> SoftDelete(int id, int employerId)
        {
            var result = await _advertisementService.SoftDeleteAsync(id, employerId);
            if (!result) return NotFound();
            return NoContent();
        }

        // Khôi phục quảng cáo
        [HttpPatch("{id}/employer/{employerId}/restore")]
        public async Task<ActionResult> Restore(int id, int employerId)
        {
            var result = await _advertisementService.RestoreAsync(id, employerId);
            if (!result) return NotFound();
            return NoContent();
        }

        // Xóa cứng quảng cáo
        [HttpDelete("{id}/employer/{employerId}/hard")]
        public async Task<ActionResult> HardDelete(int id, int employerId)
        {
            var result = await _advertisementService.HardDeleteAsync(id, employerId);
            if (!result) return NotFound();
            return NoContent();
        }

        // Tăng lượt hiển thị quảng cáo
        [HttpPost("{id}/increment-impression")]
        [AllowAnonymous] // Có thể để public để frontend gọi tăng lượt xem
        public async Task<ActionResult> IncrementImpression(int id)
        {
            var result = await _advertisementService.IncrementImpressionAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
