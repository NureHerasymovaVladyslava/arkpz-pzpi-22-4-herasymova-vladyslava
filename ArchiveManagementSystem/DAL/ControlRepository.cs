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
    public class ControlRepository : GenericRepository<Control>
    {
        public ControlRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<Control>> GetForRoomAsync(int roomId, int? typeId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = new StringBuilder(@"SELECT * FROM Control WHERE RoomId = @RoomId");

            if (typeId != null)
            {
                sql.Append(@" AND TypeId = @TypeId");
            }

            sql.Append(";");

            return await connection.QueryAsync<Control>
                (sql.ToString(), new { RoomId = roomId, TypeId = typeId });
        }
    }
}
