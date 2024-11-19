using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("LockLog")]
    public class LockLog
    {
        public int Id { get; set; }
        public int LockId { get; set; }
        public int UserId { get; set; }
        public DateTime LogTime { get; set; }
        public bool? Approved { get; set; }
    }
}
