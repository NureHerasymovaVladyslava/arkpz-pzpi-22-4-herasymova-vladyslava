using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Notification;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly GenericRepository<Notification> _notificationRepository;

        public NotificationController(GenericRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationModel model)
        {
            // check user access level

            var notification = new Notification();
            notification.MapFrom(model);
            notification.Sent = DateTime.Now;

            try
            {
                var result = await _notificationRepository.CreateAsync(notification);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // will be accessed from Android, may be modified in the future
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // check user access level

            try
            {
                var result = await _notificationRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
