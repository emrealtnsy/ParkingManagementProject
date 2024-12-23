using MediatR;
using ParkingManagement.Application.Models;

namespace ParkingManagement.Application.Commands.CheckOut;

public record CheckOutCommand(string Plate) : IRequest<CalculatePriceResponse>;