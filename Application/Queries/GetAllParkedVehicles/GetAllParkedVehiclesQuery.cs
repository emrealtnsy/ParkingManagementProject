using MediatR;
using ParkingManagement.Application.Queries.GetAllParkSports;

namespace ParkingManagement.Application.Queries.GetAllParkSports;

public record GetAllParkedVehiclesQuery : IRequest<List<ParkedVehicleResponse>>;
