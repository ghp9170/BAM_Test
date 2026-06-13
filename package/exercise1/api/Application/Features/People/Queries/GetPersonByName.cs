using Dapper;
using MediatR;
using StargateAPI.Application.Features.Dtos;
using StargateAPI.Domain.Entities;
using StargateAPI.Presentation.Controllers;

namespace StargateAPI.Application.Features.People.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        private readonly StargateContext _context;
        public GetPersonByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByNameResult();

            var query = @"
                        SELECT 
                            p.Id as PersonId, p.Name, 
                            currentDuty.rank as 'CurrentRank',currentDuty.DutyTitle as 'CurrentDutyTitle', details.CareerStartDate, details.CareerEndDate
                        FROM [Person] AS p
                        LEFT JOIN [AstronautDetail] AS details ON details.PersonId = p.Id
                        LEFT JOIN [AstronautDuty] AS currentDuty ON currentDuty.astronautId = details.Id AND currentDuty.isCurrent = 1
                        WHERE @userName = p.Name
                       ";


            var person = await _context.Connection.QueryAsync<PersonAstronaut>(query, new { userName = request.Name });

            result.Person = person.FirstOrDefault();

            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
