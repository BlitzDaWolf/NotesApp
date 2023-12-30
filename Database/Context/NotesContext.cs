using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class NotesContext : IdentityDbContext<User>
    {
        public NotesContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
    }
}
