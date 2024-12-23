using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Models;

public record CalculateResponse(string Plate, decimal Price, SpotSizeType SpotSize, int TotalParkingSpotHours);
