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
    public class LockLogController : ControllerBase
    {
        private readonly LockLogRepository _lockLogRepository;

        public LockLogController(LockLogRepository lockLogRepository)
        {
            _lockLogRepository = lockLogRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLog([FromBody] CreateLockLogModel model)
        {
            var lockLog = new LockLog();
            lockLog.MapFrom(model);
            lockLog.LogTime = DateTime.Now;

            try
            {
                var result = await _lockLogRepository.CreateAsync(lockLog);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("lock/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForLock(int id)
        {
            try
            {
                var result = await _lockLogRepository.GetForLockAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("requests")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var result = await _lockLogRepository.GetUnprocessedAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("confirm/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> ConfirmRequest(int id)
        {
            try
            {
                var lockLog = await _lockLogRepository.GetByIdAsync(id);
                if (lockLog == null)
                {
                    return NotFound();
                }

                if (lockLog.Approved != null)
                {
                    return BadRequest();
                }

                lockLog.Approved = true;
                lockLog.ApprovedTime = DateTime.Now;
                var result = await _lockLogRepository.UpdateAsync(lockLog);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("discard/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> DiscardRequest(int id)
        {
            try
            {
                var lockLog = await _lockLogRepository.GetByIdAsync(id);
                if (lockLog == null)
                {
                    return NotFound();
                }

                if (lockLog.Approved != null)
                {
                    return BadRequest();
                }

                lockLog.Approved = false;
                var result = await _lockLogRepository.UpdateAsync(lockLog);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
