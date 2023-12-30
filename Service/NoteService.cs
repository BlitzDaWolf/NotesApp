using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service
{
    public class NoteService : INoteService
    {
        readonly NotesContext context;

        public NoteService(NotesContext context)
        {
            this.context = context;
        }

        public User GetUser(string email)
        {
            using var userActivity = Instrumentation.GetActivitySource<NoteService>().StartActivity("getting user");
            userActivity!.AddTag("user-email", email);
            return context.Users.FirstOrDefault(x => x.Email == email)!;
        }
        public async Task<User> GetUserAsync(string email)
        {
            using var userActivity = Instrumentation.GetActivitySource<NoteService>().StartActivity("getting user async");
            userActivity!.AddTag("user-email", email);
            return (await context.Users.FirstOrDefaultAsync(x => x.Email == email))!;
        }

        public Guid CreateNote(string? name, string noteName, string noteDescription)
        {
            using var createActivity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Creating note");
            var userId = GetUser(name).Id;
            var note = new Note {
                IsPublic = true,
                Name = noteName,
                UserId = userId,
                Description = noteDescription
            };
            context.Notes.Add(note);
            using var saveActivity = Instrumentation.GetActivitySource<NoteService>().StartActivity("saving note");
            context.SaveChanges();
            saveActivity.AddEvent(new System.Diagnostics.ActivityEvent("Note saved"));
            return note.Id;
        }

        public IEnumerable<Note> GetAllNotes()
        {
            using var activity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Retriving notes");
            return context.Notes;
        }

        public IEnumerable<Note> GetMyNotes(string? name)
        {
            using var activity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Retriving my notes");
            var userId = GetUser(name).Id;
            return GetAllNotes().Where(x => x.UserId == userId);
        }

        public IEnumerable<Note> GetPublicNotes()
        {
            using var activity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Retriving public notes");
            return GetAllNotes().Where(x => x.IsPublic);
        }


        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            using var activity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Retriving notes async");
            return await context.Notes.ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetMyNotesAsync(string? name)
        {
            using var activity = Instrumentation.GetActivitySource<NoteService>().StartActivity("Retriving my notes async");
            var userId = (await GetUserAsync(name)).Id;

            if (Random.Shared.Next(100) > 70)
            {
                await Task.Delay(Random.Shared.Next(1000, 8000));
            }
            if (Random.Shared.Next(100) > 95)
            {
                throw new Exception();
            }

            return (await GetAllNotesAsync()).Where(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Note>> GetPublicNotesAsync()
        {
            return (await GetAllNotesAsync()).Where(x => x.IsPublic);
        }
    }
}
