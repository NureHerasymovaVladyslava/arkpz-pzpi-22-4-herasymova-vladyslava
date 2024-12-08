using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Document;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentLogController : ControllerBase
    {
        private readonly DocumentLogRepository _documentLogRepository;
        private readonly GenericRepository<Document> _documentRepository;

        public DocumentLogController(DocumentLogRepository documentLogRepository, 
            GenericRepository<Document> documentRepository)
        {
            _documentLogRepository = documentLogRepository;
            _documentRepository = documentRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLog([FromBody] EditDocumentModel model)
        {
            var docLog = new DocumentLog()
            {
                DocumentId = model.Id,
                LogTime = DateTime.Now,
                UserId = model.UserId
            };

            try
            {
                var document = await _documentRepository.GetByIdAsync(docLog.DocumentId);
                if (document == null)
                {
                    return NotFound();
                }

                docLog.NewName = document.Name != model.Name 
                    ? model.Name : null;

                docLog.NewRoomId = document.RoomId != model.RoomId
                    ? model.RoomId : null;

                docLog.NewStatusId = document.StatusId != model.StatusId
                    ? model.StatusId : null;

                docLog.NewTypeId = document.TypeId != model.TypeId
                    ? model.TypeId : null;

                docLog.NewAdditionalInfo = document.AdditionalInfo 
                    != model.AdditionalInfo ? model.AdditionalInfo : null;

                var result = await _documentLogRepository.CreateAsync(docLog);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("document/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetForDocument(int id)
        {
            try
            {
                var result = await _documentLogRepository.GetForDocumentAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("requests")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var result = await _documentLogRepository.GetUnprocessedAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("confirm/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> ConfirmRequest(int id)
        {
            try
            {
                var docLog = await _documentLogRepository.GetByIdAsync(id);
                if (docLog == null)
                {
                    return NotFound();
                }

                // verification

                docLog.Approved = true;
                var result = await _documentLogRepository.UpdateAsync(docLog);

                if (!result)
                {
                    return BadRequest();
                }

                var document = await _documentRepository.GetByIdAsync(docLog.DocumentId);

                if (document == null)
                {
                    return NotFound();
                }

                document.Name = docLog.NewName ?? document.Name;
                document.RoomId = docLog.NewRoomId ?? document.RoomId;
                document.StatusId = docLog.NewStatusId ?? document.StatusId;
                document.TypeId = docLog.NewTypeId ?? document.TypeId;
                document.AdditionalInfo = docLog.NewAdditionalInfo ?? document.AdditionalInfo;

                var docResult = await _documentRepository.UpdateAsync(document);

                return docResult ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("discard/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> DiscardRequest(int id)
        {
            try
            {
                var dockLog = await _documentLogRepository.GetByIdAsync(id);
                if (dockLog == null)
                {
                    return NotFound();
                }

                // verification

                dockLog.Approved = false;
                var result = await _documentLogRepository.UpdateAsync(dockLog);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
