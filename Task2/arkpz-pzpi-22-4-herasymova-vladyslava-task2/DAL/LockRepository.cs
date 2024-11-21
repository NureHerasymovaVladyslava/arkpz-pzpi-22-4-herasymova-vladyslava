using Core.Enums;
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
    public class LockRepository : GenericRepository<Lock>
    {
        public LockRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<Lock>> GetForRoomAsync(int roomId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM Lock WHERE RoomId = @RoomId;";

            return await connection.QueryAsync<Lock>
                (sql, new { RoomId = roomId });
        }
    }
}
