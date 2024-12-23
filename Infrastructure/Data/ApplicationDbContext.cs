using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain;

namespace ParkingManagement.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Region> Regions { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}