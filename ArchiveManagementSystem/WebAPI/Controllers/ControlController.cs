using Core.Enums;
using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
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
        public async Task<IActionResult> CreateControl([FromBody] CreateControlModel model)
        {
            // check user access level

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
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditControl([FromBody] EditControlModel model)
        {
            // check user access level

            try
            {
                var control = await _controlRepository.GetByIdAsync(model.Id);
                if (control == null)
                {
                    return NotFound();
                }

                control.MapFrom(model);
                var result = await _controlRepository.UpdateAsync(control);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteControl(int id)
        {
            // check user access level

            try
            {
                var result = await _controlRepository.DeleteAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetForRoom(int id, int? typeId)
        {
            // check user access level

            try
            {
                var result = await _controlRepository.GetForRoomAsync(id, typeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("turn-on/{id}")]
        public async Task<IActionResult> TurnOn(int id)
        {
            // check user access level
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("turn-off/{id}")]
        public async Task<IActionResult> TurnOff(int id)
        {
            // check user access level
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
