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
    public class DocumentLogRepository : GenericRepository<DocumentLog>
    {
        public DocumentLogRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<DocumentLog>> GetForDocumentAsync(int documentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM DocumentLog WHERE DocumentId = @DocumentId ORDER BY LogTime;";

            return await connection.QueryAsync<DocumentLog>
                (sql, new { DocumentId = documentId });
        }

        public async Task<IEnumerable<DocumentLog>> GetUnprocessedAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM DocumentLog WHERE Approved IS NULL ORDER BY LogTime;";


            return await connection.QueryAsync<DocumentLog>(sql);
        }
    }
}
