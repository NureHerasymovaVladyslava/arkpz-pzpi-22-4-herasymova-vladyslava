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
    public class UserRoleRepository : GenericRepository<UserRole>
    {
        public UserRoleRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<UserRole> GetRoleByName(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM UserRole WHERE Name = @Name;";

            return await connection.QuerySingleAsync<UserRole>
                (sql, new { Name = name });
        }
    }
}
