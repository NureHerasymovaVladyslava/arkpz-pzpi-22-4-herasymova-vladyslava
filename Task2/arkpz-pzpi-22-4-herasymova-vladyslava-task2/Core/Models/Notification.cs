using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Notification")]
    public class Notification
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int TypeId { get; set; }
        public DateTime Sent { get; set; }
        public string Text { get; set; }
    }
}
