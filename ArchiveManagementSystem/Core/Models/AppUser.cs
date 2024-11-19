using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("AppUser")]
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}
