using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class ParkingFeeConfiguration : IEntityTypeConfiguration<ParkingFee>
{
    public void Configure(EntityTypeBuilder<ParkingFee> builder)
    {
        builder.ToTable("ParkingFees");
        builder.HasKey(pf => pf.Id);
        
        builder.Property(pf => pf.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(pf => pf.ParkingSpot)
            .WithOne()
            .HasForeignKey<ParkingFee>(pf => pf.ParkingSpotId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}