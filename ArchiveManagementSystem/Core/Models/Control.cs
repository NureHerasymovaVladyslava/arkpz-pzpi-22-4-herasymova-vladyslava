using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Control
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int TypeId { get; set; }
        public bool Working { get; set; }
    }
}
