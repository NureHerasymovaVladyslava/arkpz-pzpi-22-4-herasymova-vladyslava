using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Managers;
using WebAPI.Models.AppUser;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly DatabaseAdminManager _databaseAdminManager;
        private readonly AppUserRepository _userRepository;
        private readonly UserRoleRepository _roleRepository;
        private readonly EmailManager _emailManager;

        public SetupController(DatabaseAdminManager databaseAdminManager, 
            AppUserRepository userRepository, UserRoleRepository roleRepository, 
            EmailManager emailManager)
        {
            _databaseAdminManager = databaseAdminManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _emailManager = emailManager;
        }

        [HttpPost("database")]
        public async Task<IActionResult> SetupDatabase(string backupFilePath)
        {
            try
            {
                var result = await _databaseAdminManager.SetupDatabase(backupFilePath);

                return result ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("first-user")]
        public async Task<IActionResult> CreateFirstUser([FromBody] CreateFirstUserModel model)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                if (users.Any())
                {
                    return BadRequest();
                }

                var appUser = new AppUser();
                appUser.MapFrom(model);

                var userRole = await _roleRepository.GetRoleByName(UserRoleManager.RoleAdmin);
                if (userRole == null)
                {
                    return NotFound();
                }

                appUser.RoleId = userRole.Id;

                var tempPassword = PasswordManager.GenerateTemporaryPassword();
                appUser.PasswordHash = HashPassword(tempPassword);

                var result = await _userRepository.CreateAsync(appUser);

                var emailResult = await _emailManager
                    .SendNewUserEmail(appUser.FullName, tempPassword, appUser.EmailAddress);

                return emailResult ? Ok(result) : StatusCode(
                    StatusCodes.Status500InternalServerError, "Failed to send an email");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
