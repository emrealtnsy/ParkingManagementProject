using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
{
    public void Configure(EntityTypeBuilder<ParkingSpot> builder)
    {
        builder.ToTable("ParkingSpots");
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(ps => ps.Region)
            .WithMany(r => r.ParkingSpots)
            .HasForeignKey(ps => ps.RegionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}