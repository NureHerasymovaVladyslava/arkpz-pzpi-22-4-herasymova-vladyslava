using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("lock/{id}")]
        public async Task<IActionResult> GetForLock(int id)
        {
            // check user access level

            try
            {
                var result = await _lockLogRepository.GetForLockAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            // check user access level

            try
            {
                var result = await _lockLogRepository.GetUnprocessedAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("confirm/{id}")]
        public async Task<IActionResult> ConfirmRequest(int id)
        {
            // check user access level

            try
            {
                var lockLog = await _lockLogRepository.GetByIdAsync(id);
                if (lockLog == null)
                {
                    return NotFound();
                }

                // verification

                lockLog.Approved = true;
                var result = await _lockLogRepository.UpdateAsync(lockLog);

                // open lock

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("discard/{id}")]
        public async Task<IActionResult> DiscardRequest(int id)
        {
            // check user access level

            try
            {
                var lockLog = await _lockLogRepository.GetByIdAsync(id);
                if (lockLog == null)
                {
                    return NotFound();
                }
                
                // verification

                lockLog.Approved = false;
                var result = await _lockLogRepository.UpdateAsync(lockLog);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
