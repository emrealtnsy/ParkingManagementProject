using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Domain;

public class ParkingSpot
{
    public Guid Id { get; set; }
    public SpotSizeType Size { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public Guid RegionId { get; set; }
    public virtual Region? Region { get; set; }
    public Guid VehicleId { get; set; }
    public virtual Vehicle? Vehicle { get; set; }
}