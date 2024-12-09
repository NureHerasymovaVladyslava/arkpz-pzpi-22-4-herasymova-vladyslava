using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Managers;
using WebAPI.Middlewares;
using WebAPI.Models.Document;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentStatusController : ControllerBase
    {
        private readonly GenericRepository<DocumentStatus> _documentStatusRepository;

        public DocumentStatusController(GenericRepository<DocumentStatus> documentStatusRepository)
        {
            _documentStatusRepository = documentStatusRepository;
        }

        [HttpPost("create")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> CreateStatus([FromBody] string name)
        {
            var status = new DocumentStatus();
            status.Name = name;

            try
            {
                var result = await _documentStatusRepository.CreateAsync(status);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> EditStatus([FromBody] EditDocumentStatusModel model)
        {
            try
            {
                var status = await _documentStatusRepository.GetByIdAsync(model.Id);
                if (status == null)
                {
                    return NotFound();
                }

                status.MapFrom(model);
                var result = await _documentStatusRepository.UpdateAsync(status);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            try
            {
                var result = await _documentStatusRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _documentStatusRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
