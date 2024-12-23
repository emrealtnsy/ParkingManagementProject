namespace ParkingManagement.Domain;

public class ParkingFee
{
    public Guid Id { get; set; }
    public decimal TotalFee { get; set; }

    public Guid ParkingSpotId { get; set; }
    public virtual ParkingSpot? ParkingSpot { get; set; }
}
