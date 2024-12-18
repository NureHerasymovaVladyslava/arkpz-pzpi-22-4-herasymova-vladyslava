﻿using Core.Helpers;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Document;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly GenericRepository<Document> _documentRepository;
        private readonly DocumentLogRepository _documentLogRepository;

        public DocumentController(GenericRepository<Document> documentRepository, DocumentLogRepository documentLogRepository)
        {
            _documentRepository = documentRepository;
            _documentLogRepository = documentLogRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentModel model)
        {
            // check user access level

            var document = new Document();
            document.MapFrom(model);
            document.Added = DateTime.Now;

            try
            {
                var result = await _documentRepository.CreateAsync(document);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            // check user access level

            try
            {
                var result = await _documentRepository.DeleteAsync(id);

                return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() //TODO: Add sordting and filtering
        {
            // check user access level

            try
            {
                var result = await _documentRepository.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditDocument([FromBody] EditDocumentModel model)
        {
            // check user access level

            var docLog = new DocumentLog()
            {
                DocumentId = model.Id,
                LogTime = DateTime.Now,
                UserId = model.UserId,
                Approved = true
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

                document.MapFrom(model);

                var docResult = await _documentRepository.UpdateAsync(document);

                return docResult ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
