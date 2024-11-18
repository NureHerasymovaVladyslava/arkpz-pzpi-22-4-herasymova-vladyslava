using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public SensorTypes SensorType { get; set; }
    }
}
