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
        public async Task<IActionResult> CreateNote(CreateNote noteCreate)
        {
            var u = User;
            Guid id = await noteService.CreateNoteAsync(u.Identity!.Name, noteCreate.noteName, noteCreate.noteDescription);
            return Created("api/[controller]/", id);
        }

        [HttpPost("edit")]
        public async Task<ActionResult<Note>> EditNote(EditNote newNote)
        {
            var u = User;
            return Ok(await noteService.EditNoteAsync(newNote.noteId, newNote.newText, u.Identity!.Name));
        }

        [HttpPut("{id:guid}/{userId}")]
        public async Task<IActionResult> AddUser(Guid id, string userId)
        {
            var u = User;
            await noteService.AddUserAsync(id, userId, u.Identity!.Name);
            return Ok();
        }

        [HttpDelete("{id:guid}/{userId}")]
        public async Task<IActionResult> RemoveUser(Guid id, string userId)
        {
            var u = User;
            await noteService.RemoveUserAsync(id, userId, u.Identity!.Name);
            return Ok();
        }
    }

    public record CreateNote(string noteName, string noteDescription);
    public record EditNote(Guid noteId, string newText);
}
