using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.NotificationModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace innosphere_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Lấy tất cả thông báo của người dùng đang đăng nhập.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy toàn bộ thông báo đang hoạt động – chỉ Admin mới có quyền.
        /// </summary>
        [HttpGet("active")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllActiveForAdmin()
        {
            var result = await _notificationService.GetAllActiveNotificationsForAdminAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông báo theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var result = await _notificationService.GetNotificationByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Tạo thông báo mới cho người dùng hiện tại.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationModel dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _notificationService.CreateNotificationAsync(dto, userId);
            return CreatedAtAction(nameof(GetNotificationById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Cập nhật thông báo.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationModel dto)
        {
            var result = await _notificationService.UpdateNotificationAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Xóa mềm một thông báo.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteNotification(int id)
        {
            await _notificationService.SoftDeleteNotificationAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Khôi phục thông báo bị xóa mềm.
        /// </summary>
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreNotification(int id)
        {
            await _notificationService.RestoreNotificationAsync(id);
            return Ok(new { message = "Restore successful" });
        }

        /// <summary>
        /// Xóa cứng khỏi database.
        /// </summary>
        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteNotification(int id)
        {
            await _notificationService.HardDeleteNotificationAsync(id);
            return NoContent();
        }
    }
}
