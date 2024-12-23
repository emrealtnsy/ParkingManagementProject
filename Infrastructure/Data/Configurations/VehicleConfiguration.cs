using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Property(v => v.Plate)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasMany(pf => pf.ParkingSpots)
            .WithOne(pf=> pf.Vehicle)
            .HasForeignKey(pf => pf.VehicleId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}