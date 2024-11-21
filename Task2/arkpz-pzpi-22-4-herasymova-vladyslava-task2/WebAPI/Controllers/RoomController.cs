using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Room;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly GenericRepository<Room> _roomRepository;

        public RoomController(GenericRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomModel model)
        {
            // check user access level

            var room = new Room();
            room.MapFrom(model);

            try
            {
                var result = await _roomRepository.CreateAsync(room);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            // check user access level

            try
            {
                var result = await _roomRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> EditRoom([FromBody] EditRoomModel model)
        {
            // check user access level

            try
            {
                var room = await _roomRepository.GetByIdAsync(model.Id);
                if (room == null)
                {
                    return NotFound();
                }

                room.MapFrom(model);
                var result = await _roomRepository.UpdateAsync(room);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // room should not contain any sensors or controls
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            // check user access level

            try
            {
                var result = await _roomRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // check user access level

            try
            {
                var result = await _roomRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
