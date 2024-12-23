using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Commands.CheckOut;

public record CalculatePriceResponse(
        string Plate,
        decimal Price,
        SpotSizeType SpotSize,
        int TotalParkingSpotHours);
