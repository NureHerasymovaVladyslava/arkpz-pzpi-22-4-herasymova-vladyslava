using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // will be accessed from Android, may be modified in the future
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateChatMessage model)
        {
            // check user access level

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
                return BadRequest(ex.Message);
            }
        }

        // will be accessed from Android, may be modified in the future
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // check user access level

            try
            {
                var result = await _chatMessageRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
