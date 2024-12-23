using MediatR;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Application.Commands.CheckIn;

public record CheckInCommand(string Plate, VehicleSizeType VehicleSizeType) : IRequest<CheckInCommandResponse>;
