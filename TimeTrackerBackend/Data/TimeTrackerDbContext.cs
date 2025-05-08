using Microsoft.EntityFrameworkCore;
using TimeTrackerBackend.Data.Models;

namespace TimeTrackerBackend.Data
{
    public class TimeTrackerDbContext : DbContext
    {
        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<TimeTask> Tasks { get; set; }
    }
}
