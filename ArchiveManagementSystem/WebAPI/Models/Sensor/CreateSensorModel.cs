using Core.Enums;

namespace WebAPI.Models.Sensor
{
    public class CreateSensorModel
    {
        public int RoomId { get; set; }
        public SensorType SensorType { get; set; }
    }
}
