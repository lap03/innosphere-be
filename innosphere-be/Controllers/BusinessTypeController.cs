using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.BusinessTypeModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessTypeController : ControllerBase
    {
        private readonly IBusinessTypeService _businessTypeService;

        public BusinessTypeController(IBusinessTypeService businessTypeService)
        {
            _businessTypeService = businessTypeService;
        }

        // GET: api/businessType
        [HttpGet]
        public async Task<ActionResult<List<BusinessTypeModel>>> GetAll()
        {
            var list = await _businessTypeService.GetAllAsync();
            return Ok(list);
        }

        // GET: api/businessType/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessTypeModel>> GetById(int id)
        {
            try
            {
                var item = await _businessTypeService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/businessType
        [HttpPost]
        public async Task<ActionResult<BusinessTypeModel>> Create([FromBody] CreateBusinessTypeModel model)
        {
            try
            {
                var created = await _businessTypeService.CreateAsync(model);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/businessType/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BusinessTypeModel>> Update(int id, [FromBody] UpdateBusinessTypeModel model)
        {
            try
            {
                var updated = await _businessTypeService.UpdateAsync(id, model);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/businessType/{id} (Soft delete)
        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            try
            {
                await _businessTypeService.SoftDeleteAsync(id);
                return Ok(new { message = "Soft delete successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
