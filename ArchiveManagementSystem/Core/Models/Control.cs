using Core.Enums;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Control")]
    public class Control
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public MonitoringValue ControlType { get; set; }
        public bool Working { get; set; }
    }
}
