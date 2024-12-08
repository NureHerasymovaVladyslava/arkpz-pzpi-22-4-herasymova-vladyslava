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
    public class SensorLogRepository : GenericRepository<SensorLog>
    {
        public SensorLogRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<SensorLog>> GetForSensorAsync(int sensorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM SensorLog WHERE SensorId = @SensorId
                        ORDER BY LogTime DESC;";

            return await connection.QueryAsync<SensorLog>
                (sql, new { SensorId = sensorId });
        }
    }
}
