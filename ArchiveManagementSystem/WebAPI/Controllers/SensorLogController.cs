using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Sensor;
using Core.Enums;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorLogController : ControllerBase
    {
        private readonly SensorLogRepository _sensorLogRepository;
        private readonly SensorRepository _sensorRepository;
        private readonly GenericRepository<Room> _roomRepository;
        private readonly IHubContext<AlertHub> _hubContext;
        private readonly ILogger<SensorLogController> _logger;

        public SensorLogController(SensorLogRepository sensorLogRepository, 
            SensorRepository sensorRepository, GenericRepository<Room> roomRepository, 
            IHubContext<AlertHub> hubContext, ILogger<SensorLogController> logger)
        {
            _sensorLogRepository = sensorLogRepository;
            _sensorRepository = sensorRepository;
            _roomRepository = roomRepository;
            _hubContext = hubContext;
            _logger = logger;
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

                var sensor = await _sensorRepository.GetByIdAsync(sensorLog.SensorId);
                if (sensor == null)
                {
                    return NotFound();
                }

                if (sensor.RoomId == null)
                {
                    return NotFound();
                }
                var room = await _roomRepository.GetByIdAsync((int)sensor.RoomId);
                if (room == null)
                {
                    return NotFound();
                }

                if (sensor.SensorType == Core.Enums.MonitoringValue.Temperature
                    && (sensorLog.Value > room.TempMax || sensorLog.Value < room.TempMin))
                {
                    await _hubContext.Clients.All
                        .SendAsync(AlertHub.ReceiveAlertString, room.Id, "temp");

                    _logger.LogInformation("Temperature in room {roomId} outside the limits", room.Id); //Temp
                }

                if (sensor.SensorType == Core.Enums.MonitoringValue.Humidity
                    && (sensorLog.Value > room.HumMax || sensorLog.Value < room.HumMin))
                {
                    await _hubContext.Clients.All
                        .SendAsync(AlertHub.ReceiveAlertString, room.Id, "hum");

                    _logger.LogInformation("Humidity in room {roomId} outside the limits", room.Id); //Temp
                }

                if (sensor.SensorType == Core.Enums.MonitoringValue.Lighting
                    && (sensorLog.Value > room.LightMax || sensorLog.Value < room.LightMin))
                {
                    await _hubContext.Clients.All
                        .SendAsync(AlertHub.ReceiveAlertString, room.Id, "light");

                    _logger.LogInformation("Light in room {roomId} outside the limits", room.Id); //Temp
                }

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
