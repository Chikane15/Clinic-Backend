using Microsoft.EntityFrameworkCore;

namespace Assignment_05_03.Models
{
    public class LoggerDbContext:DbContext
    {
        public DbSet<Logger> Loggers { get; set; } = null!;
        public DbSet<ErrorLogger> ErrorLoggers { get; set; } = null!;

        public LoggerDbContext()
        {

        }
        // DI Resolve
        public LoggerDbContext(DbContextOptions<LoggerDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
        }
    }
}
