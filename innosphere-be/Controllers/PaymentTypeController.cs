using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.PaymentTypeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentTypeService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _paymentTypeService.GetAllActiveAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _paymentTypeService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentTypeModel dto)
        {
            var result = await _paymentTypeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentTypeModel dto)
        {
            var result = await _paymentTypeService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _paymentTypeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _paymentTypeService.RestoreAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _paymentTypeService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
