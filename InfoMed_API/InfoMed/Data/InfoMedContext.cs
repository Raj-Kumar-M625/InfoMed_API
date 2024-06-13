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

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventVersions> EventVersions { get; set; }
        public DbSet<EventsMaster> EventsMaster { get; set; }
        public DbSet<TextContentAreas> TextContentAreas { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<ScheduleMaster> ScheduleMaster { get; set; }
        public DbSet<ScheduleDetails> ScheduleDetails { get; set; }
        public DbSet<SponserType> SponserType { get; set; }
        public DbSet<Sponsers> Sponsors { get; set; }
        public DbSet<Speakers> Speakers { get; set; }
        public DbSet<ConferenceFees> ConferenceFees { get; set; }
        public DbSet<LastYearMemory> LastYearMemories { get; set; }
        public DbSet<LastYearMemoryDetail> LastYearMemoryDetails { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
    }
}
