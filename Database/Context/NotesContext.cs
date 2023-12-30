using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class NotesContext : IdentityDbContext<User>
    {
        public NotesContext(DbContextOptions options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
    }
}
