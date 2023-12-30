namespace Database.Entities
{
    public class UserNote
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }
        public Guid NoteId { get; set; }
        public bool CanWrite { get; set; } = false;

        public User User { get; set; }
        public Note Note { get; set; }
    }
}
