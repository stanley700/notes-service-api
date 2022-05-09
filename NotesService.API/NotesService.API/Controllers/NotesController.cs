using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesService.API.Models;
using NotesService.Core;
using NotesService.Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NotesService.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : BaseController
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] AddNoteModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            var note = await _noteService.Add(new Core.Data.Entities.Note(model.Id, model.Tile?.Trim(), model.Body, model.Tags));


            return Ok(new { note.Id, note.Body, note.Title, note.Tags });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] AddNoteModel model)
        {
            if(string.IsNullOrEmpty(model.Id))
            {
                return BadRequest(new { responseCode = SystemCodes.InvalidRequest, responseDescription = "Note identifier is missing"});
            }

            var note = await _noteService.Update(new Core.Data.Entities.Note(model.Id, model.Tile?.Trim(), model.Body, model.Tags));


            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", note.Id, note.Body, note.Title, note.Tags });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _noteService.Delete(id);

            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful" });
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var result = await _noteService.GetAll(UserIdFromToken);

            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }

        [HttpGet("filter/{tags}")]
        public async Task<IActionResult> GetByTags(string tags)
        {
            var result = await _noteService.Find(tags.Split(','));

            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }


        [HttpGet("search/{keywords}")]
        public async Task<IActionResult> GetByKeywords(string keywords)
        {
            var result = await _noteService.Search(keywords);

            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _noteService.GetAll(id);

            return Ok(new { responseCode = SystemCodes.Successful, responseDescription = "Successful", data = result });
        }
    }
}
