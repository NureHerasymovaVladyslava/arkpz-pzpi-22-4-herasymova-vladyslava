using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("DocumentLog")]
    public class DocumentLog
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public DateTime LogTime { get; set; }
        public string? NewName { get; set; }
        public int? NewRoomId { get; set; }
        public int? NewStatusId { get; set; }
        public int? NewTypeId { get; set; }
        public string? NewAdditionalInfo { get; set; }
        public bool? Approved { get; set; }
    }
}
