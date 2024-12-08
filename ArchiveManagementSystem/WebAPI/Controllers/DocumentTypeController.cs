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
    public class DocumentTypeController : ControllerBase
    {
        private readonly GenericRepository<DocumentType> _documentTypeRepository;

        public DocumentTypeController(GenericRepository<DocumentType> documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        [HttpPost("create")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> CreateType([FromBody] string name)
        {
            var type = new DocumentType();
            type.Name = name;

            try
            {
                var result = await _documentTypeRepository.CreateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> EditType([FromBody] EditDocumentTypeModel model)
        {
            try
            {
                var type = await _documentTypeRepository.GetByIdAsync(model.Id);
                if (type == null)
                {
                    return NotFound();
                }

                type.MapFrom(model);
                var result = await _documentTypeRepository.UpdateAsync(type);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(UserRoleManager.RoleManager)]
        public async Task<IActionResult> DeleteType(int id)
        {
            try
            {
                var result = await _documentTypeRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // can be accessed from android
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _documentTypeRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
