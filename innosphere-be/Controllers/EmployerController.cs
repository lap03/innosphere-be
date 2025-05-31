using AutoMapper;
using innosphere_be.Models.responses.EmployerResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using Service.Interfaces;
using Service.Models.EmployerModels;
using System.Security.Claims;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesHelper.Employer)]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        private readonly IMapper _mapper;

        public EmployerController(IEmployerService employerService, IMapper mapper)
        {
            _employerService = employerService;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in claims.");

            var employer = await _employerService.GetProfileAsync(userId);
            var response = _mapper.Map<EmployerProfileResponse>(employer);
            return Ok(response);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] EmployerEditModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in claims.");

            var employer = await _employerService.CreateProfileAsync(userId, model);
            var response = _mapper.Map<EmployerProfileResponse>(employer);
            return Ok(response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] EmployerEditModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in claims.");

            var employer = await _employerService.UpdateProfileAsync(userId, model);
            var response = _mapper.Map<EmployerProfileResponse>(employer);
            return Ok(response);
        }
    }
}