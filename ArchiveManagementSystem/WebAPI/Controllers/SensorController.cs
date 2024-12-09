using Core.Enums;
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
    public class SensorController : ControllerBase
    {
        private readonly SensorRepository _sensorRepository;
        private readonly SensorLogRepository _sensorLogRepository;

        public SensorController(SensorRepository sensorRepository, SensorLogRepository sensorLogRepository)
        {
            _sensorRepository = sensorRepository;
            _sensorLogRepository = sensorLogRepository;
        }

        [HttpPost("create")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> CreateSensor([FromBody] CreateSensorModel model)
        {
            var sensor = new Sensor();
            sensor.MapFrom(model);

            try
            {
                var result = await _sensorRepository.CreateAsync(sensor);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditSensor([FromBody] EditSensorModel model)
        {
            try
            {
                var sensor = await _sensorRepository.GetByIdAsync(model.Id);
                if (sensor == null)
                {
                    return NotFound();
                }

                sensor.MapFrom(model);
                var result = await _sensorRepository.UpdateAsync(sensor);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            try
            {
                var result = await _sensorRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("room/{id}")]
        [Authorize(UserRoleManager.RoleAdmin, UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForRoom(int id, SensorType? sensorType)
        {
            try
            {
                var result = await _sensorRepository.GetForRoomAsync(id, sensorType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("diagnosis")]
        //[Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> ConductDiagnosis(int roomId, SensorType sensorType, int minutes)
        {
            try
            {
                if (minutes > 120)
                {
                    return BadRequest();
                }
                var sensors = await _sensorRepository.GetForRoomAsync(roomId, sensorType);
                var sensorLogs = new Dictionary<int, float[]>();

                var avgArray = new float[minutes];
                var valuesCount = new int[minutes];

                foreach (var sensor in sensors)
                {
                    var logs = await _sensorLogRepository.GetForSensorAsync(sensor.Id);
                    var values = logs.Take(minutes).Select(l => l.Value).ToArray();
                    sensorLogs.Add(sensor.Id, values);

                    for (int i = 0; i < values.Length; i++)
                    {
                        avgArray[i] += values[i];
                        valuesCount[i]++;
                    }
                }

                for (int i = 0; i < avgArray.Length; i++)
                {
                    if (valuesCount[i] == 0)
                    {
                        break;
                    }

                    avgArray[i] /= valuesCount[i];
                }

                var diagnosisResults = new List<SensorDiagnosisResult>();

                foreach (var sensor in sensors)
                {
                    var diagnosisResult = new SensorDiagnosisResult() { Id = sensor.Id };
                    float devSum = 0;

                    for (int i = 0; i < sensorLogs[sensor.Id].Length; i++)
                    {
                        var dif = (sensorLogs[sensor.Id][i] - avgArray[i]) / avgArray[i];

                        if (dif > 0)
                        {
                            devSum += dif;
                        }
                        else
                        {
                            devSum -= dif;
                        }
                    }

                    diagnosisResult.FailProbability = devSum / sensorLogs[sensor.Id].Length;

                    diagnosisResults.Add(diagnosisResult);
                }

                return Ok(diagnosisResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
