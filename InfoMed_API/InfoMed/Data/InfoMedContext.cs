using InfoMed.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfoMed.Data
{
    public class InfoMedContext : IdentityDbContext
    {
        public InfoMedContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Event> Event { get; set; }
        public DbSet<EventVersions> EventVersions { get; set; }
    }
}
