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

        public async Task<IEnumerable<Control>> GetForRoomAsync(int roomId, MonitoringValue? controlType)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = new StringBuilder(@"SELECT * FROM Control WHERE RoomId = @RoomId");

            if (controlType != null)
            {
                sql.Append(@" AND ControlType = @ControlType");
            }

            sql.Append(";");

            return await connection.QueryAsync<Control>
                (sql.ToString(), new { RoomId = roomId, ControlType = controlType });
        }
    }
}
