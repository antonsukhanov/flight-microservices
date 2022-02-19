using Microsoft.EntityFrameworkCore;

namespace FlightMicroservices.Consumer.Models.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Flight> Flights { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flight>()
            .HasIndex(e => e.FlightNumber)
            .IsUnique();
    }
}