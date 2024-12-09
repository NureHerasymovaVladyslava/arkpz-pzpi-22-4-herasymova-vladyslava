using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
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
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomModel model)
        {
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
        [Authorize(UserRoleManager.RoleAdmin, UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetRoom(int id)
        {
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
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> EditRoom([FromBody] EditRoomModel model)
        {
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
        [Authorize(UserRoleManager.RoleAdmin)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
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
        [Authorize(UserRoleManager.RoleAdmin, UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetAll()
        {
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
