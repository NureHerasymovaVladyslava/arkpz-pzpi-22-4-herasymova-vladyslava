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
    public class DocumentRepository : GenericRepository<Document>
    {
        private readonly Dictionary<OrderBy, string> _orderString = new()
        {
            {OrderBy.CreatedAsc, " ORDER BY Created ASC;" },
            {OrderBy.CreatedDesc, " ORDER BY Created DESC;" },
            {OrderBy.NameAsc, " ORDER BY Name ASC;" },
            {OrderBy.NameDesc, " ORDER BY Name Desc;" }
        };
        public DocumentRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(int? roomId, int? statusId, 
            int? typeId, string searchQuery = "", OrderBy orderBy = OrderBy.CreatedDesc)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = new StringBuilder($"SELECT * FROM Document WHERE Name LIKE '%{searchQuery}%'");

            if (roomId != null)
            {
                sql.Append($" AND RoomId = {roomId}");
            }
            if (statusId != null)
            {
                sql.Append($" AND StatusId = {statusId}");
            }
            if (typeId != null)
            {
                sql.Append($" AND TypeId = {typeId}");
            }

            sql.Append(_orderString[orderBy]);

            return await connection.QueryAsync<Document>(sql.ToString());
        }
    }
}
