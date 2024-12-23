using MediatR;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.Queries.GetAllParkSports;
using ParkingManagement.Domain;
using ParkingManagement.Infrastructure.Repositories;
using ParkingManagement.Infrastructure.UnitOfWorks;

namespace ParkingManagement.Application.Queries.GetAllParkSports;

public class GetAllParkedVehiclesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllParkedVehiclesQuery, List<ParkedVehicleResponse>>
{
    private readonly IRepository<ParkingSpot> _parkingSpotRepository = unitOfWork.Repository<ParkingSpot>();

    public async Task<List<ParkedVehicleResponse>> Handle(GetAllParkedVehiclesQuery request, CancellationToken cancellationToken)
    {
        var parkingSpots = await _parkingSpotRepository
                                 .GetQueryable()
                                 .Include(p => p.Region)
                                 .Include(p => p.Vehicle)
                                 .Where(p => !p.ExitTime.HasValue)
                                 .ToListAsync(cancellationToken);
        
        
        var response = parkingSpots
                       .Select(p => new ParkedVehicleResponse 
                       (
                           VehiclePlate: p.Vehicle?.Plate ?? "N/A",
                           RegionName: p.Region?.Name ?? "Unknown",
                           EntryTime: p.EntryTime
                       )).ToList();

        return response;
    }
}