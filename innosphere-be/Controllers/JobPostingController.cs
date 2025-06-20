using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.Helpers;
using Service.Interfaces;
using Service.Models.JobPostings;
using Service.Models.PagedResultModels;
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

        [HttpGet]
        public async Task<ActionResult<PagedResultModel<JobPostingModel>>> GetJobPostings([FromQuery] JobPostingFilterModel filter)
        {
            try
            {
                // Set default values if not provided
                filter.Page = filter.Page <= 0 ? 1 : filter.Page;
                filter.PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;
                filter.PageSize = filter.PageSize > 100 ? 100 : filter.PageSize; // Limit max page size

                var result = await _jobPostingService.GetJobPostingsAsync(filter);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving job postings", error = ex.Message });
            }
        }

        // Lấy danh sách bài đăng theo employer
        [HttpGet("employer/{employerId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetByEmployer(int employerId, [FromQuery] string? status = null)
        {
            try
            {
                // Validate status if provided
                if (!string.IsNullOrEmpty(status))
                {
                    var validStatuses = new[] { "PENDING", "APPROVED", "CLOSED", "REJECT" };
                    if (!validStatuses.Contains(status.ToUpper()))
                    {
                        return BadRequest(new { message = "Invalid status. Valid values are: PENDING, APPROVED, CLOSED, REJECT" });
                    }
                    status = status.ToUpper(); // Normalize to uppercase
                }

                var result = await _jobPostingService.GetJobPostingsByEmployerAsync(employerId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employer job postings", error = ex.Message });
            }
        }

        // Lấy chi tiết bài đăng
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var jobPosting = await _jobPostingService.GetJobPostingByIdAsync(id);
                if (jobPosting == null)
                {
                    return NotFound(new { message = "Job posting not found." });
                }
                return Ok(jobPosting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the job posting.", error = ex.Message });
            }
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

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _jobPostingService.UpdateJobPostingStatusAsync(id, JobPostingStatusHelper.Approved);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin,Employer")]
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

        // Admin: Lấy tất cả job posting từ tất cả employers
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<JobPostingModel>>> GetAllForAdmin()
        {
            var jobPostings = await _jobPostingService.GetAllForAdminAsync();
            return Ok(jobPostings);
        }
    }
}