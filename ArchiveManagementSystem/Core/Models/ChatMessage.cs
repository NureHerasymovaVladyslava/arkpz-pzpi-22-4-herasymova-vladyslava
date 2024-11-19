using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("ChatMessage")]
    public class ChatMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Sent { get; set; }
        public string Text { get; set; }
    }
}
