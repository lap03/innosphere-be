using innosphere_be.Models.Requests.InitRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IInitService _initService;

        public InitController(IInitService initService)
        {
            _initService = initService;
        }

        // POST: api/seed/cities
        [HttpPost("cities")]
        public async Task<IActionResult> SeedCities()
        {
            var result = await _initService.SeedCitiesAsync();
            if (result.Succeeded)
                return Ok("Cities seeded successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/jobtags
        [HttpPost("jobtags")]
        public async Task<IActionResult> SeedJobTags()
        {
            var result = await _initService.SeedJobTagsAsync();
            if (result.Succeeded)
                return Ok("JobTags seeded successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/paymenttypes
        [HttpPost("paymenttypes")]
        public async Task<IActionResult> SeedPaymentTypes()
        {
            var result = await _initService.SeedPaymentTypesAsync();
            if (result.Succeeded)
                return Ok("PaymentTypes seeded successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/businesstypes
        [HttpPost("businesstypes")]
        public async Task<IActionResult> SeedBusinessTypes()
        {
            var result = await _initService.SeedBusinessTypesAsync();
            if (result.Succeeded)
                return Ok("BusinessTypes seeded successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/users
        [HttpPost("users")]
        public async Task<IActionResult> SeedUsers()
        {
            var result = await _initService.SeedUsersAsync();
            if (result.Succeeded)
                return Ok("Users seeded successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/user
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserInitReuest model)
        {
            var result = await _initService.CreateUserWithRoleAsync(model.Email, model.Password, model.RoleName);
            if (result.Succeeded)
                return Ok("User created successfully.");
            return BadRequest(result.Errors);
        }

        // PUT: api/seed/user/assign-role
        [HttpPut("user/assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest model)
        {
            var result = await _initService.AssignRoleAsync(model.Email, model.RoleName);
            if (result.Succeeded)
                return Ok("Role assigned successfully.");
            return BadRequest(result.Errors);
        }

        // POST: api/seed/all
        [HttpPost("all")]
        public async Task<IActionResult> SeedAll()
        {
            var results = new List<string>();
            var errors = new List<string>();

            var citiesResult = await _initService.SeedCitiesAsync();
            if (citiesResult.Succeeded)
                results.Add("Cities seeded successfully.");
            else
                errors.AddRange(citiesResult.Errors.Select(e => e.Description));

            var jobTagsResult = await _initService.SeedJobTagsAsync();
            if (jobTagsResult.Succeeded)
                results.Add("JobTags seeded successfully.");
            else
                errors.AddRange(jobTagsResult.Errors.Select(e => e.Description));

            var paymentTypesResult = await _initService.SeedPaymentTypesAsync();
            if (paymentTypesResult.Succeeded)
                results.Add("PaymentTypes seeded successfully.");
            else
                errors.AddRange(paymentTypesResult.Errors.Select(e => e.Description));

            var businessTypesResult = await _initService.SeedBusinessTypesAsync();
            if (businessTypesResult.Succeeded)
                results.Add("BusinessTypes seeded successfully.");
            else
                errors.AddRange(businessTypesResult.Errors.Select(e => e.Description));

            var usersResult = await _initService.SeedUsersAsync();
            if (usersResult.Succeeded)
                results.Add("Users seeded successfully.");
            else
                errors.AddRange(usersResult.Errors.Select(e => e.Description));

            if (errors.Count == 0)
                return Ok(new { Success = true, Messages = results });
            return BadRequest(new { Success = false, Messages = results, Errors = errors });
        }
    }
}
