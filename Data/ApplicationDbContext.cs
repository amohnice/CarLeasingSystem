using Microsoft.EntityFrameworkCore;
using CarLeasingSystem.Models;

namespace CarLeasingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        // This constructor passes configuration options (like connection strings) to the base DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(c => c.DailyRate)
                .HasPrecision(18, 2); // This means 18 digits total, 2 after the decimal (perfect for money)
        }
    }
}
