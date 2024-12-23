using ParkingManagement.Application.Interfaces;
using ParkingManagement.Application.Models;
using ParkingManagement.Domain;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Helpers;

public class ParkingHelper : IParkingHelper
{
    public int GetVehicleTotalParkingSpotHours(ParkingSpot parkingSpot)
    {
        var exitTime = DateTime.UtcNow - parkingSpot.EntryTime;
        var totalHours = exitTime.Hours;

        if (totalHours <= 0 || exitTime.Minutes > 15)
        {
            totalHours++;
        }

        return totalHours;
    }

    public bool IsRegionAvailable(Region? region, VehicleSizeType vehicleSizeType)
    {
        if (region is null)
        {
            return false;
        }

        var spotSize = GetSpotSize(vehicleSizeType, region.VehicleSize);
        var parkingSpotsCount = region.ParkingSpots.Sum(p => (int)p.Size);

        return region.TotalCapacity - parkingSpotsCount - spotSize >= 0;
    }

    public IEnumerable<VehicleSizeType> GetVehicleSizeHierarchy(VehicleSizeType vehicleSizeType)
        => vehicleSizeType switch
        {
            VehicleSizeType.Small => [VehicleSizeType.Small, VehicleSizeType.Medium, VehicleSizeType.Large],
            VehicleSizeType.Medium => [VehicleSizeType.Medium, VehicleSizeType.Large, VehicleSizeType.Small],
            VehicleSizeType.Large => [VehicleSizeType.Large, VehicleSizeType.Medium, VehicleSizeType.Small],
            _ => throw new ArgumentOutOfRangeException(nameof(vehicleSizeType), vehicleSizeType, null)
        };
    
    public SpotSizeType GetSpotSize(VehicleSizeType vehicleSizeType, VehicleSizeType availableRegionVehicleSizeType)
        => vehicleSizeType switch
        {
            VehicleSizeType.Small => SpotSizeType.OneVehicle,
            VehicleSizeType.Medium => availableRegionVehicleSizeType switch
            {
                VehicleSizeType.Small => SpotSizeType.TwoVehicle,
                VehicleSizeType.Medium or VehicleSizeType.Large => SpotSizeType.OneVehicle
            },
            VehicleSizeType.Large => availableRegionVehicleSizeType switch
            {
                VehicleSizeType.Small => SpotSizeType.ThreeVehicle,
                VehicleSizeType.Medium => SpotSizeType.TwoVehicle,
                VehicleSizeType.Large => SpotSizeType.OneVehicle,
            },
            _ => throw new ArgumentOutOfRangeException(nameof(vehicleSizeType), vehicleSizeType, null)
        };

    public void ValidatePlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
        {
            throw new NullReferenceException($"{nameof(Vehicle.Plate)} is null!");
        }
    }

    public CalculateResponse CalculatePrice(ParkingSpot parkingSpot)
    {
        var totalParkingSpotHours = GetVehicleTotalParkingSpotHours(parkingSpot);
        var parkingUsage = (int)parkingSpot.Size * totalParkingSpotHours;
        var price = parkingUsage * parkingSpot.Region!.HourlyRate;

        return new(parkingSpot.Vehicle!.Plate, price, parkingSpot.Size, totalParkingSpotHours);
    }
}
