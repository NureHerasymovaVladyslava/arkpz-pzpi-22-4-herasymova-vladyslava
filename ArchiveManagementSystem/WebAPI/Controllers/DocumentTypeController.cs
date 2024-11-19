using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateType([FromBody] string name)
        {
            // check user access level

            var type = new DocumentType();
            type.Name = name;

            try
            {
                var result = await _documentTypeRepository.CreateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditType([FromBody] EditDocumentTypeModel model)
        {
            // check user access level

            try
            {
                var type = await _documentTypeRepository.GetByIdAsync(model.Id);
                if (type == null)
                {
                    return NotFound();
                }

                type.MapFrom(model);
                var result = await _documentTypeRepository.UpdateAsync(type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            // check user access level

            try
            {
                var result = await _documentTypeRepository.DeleteAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // check user access level

            try
            {
                var result = await _documentTypeRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
