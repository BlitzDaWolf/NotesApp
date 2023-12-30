namespace Database.Entities
{
    public class ChangeLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid NoteId { get; set; }
        public Note Note { get; set; }

        public string Hash { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
