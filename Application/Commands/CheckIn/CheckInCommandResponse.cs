using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Commands.CheckIn;

public record CheckInCommandResponse(Guid RegionId, string RegionName, VehicleSizeType RegionVehicleSize);
