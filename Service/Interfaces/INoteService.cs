using Database.Entities;

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
    }
}
