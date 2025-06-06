﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers;
using Service.Interfaces;
using Service.Models.WorkerModels;
using System.Security.Claims;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesHelper.Worker)]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        private readonly IMapper _mapper;

        public WorkerController(IWorkerService workerService, IMapper mapper)
        {
            _workerService = workerService;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var worker = await _workerService.GetProfileAsync(userId);
            return Ok(worker);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] WorkerEditModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var worker = await _workerService.CreateProfileAsync(userId, model);
            return Ok(worker);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] WorkerEditModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var worker = await _workerService.UpdateProfileAsync(userId, model);
            return Ok(worker);
        }
    }
}
