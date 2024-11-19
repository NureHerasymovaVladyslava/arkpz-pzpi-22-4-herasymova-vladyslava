using Core.Enums;
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
    public class SensorController : ControllerBase
    {
        private readonly SensorRepository _sensorRepository;

        public SensorController(SensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSensor([FromBody] CreateSensorModel model)
        {
            // check user access level

            var sensor = new Sensor();
            sensor.MapFrom(model);

            try
            {
                var result = await _sensorRepository.CreateAsync(sensor);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditSensor([FromBody] EditSensorModel model)
        {
            // check user access level

            try
            {
                var sensor = await _sensorRepository.GetByIdAsync(model.Id);
                sensor.MapFrom(model);
                var result = await _sensorRepository.UpdateAsync(sensor);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            // check user access level

            try
            {
                var result = await _sensorRepository.DeleteAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetForRoom(int id, SensorTypes? sensorType)
        {
            // check user access level

            try
            {
                var result = await _sensorRepository.GetForRoomAsync(id, sensorType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
