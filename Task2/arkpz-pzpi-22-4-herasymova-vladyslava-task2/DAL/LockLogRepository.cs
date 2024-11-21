using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LockLogRepository : GenericRepository<LockLog>
    {
        public LockLogRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<LockLog>> GetForLockAsync(int lockId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM LockLog WHERE LockId = @LockId ORDER BY LogTime;";

            return await connection.QueryAsync<LockLog>
                (sql, new { LockId = lockId });
        }

        public async Task<IEnumerable<LockLog>> GetUnprocessedAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM LockLog WHERE Approved IS NULL ORDER BY LogTime;";


            return await connection.QueryAsync<LockLog>(sql);
        }
    }
}
