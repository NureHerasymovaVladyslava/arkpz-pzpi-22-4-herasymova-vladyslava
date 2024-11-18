using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SensorLog
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public float Value { get; set; }
        public DateTime LogTime { get; set; }
    }
}
