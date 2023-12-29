using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Context
{
    public class NotesContext : IdentityDbContext<User>
    {
        public NotesContext(DbContextOptions options) : base(options)
        {
        }
    }
}
