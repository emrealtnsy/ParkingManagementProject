using ParkingManagement.Application.Models;
using ParkingManagement.Domain;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Interfaces;

public interface IParkingHelper
{
    int GetVehicleTotalParkingSpotHours(ParkingSpot parkingSpot);
    bool IsRegionAvailable(Region? region, VehicleSizeType vehicleSizeType);
    IEnumerable<VehicleSizeType> GetVehicleSizeHierarchy(VehicleSizeType vehicleSizeType);
    SpotSizeType GetSpotSize(VehicleSizeType vehicleSizeType, VehicleSizeType availableRegionVehicleSizeType);
    void ValidatePlate(string plate);
    CalculateResponse CalculatePrice(ParkingSpot parkingSpot);
}