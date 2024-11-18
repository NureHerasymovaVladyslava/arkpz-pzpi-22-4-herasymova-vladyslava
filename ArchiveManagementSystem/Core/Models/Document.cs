using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public DateTime Added { get; set; }
    }
}
