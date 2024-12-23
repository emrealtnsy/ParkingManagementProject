using MediatR;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Domain;
using ParkingManagement.Domain.Enums;
using ParkingManagement.Infrastructure.Repositories;
using ParkingManagement.Infrastructure.UnitOfWorks;

namespace ParkingManagement.Application.Commands.CheckIn;

public class CheckInCommandHandler(IUnitOfWork unitOfWork, IParkingHelper parkingHelper) : IRequestHandler<CheckInCommand, CheckInCommandResponse>
{
    private readonly IRepository<Region> _regionRepository = unitOfWork.Repository<Region>();
    private readonly IRepository<ParkingSpot> _parkingSpotRepository = unitOfWork.Repository<ParkingSpot>();
    private readonly IRepository<Vehicle> _vehicleRepository = unitOfWork.Repository<Vehicle>();

    public async Task<CheckInCommandResponse> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        parkingHelper.ValidatePlate(request.Plate);
        await CheckAllocateParkingSpot(request.Plate);
        var region = await GetAvailableRegion(request.VehicleSizeType);
        var vehicle = await GetOrAddVehicle(request.Plate, request.VehicleSizeType);
        var spotSize = parkingHelper.GetSpotSize(request.VehicleSizeType, region.VehicleSize);
        await AllocateParkingSpot(region, vehicle, spotSize);

        return new(region.Id, region.Name, region.VehicleSize);
    }

    private async Task AllocateParkingSpot(Region region, Vehicle vehicle, SpotSizeType spotSizeType)
    {
        ArgumentNullException.ThrowIfNull(region);
        ArgumentNullException.ThrowIfNull(vehicle);
        
        await _parkingSpotRepository.AddAsync(new()
        {
            Size = spotSizeType,
            EntryTime = DateTime.UtcNow,
            RegionId = region.Id,
            VehicleId = vehicle.Id, 
        });
        
        await unitOfWork.SaveAsync();
    }

    private async Task<Region> GetAvailableRegion(VehicleSizeType vehicleSizeType)
    {
        var vehicleSizes = parkingHelper.GetVehicleSizeHierarchy(vehicleSizeType);
        foreach (var vehicleSize in vehicleSizes)
        {
            var region = await _regionRepository.GetQueryable()
                                                .AsNoTracking()
                                                .Include(r => r.ParkingSpots.Where(ps => !ps.ExitTime.HasValue))
                                                .FirstOrDefaultAsync(r => r.VehicleSize == vehicleSize
                                                                          && r.TotalCapacity - r.ParkingSpots.Sum(x => (int)x.Size) > 0);


            if (parkingHelper.IsRegionAvailable(region, vehicleSizeType))
            {
                return region!;
            }
        }

        throw new Exception("No available region found");
    }

    private async Task<Vehicle> GetOrAddVehicle(string plate, VehicleSizeType vehicleSizeType)
    {
        parkingHelper.ValidatePlate(plate);
        var vehicle = await _vehicleRepository.GetQueryable()
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(p => p.Plate == plate);

        if (vehicle is null)
        {
            vehicle = new Vehicle()
            {
                Plate = plate,
                VehicleSize = vehicleSizeType,
            };
            await _vehicleRepository.AddAsync(vehicle);
            await unitOfWork.SaveAsync();
        }

        return vehicle;
    }

    private async Task CheckAllocateParkingSpot(string plate)
    {
        var canAllocate = !await _parkingSpotRepository
                      .GetQueryable()
                      .Include(p => p.Vehicle)
                      .Where(p => !p.ExitTime.HasValue)
                      .AnyAsync(p => p.Vehicle!.Plate == plate);

        if (!canAllocate)
        {
            throw new Exception("The vehicle has already been allocated a parking spot");
        }
    }
}
