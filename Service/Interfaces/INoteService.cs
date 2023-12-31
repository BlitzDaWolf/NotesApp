﻿using Database.Entities;
using System.Security.Claims;

namespace Service.Interfaces
{
    public interface INoteService
    {
        Guid CreateNote(string? name, string noteName, string noteDescription);
        IEnumerable<Note> GetAllNotes();
        IEnumerable<Note> GetMyNotes(string? name);
        IEnumerable<Note> GetPublicNotes();


        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<IEnumerable<Note>> GetMyNotesAsync(string? name);
        Task<IEnumerable<Note>> GetPublicNotesAsync();
        Task<Note> GetNoteAsync(Guid id, string? email);
        Task<Note> EditNoteAsync(Guid noteId, string newText, string? name);
        Task<Guid> CreateNoteAsync(string? name, string noteName, string noteDescription);
        Task AddUserAsync(Guid id, string userId, string? email);
        Task RemoveUserAsync(Guid id, string userId, string? email);
    }
}
