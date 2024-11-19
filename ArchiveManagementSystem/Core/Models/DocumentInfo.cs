using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("DocumentInfo")]
    public class DocumentInfo
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
