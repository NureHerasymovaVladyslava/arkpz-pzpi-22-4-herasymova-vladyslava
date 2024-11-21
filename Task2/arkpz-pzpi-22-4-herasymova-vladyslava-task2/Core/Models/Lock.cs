using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Lock")]
    public class Lock
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
    }
}
