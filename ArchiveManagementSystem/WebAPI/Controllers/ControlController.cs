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
        public async Task<IActionResult> CreateControl([FromBody] MonitoringValue controlType)
        {
            var sensor = new Control()
            {
                ControlType = controlType,
                Working = false
            };

            try
            {
                var result = await _controlRepository.CreateAsync(sensor);

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
        public async Task<IActionResult> GetForRoom(int id, MonitoringValue? controlType)
        {
            try
            {
                var result = await _controlRepository.GetForRoomAsync(id, controlType);

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

        [HttpGet("check-state/{id}")]
        public async Task<IActionResult> CheckState(int id)
        {
            try
            {
                var control = await _controlRepository.GetByIdAsync(id);
                if (control == null)
                {
                    return NotFound();
                }

                return Ok(control.Working);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
