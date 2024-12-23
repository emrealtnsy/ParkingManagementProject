using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Domain;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public VehicleSizeType VehicleSize { get; set; }

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; }
}