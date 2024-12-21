using Core.Enums;
using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Lock;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockController : ControllerBase
    {
        private readonly LockRepository _lockRepository;
        private readonly LockLogRepository _lockLogRepository;

        public LockController(LockRepository lockRepository, LockLogRepository lockLogRepository)
        {
            _lockRepository = lockRepository;
            _lockLogRepository = lockLogRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLock()
        {
            var @lock = new Lock();

            try
            {
                var result = await _lockRepository.CreateAsync(@lock);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("check-state/{id}")]
        public async Task<IActionResult> CheckState(int id)
        {
            try
            {
                var logs = await _lockLogRepository.GetForLockAsync(id);

                return Ok(logs.Any(l => l.Approved != null && (bool)l.Approved 
                    && l.ApprovedTime > DateTime.Now.AddSeconds(-30)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditLock([FromBody] EditLockModel model)
        {
            try
            {
                var @lock = await _lockRepository.GetByIdAsync(model.Id);
                if (@lock == null)
                {
                    return NotFound();
                }

                @lock.MapFrom(model);
                var result = await _lockRepository.UpdateAsync(@lock);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteLock(int id)
        {
            try
            {
                var result = await _lockRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("room/{id}")]
        [Authorize(UserRoleManager.RoleAdmin, UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForRoom(int id)
        {
            try
            {
                var result = await _lockRepository.GetForRoomAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
