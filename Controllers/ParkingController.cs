using MediatR;
using ParkingManagement.Application.Commands.CheckIn;
using ParkingManagement.Application.Commands.CheckOut;
using ParkingManagement.Application.Queries.GetAllParkSports;

namespace ParkingManagement.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ParkingController(IMediator mediator) : ControllerBase
{
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInCommand model)
    {
        var result = await mediator.Send(model);

        return Ok(result);
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutCommand model)
    {
        var result = await mediator.Send(model);

        return Ok(result);
    }
    
    [HttpGet("parked-vehicles")]
    public async Task<IActionResult> GetParkedSpots(CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetAllParkedVehiclesQuery(), cancellationToken);

            if (result == null || !result.Any())
            {
                return NotFound("No active parking spots found.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}

