using Five9.CodingAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Five9.CodingAssessment.Data
{
    public class CallCenterContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Queue> Queues { get; set; }

        public CallCenterContext(DbContextOptions<CallCenterContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>().HasMany(a => a.Queues);
        }
    }
}
