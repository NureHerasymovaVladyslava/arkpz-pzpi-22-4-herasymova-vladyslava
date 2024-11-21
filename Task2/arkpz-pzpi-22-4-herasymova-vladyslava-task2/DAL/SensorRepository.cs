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
    public class SensorRepository : GenericRepository<Sensor>
    {
        public SensorRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<Sensor>> GetForRoomAsync(int roomId, SensorTypes? sensorType)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = new StringBuilder(@"SELECT * FROM Sensor WHERE RoomId = @RoomId");

            if (sensorType != null)
            {
                sql.Append(@" AND SensorType = @SensorType");
            }

            sql.Append(";");

            return await connection.QueryAsync<Sensor>
                (sql.ToString(), new { RoomId = roomId, SensorType = sensorType });
        }
    }
}
