using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Control;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlTypeController : ControllerBase
    {
        private readonly GenericRepository<ControlType> _controlTypeRepository;

        public ControlTypeController(GenericRepository<ControlType> controlTypeRepository)
        {
            _controlTypeRepository = controlTypeRepository;
        }

        [HttpPost("create")]
        [Authorize (UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> CreateType([FromBody] CreateControlTypeModel model)
        {
            var type = new ControlType();
            type.MapFrom(model);

            try
            {
                var result = await _controlTypeRepository.CreateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditType([FromBody] EditControlTypeModel model)
        {
            try
            {
                var type = await _controlTypeRepository.GetByIdAsync(model.Id);
                if (type == null)
                {
                    return NotFound();
                }

                type.MapFrom(model);
                var result = await _controlTypeRepository.UpdateAsync(type);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteType(int id)
        {
            try
            {
                var result = await _controlTypeRepository.DeleteAsync(id);

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
                var result = await _controlTypeRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
