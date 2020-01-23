using Microsoft.EntityFrameworkCore;
using SecData.Models.Db;

namespace SecData
{
    public class FilingsDbContext : DbContext
    {
        public FilingsDbContext(DbContextOptions<FilingsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Fund> Funds { get; set; }
        public DbSet<Filing> Filings { get; set; }
    }
}
