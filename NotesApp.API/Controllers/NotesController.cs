using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NotesApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            var u = User;
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var u = User;
            return Ok();
        }

        [HttpGet("public")]
        public IActionResult PublicNotes()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateNote()
        {
            var u = User;
            return Ok();
        }

        [HttpPost]
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
}
