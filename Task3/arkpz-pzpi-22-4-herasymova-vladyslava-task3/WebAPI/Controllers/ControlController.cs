using Core.Enums;
using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Control;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly ControlRepository _controlRepository;

        public ControlController(ControlRepository controlRepository)
        {
            _controlRepository = controlRepository;
        }

        [HttpPost("create")]
        [Authorize (UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> CreateControl([FromBody] CreateControlModel model)
        {
            var control = new Control();
            control.MapFrom(model);
            control.Working = false;

            try
            {
                var result = await _controlRepository.CreateAsync(control);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize (UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditControl([FromBody] EditControlModel model)
        {
            try
            {
                var control = await _controlRepository.GetByIdAsync(model.Id);
                if (control == null)
                {
                    return NotFound();
                }

                control.MapFrom(model);
                var result = await _controlRepository.UpdateAsync(control);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize (UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteControl(int id)
        {
            try
            {
                var result = await _controlRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("room/{id}")]
        [Authorize(UserRoleManager.RoleAdmin, UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForRoom(int id, int? typeId)
        {
            try
            {
                var result = await _controlRepository.GetForRoomAsync(id, typeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("turn-on/{id}")]
        [Authorize (UserRoleManager.RoleManager)]
        public async Task<IActionResult> TurnOn(int id)
        {
            // logic for turning control device on

            return Ok();
        }

        // accessed by control device after it is turned on
        [HttpPut("confirm-on/{id}")]
        public async Task<IActionResult> ConfirmOn(int id)
        {
            try
            {
                var control = await _controlRepository.GetByIdAsync(id);
                if (control == null)
                {
                    return NotFound();
                }

                control.Working = true;
                var result = await _controlRepository.UpdateAsync(control);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("turn-off/{id}")]
        [Authorize (UserRoleManager.RoleManager)]
        public async Task<IActionResult> TurnOff(int id)
        {
            // logic for turning control device back off

            return Ok();
        }

        // accessed by control device after it is turned off
        [HttpPut("confirm-off/{id}")]
        public async Task<IActionResult> ConfirmOff(int id)
        {
            try
            {
                var control = await _controlRepository.GetByIdAsync(id);
                if (control == null)
                {
                    return NotFound();
                }

                control.Working = false;
                var result = await _controlRepository.UpdateAsync(control);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
