using CsvHelper.Expressions;
using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Collections;
using System.Security.Claims;

namespace NotesApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        readonly INoteService noteService;

        public NotesController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> Index()
        {
            var u = User;
            // Retrive my notes
            IEnumerable<Note> myNotes = await noteService.GetMyNotesAsync(u.Identity!.Name);
            return Ok(myNotes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Note>> Details(Guid id)
        {
            var u = User;
            Note n = await noteService.GetNoteAsync(id, u.Identity!.Name);
            return Ok(n);
        }

        [HttpGet("public")]
        public ActionResult<IEnumerable<Note>> PublicNotes()
        {
            IEnumerable<Note> notes = noteService.GetPublicNotes();
            return Ok(notes);
        }

        [HttpPost("create")]
        public IActionResult CreateNote(CreateNote noteCreate)
        {
            var u = User;
            Guid id = noteService.CreateNote(u.Identity!.Name, noteCreate.noteName, noteCreate.noteDescription);
            return Created("api/[controller]/", id);
        }

        [HttpPost("edit")]
        public IActionResult EditNote()
        {
            var u = User;
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public IActionResult AddUser(Guid id)
        {
            var u = User;
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult RemoveUser(Guid id)
        {
            return Ok();
        }
    }

    public record CreateNote(string noteName, string noteDescription)
    {

    }
}
