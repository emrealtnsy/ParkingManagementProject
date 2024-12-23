using ParkingManagement.Domain;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Regions.Any())
            return;

        // REGION A
        var regionA = new Region
        {
            Name = RegionType.A.ToString(),
            TotalCapacity = 15,
            HourlyRate = 100,
            VehicleSize = VehicleSizeType.Small
        };

        // REGION B
        var regionB = new Region
        {
            Name = RegionType.B.ToString(),
            TotalCapacity = 10,
            HourlyRate = 120,
            VehicleSize = VehicleSizeType.Medium
        };

        // REGION C
        var regionC = new Region
        {
            Name = RegionType.C.ToString(),
            TotalCapacity = 5,
            HourlyRate = 150,
            VehicleSize = VehicleSizeType.Large
        };

        context.Regions.AddRange(regionA, regionB, regionC);
        context.SaveChanges();
    }
}