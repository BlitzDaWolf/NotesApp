namespace Database.Entities
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public bool IsPublic { get; set; }
        public string Text { get; set; } = "";

        public string CurrentHash { get; set; }
        public List<ChangeLog> Logs { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        List<UserNote> UserConections { get; set; }
    }
}
