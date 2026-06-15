using Dapper;
using MediatR;
using StargateAPI.Application.Features.Dtos;
using StargateAPI.Domain.Entities;
using StargateAPI.Presentation.Controllers;

namespace StargateAPI.Application.Features.Astronaut.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;

        public GetAstronautDutiesByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name)) throw new ApplicationException("Bad Request");

            var result = new GetAstronautDutiesByNameResult();
            var duties = new List<AstronautDuty>();

            var query = @"
                        SELECT 
                            p.Id as PersonId, p.Name, 
                            currentDuty.rank as 'CurrentRank',currentDuty.DutyTitle as 'CurrentDutyTitle', details.CareerStartDate, details.CareerEndDate,
                            pastDuties.Id, pastDuties.PersonId, pastDuties.Rank, pastDuties.DutyTitle, pastDuties.DutyStartDate, pastDuties.DutyEndDate
                        FROM [Person] AS p
                        LEFT JOIN [AstronautDetail] AS details ON details.PersonId = p.Id
                        LEFT JOIN [AstronautDuty] AS pastDuties ON pastDuties.astronautId = details.Id AND pastDuties.isCurrent = 0
                        LEFT JOIN [AstronautDuty] AS currentDuty ON currentDuty.astronautId = details.Id AND currentDuty.isCurrent = 1
                        WHERE p.Name = @userName
                        ORDER BY pastDuties.DutyStartDate DESC";

            await _context.Connection.QueryAsync<PersonAstronaut, AstronautDuty, PersonAstronaut>(
                query,
                (person, duty) =>
                {
                    result.Person = person;

                    if (duty != null)
                    {
                        duties.Add(duty);
                    }

                    return person; 
                },
                new { userName = request.Name },
                splitOn: "Id" 
            );

            result.AstronautDuties = duties;

            return result;

        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
