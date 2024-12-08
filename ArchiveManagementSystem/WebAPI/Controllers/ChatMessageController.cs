using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.ChatMessage;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly GenericRepository<ChatMessage> _chatMessageRepository;

        public ChatMessageController(GenericRepository<ChatMessage> chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }


        // can be accessed from android
        [HttpPost("create")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateChatMessage model)
        {
            var message = new ChatMessage();
            message.MapFrom(model);
            message.Sent = DateTime.Now;

            try
            {
                var result = await _chatMessageRepository.CreateAsync(message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // will be accessed from Android, may be modified in the future
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _chatMessageRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
