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
    public class DatabaseAdminManager
    {
        private readonly string _connectionString;

        public DatabaseAdminManager(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MasterConnectionString");
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"BACKUP DATABASE [ArchiveManagementSystem]
            TO DISK = @backupPath
            WITH FORMAT, INIT;";

            await connection.ExecuteAsync(sql, new { backupPath });
        }

        public async Task RestoreDatabaseAsync(string backupPath)
        {
            using var connection = new SqlConnection(_connectionString);

            var checkDatabaseExistsSql = $@"
            SELECT COUNT(*) 
            FROM sys.databases 
            WHERE name = 'ArchiveManagementSystem'";

            var disableConnectionsSql = $@"USE [master];
            ALTER DATABASE [ArchiveManagementSystem] 
            SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

            var restoreSql = $@"USE [master];
            RESTORE DATABASE [ArchiveManagementSystem]
            FROM DISK = @backupPath
            WITH REPLACE;";

            var enableConnectionsSql = $@"USE [master];
            ALTER DATABASE [ArchiveManagementSystem] 
            SET MULTI_USER;";

            var databaseExists = await connection.ExecuteScalarAsync<int>(checkDatabaseExistsSql) > 0;

            if (databaseExists)
            {
                await connection.ExecuteAsync(disableConnectionsSql);
            }

            await connection.ExecuteAsync(restoreSql, new { backupPath });

            if (databaseExists)
            {
                await connection.ExecuteAsync(enableConnectionsSql);
            }
        }
    }
}
