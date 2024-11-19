﻿using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateStatus([FromBody] string name)
        {
            // check user access level

            var status = new DocumentStatus();
            status.Name = name;

            try
            {
                var result = await _documentStatusRepository.CreateAsync(status);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditStatus([FromBody] EditDocumentStatusModel model)
        {
            // check user access level

            try
            {
                var status = await _documentStatusRepository.GetByIdAsync(model.Id);
                if (status == null)
                {
                    return NotFound();
                }

                status.MapFrom(model);
                var result = await _documentStatusRepository.UpdateAsync(status);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            // check user access level

            try
            {
                var result = await _documentStatusRepository.DeleteAsync(id);

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
                var result = await _documentStatusRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
