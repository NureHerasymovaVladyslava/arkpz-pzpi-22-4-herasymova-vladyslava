using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseAdminController : ControllerBase
    {
        private readonly DatabaseAdminManager _databaseAdminManager;

        public DatabaseAdminController(DatabaseAdminManager databaseAdminManager) 
        {
            _databaseAdminManager = databaseAdminManager;
        }

        [HttpPost("backup")]
        [Authorize(UserRoleManager.RoleDatabaseAdmin)]
        public async Task<IActionResult> Backup(string path)
        {
            try
            {
                await _databaseAdminManager.BackupDatabaseAsync(path);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("restore")]
        [Authorize(UserRoleManager.RoleDatabaseAdmin)]
        public async Task<IActionResult> Restore(string path)
        {
            try
            {
                await _databaseAdminManager.RestoreDatabaseAsync(path);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
