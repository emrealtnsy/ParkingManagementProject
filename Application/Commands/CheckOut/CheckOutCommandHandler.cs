using MediatR;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Domain;
using ParkingManagement.Infrastructure.Repositories;
using ParkingManagement.Infrastructure.UnitOfWorks;

namespace ParkingManagement.Application.Commands.CheckOut;

public class CheckOutCommandHandler(IUnitOfWork unitOfWork, IParkingHelper parkingHelper) : IRequestHandler<CheckOutCommand,CalculatePriceResponse>
{
    private readonly IRepository<ParkingSpot> _parkingSpotRepository = unitOfWork.Repository<ParkingSpot>();
    private readonly IRepository<ParkingFee> _parkingFeeRepository = unitOfWork.Repository<ParkingFee>();

    public async Task<CalculatePriceResponse> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        parkingHelper.ValidatePlate(request.Plate);
    
        var parkingSpot = await _parkingSpotRepository
                                .GetQueryable()
                                .Include(p => p.Region)
                                .Include(p => p.Vehicle)
                                .FirstOrDefaultAsync(p => p.Vehicle!.Plate == request.Plate && !p.ExitTime.HasValue, cancellationToken: cancellationToken)
                          ?? throw new Exception($"{nameof(Vehicle.Plate)} is null!");

        parkingSpot.ExitTime = DateTime.UtcNow;

        var vehicleParkingPrice = parkingHelper.CalculatePrice(parkingSpot);

        await _parkingFeeRepository.AddAsync(new ParkingFee
        {
            TotalFee = vehicleParkingPrice.Price,
            ParkingSpotId = parkingSpot.Id
        });

        await unitOfWork.SaveAsync();

        return new CalculatePriceResponse(Plate: vehicleParkingPrice.Plate,
                                          SpotSize: vehicleParkingPrice.SpotSize,
                                          Price: vehicleParkingPrice.Price, 
                                          TotalParkingSpotHours: vehicleParkingPrice.TotalParkingSpotHours);
    }
}
