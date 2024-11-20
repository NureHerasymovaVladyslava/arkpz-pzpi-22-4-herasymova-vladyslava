using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.AppUser;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserRepository _userRepository;

        public AppUserController(AppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            // check user access level

            var appUser = new AppUser();
            appUser.MapFrom(model);

            // generate password
            appUser.PasswordHash = "1"; // Temp

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            // check user access level

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
        public async Task<IActionResult> EditUser([FromBody] EditUserModel model)
        {
            // check user access level

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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            // validate new password

            try
            {
                var user = await _userRepository.GetByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }
                // Temp - later should probably be raplaced with session storage of sorts

                // get passwords hashes
                // chack if old password is correct

                user.PasswordHash = model.NewPassword; // Temp

                var result = await _userRepository.UpdateAsync(user);
                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                // Temp - later should probably be raplaced with session storage of sorts

                // generate password
                user.PasswordHash = "1"; // Temp

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

                // check password
                if (false)
                {
                    return BadRequest();
                }
                // start session

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            // end session

            return Ok();
        }
    }
}
