using Core.Enums;

namespace WebAPI.Models.Sensor
{
    public class CreateSensorModel
    {
        public int RoomId { get; set; }
        public SensorTypes SensorType { get; set; }
    }
}
