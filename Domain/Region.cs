using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Domain;

public class Region
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int TotalCapacity { get; set; }
    public decimal HourlyRate { get; set; }
    public VehicleSizeType VehicleSize { get; set; }

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; }
}