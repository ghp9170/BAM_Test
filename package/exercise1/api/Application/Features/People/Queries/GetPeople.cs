using Dapper;
using MediatR;
using StargateAPI.Application.Features.Dtos;
using StargateAPI.Domain.Entities;
using StargateAPI.Presentation.Controllers;

namespace StargateAPI.Application.Features.People.Queries
{
    public class GetPeople : IRequest<GetPeopleResult>
    {

    }

    public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
    {
        public readonly StargateContext _context;
        public GetPeopleHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
        {
            var result = new GetPeopleResult();

            var query = @"
                        SELECT 
                            p.Id as PersonId, p.Name, 
                            currentDuty.rank as 'CurrentRank',currentDuty.DutyTitle as 'CurrentDutyTitle', details.CareerStartDate, details.CareerEndDate
                        FROM [Person] AS p
                        LEFT JOIN [AstronautDetail] AS details ON details.PersonId = p.Id
                        LEFT JOIN [AstronautDuty] AS currentDuty ON currentDuty.astronautId = details.Id AND currentDuty.isCurrent = 1
                       ";

            var people = await _context.Connection.QueryAsync<PersonAstronaut>(query);


            result.People = people.ToList();

            return result;
        }
    }

    public class GetPeopleResult : BaseResponse
    {
        public List<PersonAstronaut> People { get; set; } = new List<PersonAstronaut> { };

    }
}
