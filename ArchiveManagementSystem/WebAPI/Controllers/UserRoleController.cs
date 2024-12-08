using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.AppUser;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly GenericRepository<UserRole> _userRoleRepository;

        public UserRoleController(GenericRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpPost("create")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> CreateRole([FromBody] string name)
        {
            var role = new UserRole
            {
                Name = name
            };

            try
            {
                var result = await _userRoleRepository.CreateAsync(role);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditRole([FromBody] EditUserRoleModel model)
        {
            try
            {
                var role = await _userRoleRepository.GetByIdAsync(model.Id);
                if (role == null)
                {
                    return NotFound();
                }

                role.MapFrom(model);
                var result = await _userRoleRepository.UpdateAsync(role);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _userRoleRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userRoleRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
