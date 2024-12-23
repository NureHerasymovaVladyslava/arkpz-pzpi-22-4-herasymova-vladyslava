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
        private readonly string _databaseName;

        public DatabaseAdminManager(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MasterConnectionString");
            _databaseName = config.GetConnectionString("TargetDatabaseName");
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = $@"BACKUP DATABASE [{_databaseName}]
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
            WHERE name = '{_databaseName}'";

            var disableConnectionsSql = $@"USE [master];
            ALTER DATABASE [{_databaseName}] 
            SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

            var restoreSql = $@"USE [master];
            RESTORE DATABASE [{_databaseName}]
            FROM DISK = @backupPath
            WITH REPLACE;";

            var enableConnectionsSql = $@"USE [master];
            ALTER DATABASE [{_databaseName}] 
            SET MULTI_USER;";

            var databaseExists = await connection
                .ExecuteScalarAsync<int>(checkDatabaseExistsSql) > 0;

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

        public async Task<bool> SetupDatabase(string backupFilePath)
        {
            if (!File.Exists(backupFilePath))
            {
                throw new FileNotFoundException
                    ("Database backup file not found.", backupFilePath);
            }

            using var connection = new SqlConnection(_connectionString);

            var checkDatabaseExistsSql = $@"
            SELECT COUNT(*) 
            FROM sys.databases 
            WHERE name = '{_databaseName}'";

            var databaseExists = await connection
                .ExecuteScalarAsync<int>(checkDatabaseExistsSql) > 0;

            if (databaseExists)
            {
                return false;
            }

            var restoreSql = $@"
            RESTORE DATABASE [{_databaseName}]
            FROM DISK = @backupFilePath
            WITH REPLACE;";

            await connection.ExecuteAsync(restoreSql, new { backupFilePath });

            return true;
        }
    }
}
