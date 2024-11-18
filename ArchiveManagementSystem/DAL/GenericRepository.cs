using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DAL
{
    public class GenericRepository<T> where T : class
    {
        private readonly string _connectionString;

        public GenericRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConnectionString");
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM @TableName WHERE Id = @Id;";

            return await connection.QuerySingleAsync<T>
                (sql, new { TableName = typeof(T).FullName, Id = id });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"DELETE FROM @TableName WHERE Id = @Id;";

            await connection.ExecuteAsync(sql,
                new { TableName = typeof(T).FullName, Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM @TableName;";

            return await connection.QueryAsync<T>
                (sql, new { TableName = typeof(T).FullName });
        }
    }
}
