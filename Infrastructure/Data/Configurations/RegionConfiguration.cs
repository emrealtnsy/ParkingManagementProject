using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(r => r.ParkingSpots)
            .WithOne(ps => ps.Region)
            .HasForeignKey(ps => ps.RegionId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}