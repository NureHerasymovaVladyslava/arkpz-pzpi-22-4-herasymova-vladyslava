using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DAL
{
    public class GenericRepository<T> where T : class
    {
        protected readonly string _connectionString;

        public GenericRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConnectionString");
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.GetAsync<T>(id);
        }

        public async Task<int> CreateAsync(T entity)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.InsertAsync<T>(entity);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.UpdateAsync<T>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var entity = await GetByIdAsync(id);

            return await connection.DeleteAsync<T>(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.GetAllAsync<T>();
        }
    }
}
