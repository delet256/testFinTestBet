using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }
        public DbSet<LogEntryModel> LogEntries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogEntryModel>(entity =>
            {
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
