using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.SubscriptionModels;
using System;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // Lấy tất cả subscription của employer (đầu vào employerId)
        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetAllByEmployer(int employerId)
        {
            var list = await _subscriptionService.GetAllByEmployerAsync(employerId);
            return Ok(list);
        }

        // Lấy subscription theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _subscriptionService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Mua subscription mới cho employer
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] CreateSubscriptionModel dto)
        {
            try
            {
                var result = await _subscriptionService.PurchaseSubscriptionAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Kiểm tra employer còn quyền đăng tin hay không
        [HttpGet("employer/{employerId}/canpost")]
        public async Task<IActionResult> CanPost(int employerId)
        {
            var canPost = await _subscriptionService.CanPostJobAsync(employerId);
            return Ok(canPost);
        }

        // Hủy subscription (soft delete)
        [HttpPut("{id}/cancel/{employerId}")]
        public async Task<IActionResult> Cancel(int id, int employerId)
        {
            try
            {
                var result = await _subscriptionService.CancelSubscriptionAsync(id, employerId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Xóa cứng subscription (chỉ khi không có tin đăng liên quan)
        [HttpDelete("{id}/employer/{employerId}")]
        public async Task<IActionResult> HardDelete(int id, int employerId)
        {
            try
            {
                var result = await _subscriptionService.HardDeleteAsync(id, employerId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Admin: Lấy tất cả subscription từ tất cả employers
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var subscriptions = await _subscriptionService.GetAllForAdminAsync();
            return Ok(subscriptions);
        }
    }
}
