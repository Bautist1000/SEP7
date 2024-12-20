using Microsoft.AspNetCore.Mvc;
using AquAnalyzerAPI.Interfaces;
using AquAnalyzerAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace AquAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllNotifications();
                return Ok(notifications);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationById(id);
            if (notification == null) return NotFound();

            return Ok(notification);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(int userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserId(userId);
            if (notifications == null) return NotFound();

            return Ok(notifications);
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> AddNotification(Notification notification)
        {
            await _notificationService.AddNotification(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, notification);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNotificationStatus(int id, string status)
        {
            var success = await _notificationService.UpdateNotificationStatus(id, status);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id, [FromBody] DateTime readAt)
        {
            var result = await _notificationService.MarkAsRead(id, readAt);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isResolved)
        {
            var result = await _notificationService.UpdateStatus(id, isResolved);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _notificationService.DeleteNotification(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByType(string type)
        {
            var notifications = await _notificationService.GetNotificationsByType(type);
            return Ok(notifications);
        }
    }

}
