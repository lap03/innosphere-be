using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.JobApplicationModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        // Worker nộp đơn ứng tuyển
        [HttpPost("apply")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> Apply([FromBody] CreateJobApplicationModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            // ❌ LỖI: Trước đây ép kiểu int.Parse(userId) => lỗi nếu userId là GUID
            // ✅ SỬA: Giữ userId dạng string, cập nhật service để nhận string
            var result = await _jobApplicationService.ApplyAsync(model, userId); // SỬA
            return Ok(result);
        }

        // Employer xem danh sách đơn ứng tuyển (tùy chọn lọc theo jobPostingId)
        [HttpGet("employer")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetByEmployer([FromQuery] int? jobPostingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            // ❌ LỖI: int.Parse(userId) lỗi nếu userId là GUID
            // ✅ SỬA: Truyền userId dạng string
            var results = await _jobApplicationService.GetByEmployerAsync(userId, jobPostingId); // SỬA
            return Ok(results);
        }

        // Worker xem danh sách đơn ứng tuyển của mình
        [HttpGet("worker")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> GetByWorker()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            // ✅ SỬA: Truyền userId dạng string
            var results = await _jobApplicationService.GetByWorkerAsync(userId); // SỬA
            return Ok(results);
        }

        // Lấy chi tiết đơn ứng tuyển
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobApplicationService.GetByIdAsync(id);
            return Ok(result);
        }

        // Employer/Admin cập nhật trạng thái đơn
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateJobApplicationStatusModel model)
        {
            var success = await _jobApplicationService.UpdateStatusAsync(id, model);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // Worker hủy đơn ứng tuyển
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> CancelApplication(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            // ✅ SỬA: Truyền userId dạng string
            var success = await _jobApplicationService.CancelApplicationAsync(id, userId); // SỬA
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
