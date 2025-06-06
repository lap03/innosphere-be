using Microsoft.AspNetCore.Mvc;
using Service.Models.AdvertisementModels;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AdvertisementController : ControllerBase
{
    private readonly IAdvertisementService _adService;

    public AdvertisementController(IAdvertisementService adService)
    {
        _adService = adService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdvertisementModel>>> GetAll()
    {
        var list = await _adService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<AdvertisementModel>>> GetAllActive()
    {
        var list = await _adService.GetAllActiveAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdvertisementModel>> GetById(int id)
    {
        var ad = await _adService.GetByIdAsync(id);
        return Ok(ad);
    }

    [HttpPost]
    public async Task<ActionResult<AdvertisementModel>> Create(CreateAdvertisementModel dto)
    {
        var created = await _adService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdvertisementModel>> Update(int id, UpdateAdvertisementModel dto)
    {
        var updated = await _adService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> SoftDelete(int id)
    {
        await _adService.SoftDeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/restore")]
    public async Task<ActionResult> Restore(int id)
    {
        await _adService.RestoreAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}/hard")]
    public async Task<ActionResult> HardDelete(int id)
    {
        await _adService.HardDeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/increment-impression")]
    public async Task<ActionResult> IncrementImpression(int id)
    {
        await _adService.IncrementImpressionAsync(id);
        return NoContent();
    }
}
