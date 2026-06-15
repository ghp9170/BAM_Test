
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Application.Features.Astronaut.Commands;
using StargateAPI.Application.Features.Astronaut.Queries;
using StargateAPI.Application.Features.People.Queries;
using System.Net;

namespace StargateAPI.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDetailController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AstronautDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }



        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDetail([FromBody] CreateAstronautDetail request)
        {
            var result = await _mediator.Send(new CreateAstronautDetail()
            {
                CareerStartDate = request.CareerStartDate,
                CurrentDutyTitle = request.CurrentDutyTitle,
                CurrentRank = request.CurrentRank,
                Name = request.Name
            });
            return this.GetResponse(result);
        }
    }
}