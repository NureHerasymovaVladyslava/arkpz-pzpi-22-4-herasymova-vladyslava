﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("DocumentType")]
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
