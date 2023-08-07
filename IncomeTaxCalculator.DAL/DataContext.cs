using IncomeTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.DAL
{
    public class DataContext : DbContext
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<TaxBand> TaxBands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxBand>()
                .HasKey(r => r.Id);
        }
    }
}
