﻿using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
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
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> CreateType([FromBody] string name)
        {
            var type = new NotificationType();
            type.Name = name;

            try
            {
                var result = await _notificationTypeRepository.CreateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> EditType([FromBody] EditNotificationTypeModel model)
        {
            try
            {
                var type = await _notificationTypeRepository.GetByIdAsync(model.Id);
                if (type == null)
                {
                    return NotFound();
                }

                type.MapFrom(model);
                var result = await _notificationTypeRepository.UpdateAsync(type);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> DeleteType(int id)
        {
            try
            {
                var result = await _notificationTypeRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _notificationTypeRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
