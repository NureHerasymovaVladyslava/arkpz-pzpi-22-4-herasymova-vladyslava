using Core.Enums;

namespace WebAPI.Models.Sensor
{
    public class EditSensorModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public SensorType SensorType { get; set; }
    }
}
