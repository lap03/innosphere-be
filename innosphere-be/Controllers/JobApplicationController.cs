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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            try
            {
                var result = await _jobApplicationService.ApplyAsync(model, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Internal Exception: {ex.Message}\n{ex.InnerException?.Message}\n{ex.StackTrace}");
            }

        }

        // Employer xem danh sách đơn ứng tuyển
        [HttpGet("employer")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetByEmployer([FromQuery] int? jobPostingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            try
            {
                var results = await _jobApplicationService.GetByEmployerAsync(userId, jobPostingId);
                return Ok(new { UserName = userName, Applications = results });
            }
            catch
            {
                return StatusCode(500, "Failed to load job applications for employer.");
            }
        }

        // Worker xem đơn ứng tuyển của mình
        [HttpGet("worker")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> GetByWorker()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            try
            {
                var results = await _jobApplicationService.GetByWorkerAsync(userId);
                return Ok(new { UserName = userName, Applications = results });
            }
            catch
            {
                return StatusCode(500, "Failed to load your job applications.");
            }
        }

        // Lấy chi tiết đơn ứng tuyển
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _jobApplicationService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Job application not found.");
            }
        }

        // Employer hoặc Admin cập nhật trạng thái đơn
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateJobApplicationStatusModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _jobApplicationService.UpdateStatusAsync(id, model);
                return success ? NoContent() : NotFound();
            }
            catch
            {
                return StatusCode(500, "Failed to update status.");
            }
        }

        // Worker hủy đơn ứng tuyển
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> CancelApplication(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            try
            {
                var success = await _jobApplicationService.CancelApplicationAsync(id, userId);
                return success ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Failed to cancel application.");
            }
        }
    }
}
