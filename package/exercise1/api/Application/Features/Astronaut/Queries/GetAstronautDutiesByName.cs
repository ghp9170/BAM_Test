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

            var result = new GetAstronautDutiesByNameResult();
            var duties = new List<AstronautDuty>();

            var query = @"
                        SELECT 
                            a.Id as PersonId, a.Name, 
                            b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate,
                            c.Id, c.PersonId, c.Rank, c.DutyTitle, c.DutyStartDate, c.DutyEndDate
                        FROM [Person] AS a
                        LEFT JOIN [AstronautDetail] AS b ON b.PersonId = a.Id
                        LEFT JOIN [AstronautDuty] AS c ON c.PersonId = a.Id
                        WHERE a.Name = @userName
                        ORDER BY c.DutyStartDate DESC";

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
