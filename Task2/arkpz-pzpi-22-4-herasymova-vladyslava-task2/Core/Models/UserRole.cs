﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("UserRole")]
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
