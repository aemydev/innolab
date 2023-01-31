using LogServiceModels;
using Microsoft.EntityFrameworkCore;

namespace LogService.Repositories
{
    public class LogServiceContext : DbContext
    {
        public DbSet<LogEntry> LogsEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseNpgsql(Startup.Configuration["SqlConnectionString"]);
            
        }
    }
}
