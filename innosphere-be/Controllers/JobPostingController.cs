using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using Service.Interfaces;
using Service.Models.JobPostings;
using Service.Services;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingService _jobPostingService;

        public JobPostingController(IJobPostingService jobPostingService)
        {
            _jobPostingService = jobPostingService;
        }

        // Tạo bài đăng mới
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Create([FromBody] CreateJobPostingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobPostingService.CreateJobPostingAsync(model, model.TagIds);
            return Ok(result);
        }

        // Lấy danh sách bài đăng theo employer
        [HttpGet("employer/{employerId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetByEmployer(int employerId)
        {
            var result = await _jobPostingService.GetJobPostingsByEmployerAsync(employerId);
            return Ok(result);
        }

        // Lấy chi tiết bài đăng
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobPostingService.GetJobPostingByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _jobPostingService.UpdateJobPostingStatusAsync(id, JobPostingStatusHelper.Approved);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _jobPostingService.UpdateJobPostingStatusAsync(id, JobPostingStatusHelper.Rejected);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id}/close")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Close(int id)
        {
            var result = await _jobPostingService.UpdateJobPostingStatusAsync(id, JobPostingStatusHelper.Closed);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _jobPostingService.UpdateJobPostingStatusAsync(id, JobPostingStatusHelper.Completed);
            return result ? Ok() : NotFound();
        }
    }
}