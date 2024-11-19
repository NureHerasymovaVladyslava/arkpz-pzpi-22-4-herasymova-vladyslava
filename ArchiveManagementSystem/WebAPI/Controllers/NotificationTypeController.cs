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
    public class NotificationTypeController : ControllerBase
    {
        private readonly GenericRepository<NotificationType> _notificationTypeRepository;

        public NotificationTypeController(GenericRepository<NotificationType> notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateType([FromBody] string name)
        {
            // check user access level

            var type = new NotificationType();
            type.Name = name;

            try
            {
                var result = await _notificationTypeRepository.CreateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditType([FromBody] EditNotificationTypeModel model)
        {
            // check user access level

            try
            {
                var type = await _notificationTypeRepository.GetByIdAsync(model.Id);
                if (type == null)
                {
                    return NotFound();
                }

                type.MapFrom(model);
                var result = await _notificationTypeRepository.UpdateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            // check user access level

            try
            {
                var result = await _notificationTypeRepository.DeleteAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // check user access level

            try
            {
                var result = await _notificationTypeRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
