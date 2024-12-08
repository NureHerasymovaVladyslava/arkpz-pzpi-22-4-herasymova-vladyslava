using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Sensor;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorLogController : ControllerBase
    {
        private readonly SensorLogRepository _sensorLogRepository;

        public SensorLogController(SensorLogRepository sensorLogRepository)
        {
            _sensorLogRepository = sensorLogRepository;
        }

        // will be accessed from IoT devise, may be deleted in the future and
        // replaced with sanding a regular request from the server
        [HttpPost("create")]
        public async Task<IActionResult> CreateLog([FromBody] CreateSensorLogModel model)
        {
            var sensorLog = new SensorLog();
            sensorLog.MapFrom(model);
            sensorLog.LogTime = DateTime.Now;

            try
            {
                var result = await _sensorLogRepository.CreateAsync(sensorLog);

                // check for critical values

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("sensor/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForSensor(int id)
        {
            try
            {
                var result = await _sensorLogRepository.GetForSensorAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
