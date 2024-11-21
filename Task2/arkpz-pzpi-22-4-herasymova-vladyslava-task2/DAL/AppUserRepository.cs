using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppUserRepository : GenericRepository<AppUser>
    {
        public AppUserRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM AppUser WHERE EmailAddress = @Email;";

            return await connection.QuerySingleAsync<AppUser>
                (sql, new { Email = email });
        }
    }
}
