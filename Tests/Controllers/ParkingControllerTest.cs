using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Controllers;
using ParkingManagement.Application.Commands.CheckIn;
using ParkingManagement.Application.Commands.CheckOut;
using ParkingManagement.Application.Queries.GetAllParkSports;
using ParkingManagement.Domain.Enums;

namespace ParkingManagement.Tests.Controllers;

public class ParkingControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ParkingController _controller;

    public ParkingControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ParkingController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CheckIn_ShouldReturnOk_WithExpectedResponse()
    {
        // Arrange
        const string plate = "34ABC34";
        const VehicleSizeType vehicleSizeType = VehicleSizeType.Small;

        var command = new CheckInCommand(plate, vehicleSizeType);

        var expectedResponse = new CheckInCommandResponse(
                                                          RegionId: Guid.NewGuid(),
                                                          RegionName: RegionType.A.ToString(),
                                                          RegionVehicleSize: VehicleSizeType.Small
                                                         );

        _mediatorMock
                .Setup(m => m.Send(It.IsAny<CheckInCommand>(), 
                                   It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.CheckIn(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<CheckInCommandResponse>(okResult.Value);

        Assert.Equal(expectedResponse.RegionId, actualResponse.RegionId);
        Assert.Equal(expectedResponse.RegionName, actualResponse.RegionName);
        Assert.Equal(expectedResponse.RegionVehicleSize, actualResponse.RegionVehicleSize);

        _mediatorMock
                .Verify(m => m.Send(It.Is<CheckInCommand>(cmd => cmd.Plate == plate
                                                                 && cmd.VehicleSizeType == vehicleSizeType),
                                         It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task CheckOut_ShouldReturnOk_WithExpectedResponse()
    {
        // Arrange
        var command = new CheckOutCommand(Plate: "34ABC123");

        var expectedResponse = new CalculatePriceResponse(Plate: "34ABC123",
                                                          SpotSize: SpotSizeType.OneVehicle,
                                                          Price: 50.0m,
                                                          TotalParkingSpotHours: 5);

        _mediatorMock
                .Setup(m => m.Send(It.IsAny<CheckOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.CheckOut(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<CalculatePriceResponse>(okResult.Value);

        Assert.Equal(expectedResponse.Plate, actualResponse.Plate);
        Assert.Equal(expectedResponse.SpotSize, actualResponse.SpotSize);
        Assert.Equal(expectedResponse.Price, actualResponse.Price);
        Assert.Equal(expectedResponse.TotalParkingSpotHours, actualResponse.TotalParkingSpotHours);

        _mediatorMock.Verify(m => m.Send(It.Is<CheckOutCommand>(cmd => cmd.Plate == "34ABC123"),
                                         It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task GetParkedSpots_ShouldReturnOk_WithAllFieldsValidated()
    {
        // Arrange
        var expectedResponse = new List<ParkedVehicleResponse>
        {
            new ParkedVehicleResponse(VehiclePlate: "34ABC123",
                                    RegionName: "Region A",
                                    EntryTime: DateTime.Now.AddHours(-1)),
            
            new ParkedVehicleResponse(VehiclePlate: "35XYZ789",
                                    RegionName: "Region B",
                                    EntryTime: DateTime.Now.AddHours(-2))
        };

        _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllParkedVehiclesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetParkedSpots(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<List<ParkedVehicleResponse>>(okResult.Value);

        Assert.Equal(expectedResponse.Count, actualResponse.Count);

        for (int i = 0; i < expectedResponse.Count; i++)
        {
            Assert.Equal(expectedResponse[i].VehiclePlate, actualResponse[i].VehiclePlate);
            Assert.Equal(expectedResponse[i].RegionName, actualResponse[i].RegionName);
            Assert.Equal(expectedResponse[i].EntryTime, actualResponse[i].EntryTime);
        }

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllParkedVehiclesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
