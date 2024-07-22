using Application.Commands.User;
using Application.Contracts.Requests.Vehicle;
using Application.Queries;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    public class VehicleController : BaseController
    {
        private readonly IMediator _mediator;

        public VehicleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequest loginUserRequest)
        {
            await _mediator.Send(new AddVehicleCommand(loginUserRequest));

            return Ok();
        }

        [HttpGet("get-all-vehicle")]
        public async Task<IActionResult> GetAllVehicle()
        {
            return Ok(await _mediator.Send(new GetAllVehiclesQuery()));
        }

    }

}
