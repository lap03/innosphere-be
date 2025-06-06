using Microsoft.AspNetCore.Mvc;
using Service.Models.SubscriptionModels;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubscriptionModel>>> GetAll()
    {
        var list = await _subscriptionService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubscriptionModel>> GetById(int id)
    {
        var subscription = await _subscriptionService.GetByIdAsync(id);
        return Ok(subscription);
    }

    [HttpPost]
    public async Task<ActionResult<SubscriptionModel>> Create(CreateSubscriptionModel dto)
    {
        var created = await _subscriptionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubscriptionModel>> Update(int id, UpdateSubscriptionModel dto)
    {
        var updated = await _subscriptionService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> SoftDelete(int id)
    {
        await _subscriptionService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/restore")]
    public async Task<ActionResult> Restore(int id)
    {
        await _subscriptionService.RestoreAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}/hard")]
    public async Task<ActionResult> HardDelete(int id)
    {
        await _subscriptionService.HardDeleteAsync(id);
        return NoContent();
    }

    // API kiểm tra có thể đăng tin được không
    [HttpGet("{subscriptionId}/can-post-job")]
    public async Task<ActionResult<bool>> CanPostJob(int subscriptionId)
    {
        var canPost = await _subscriptionService.CanPostJobAsync(subscriptionId);
        return Ok(canPost);
    }

    // API mua gói mới, forceReplace = true tự hủy gói cũ
    [HttpPost("purchase")]
    public async Task<ActionResult<SubscriptionModel>> Purchase([FromBody] CreateSubscriptionModel dto, [FromQuery] bool forceReplace = false)
    {
        var result = await _subscriptionService.PurchaseSubscriptionAsync(dto, forceReplace);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
