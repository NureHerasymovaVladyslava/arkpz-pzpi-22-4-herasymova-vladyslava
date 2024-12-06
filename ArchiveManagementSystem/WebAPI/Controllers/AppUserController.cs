using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.AppUser;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private const string SessionUserIdString = "UserId";
        private readonly AppUserRepository _userRepository;
        private readonly UserRoleManager _roleManager;

        private AppUser CurrentUser { get => HttpContext.Items["User"] as AppUser; }

        public AppUserController(AppUserRepository userRepository, UserRoleManager roleManager)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            //var currentUser = HttpContext.Items["User"] as AppUser;
            var roleResult = await _roleManager.IsUserInRole(CurrentUser, "Admin");
            if (!roleResult)
            {
                return Unauthorized();
            }

            var appUser = new AppUser();
            appUser.MapFrom(model);

            var tempPassword = PasswordManager.GenerateTemporaryPassword();
            appUser.PasswordHash = HashPassword(tempPassword);

            try
            {
                var result = await _userRepository.CreateAsync(appUser);

                // send invitation and password to email 

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            //var currentUser = HttpContext.Items["User"] as AppUser;
            return Ok(CurrentUser);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            //var currentUser = HttpContext.Items["User"] as AppUser;
            var roleResult = await _roleManager.IsUserInRole(CurrentUser, "Admin");
            if (!roleResult)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _userRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromBody] EditUserModel model)
        {
            //var currentUser = HttpContext.Items["User"] as AppUser;
            var roleResult = await _roleManager.IsUserInRole(CurrentUser, "Admin");
            if (!roleResult)
            {
                return Unauthorized();
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.MapFrom(model);
                var result = await _userRepository.UpdateAsync(user);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var currentUser = CurrentUser;
                if (HashPassword(model.OldPassword) == currentUser.PasswordHash)
                {
                    string errorMessage;
                    if (!PasswordManager.IsValidPassword(model.NewPassword, out errorMessage))
                    {
                        return BadRequest(errorMessage);
                    }

                    currentUser.PasswordHash = HashPassword(model.NewPassword);
                }

                var result = await _userRepository.UpdateAsync(currentUser);
                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound();
                }

                var tempPassword = PasswordManager.GenerateTemporaryPassword();
                user.PasswordHash = HashPassword(tempPassword);

                var result = await _userRepository.UpdateAsync(user);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                // send temporary password to email 

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInModel model)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    return NotFound();
                }

                if (HashPassword(model.Password) != user.PasswordHash)
                {
                    return BadRequest();
                }

                HttpContext.Session.SetInt32(SessionUserIdString, user.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove(SessionUserIdString);

            return Ok();
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
