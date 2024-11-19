using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateLog([FromBody] CreateSensorLogModel model)
        {
            // check user access level

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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sensor/{id}")]
        public async Task<IActionResult> GetForSensor(int id)
        {
            // check user access level

            try
            {
                var result = await _sensorLogRepository.GetForSensorAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
