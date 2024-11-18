using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Room
    {
        public int Id { get; set; }
        public float TempMax { get; set; }
        public float TempMin { get; set; }
        public float HumMax { get; set; }
        public float HumMin { get; set; }
        public int LightMax { get; set; }
        public int LightMin { get; set; }
    }
}
