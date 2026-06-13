using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Application.Features.Enums;
using StargateAPI.Application.Features.Interfaces;
using StargateAPI.Domain.Entities;
using StargateAPI.Presentation.Controllers;
using System.Net;

namespace StargateAPI.Application.Features.Astronaut.Commands
{
    public class CreateAstronautDetail : IRequest<CreateAstronautResult>, IStargateCommand
    {
        public required string Name { get; set; }
        public required string CurrentRank { get; set; }
        public required string CurrentDutyTitle { get; set; }
        public DateTime CareerStartDate { get; set; }
    }

    public class CreateAstronautPreProcessor : IRequestPreProcessor<CreateAstronautDetail>
    {
        private readonly StargateContext _context;

        public CreateAstronautPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(CreateAstronautDetail request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);
            if (person is null) throw new BadHttpRequestException("Bad Request: Person not found");

            var existingAstronaut = _context.AstronautDetails.AsNoTracking().FirstOrDefault(z => z.PersonId == person.Id);
            if (existingAstronaut is not null) throw new BadHttpRequestException("Bad Request: Person is already an astronaut");

            return Task.CompletedTask;
        }
    }

    // 3. The Handler
    public class CreateAstronautHandler : IRequestHandler<CreateAstronautDetail, CreateAstronautResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<CreateAstronautResult> Handle(CreateAstronautDetail request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name)) throw new ApplicationException("Bad Request");
            var sql = @"
                            INSERT INTO [AstronautDetail] (
                                PersonId, 
                                CurrentRank, 
                                CurrentDutyTitle, 
                                CareerStartDate, 
                                CareerEndDate
                            ) 
                            SELECT 
                                Id,               
                                @CareerStartDate, 
                                @CareerEndDate
                            FROM [Person]
                            WHERE Name = @userName;

                            SELECT CAST(SCOPE_IDENTITY() as int);
";

            // Use int? so we can detect if the insert failed due to a missing person
            var newAstronautId = await _context.Connection.ExecuteScalarAsync<int?>(sql, new
            {
                userName = request.Name,
                CareerStartDate = request.CareerStartDate.Date,
                CareerEndDate = request.CurrentDutyTitle == Duty.Retired.ToString() ? request.CareerStartDate.Date : (DateTime?)null
            });

            // If the name wasn't found, no rows were inserted, and SCOPE_IDENTITY() returns null.
            if (newAstronautId == null)
            {
                throw new ApplicationException("Bad Request: Person not found.");
            }

            return new CreateAstronautResult()
            {
                Id = newAstronautId
            };
        }
    }

    // 4. The Result
    public class CreateAstronautResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}