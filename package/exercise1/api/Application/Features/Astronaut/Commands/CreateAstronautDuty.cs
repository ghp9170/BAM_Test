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
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>, IStargateCommand
    {
        public required string Name { get; set; }

        public required string Rank { get; set; }

        public required string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
                throw new ApplicationException("Bad Request");

            var astronautExists = await _context.AstronautDetails
                .AnyAsync(a => a.Person.Name == request.Name, cancellationToken);

            if (!astronautExists)
                throw new ApplicationException("Bad Request");
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var sql = @"
                            DECLARE @TargetAstronautId INT;

                            SELECT @TargetAstronautId = a.Id 
                            FROM [AstronautDetail] a
                            INNER JOIN [Person] p ON p.Id = a.PersonId
                            WHERE p.Name = @userName;

                            --part of the rules is to mark their career  end date if they retire. So we are always updating this and if they retire then cool
                            --if not no harm no foul
                            UPDATE [AstronautDetails]
                            SET CareerEndDate = @CareerEndDate
                            WHERE id = @TargetAstronautId
        
                            --end the preivous duty so we can start a new one
                            UPDATE [AstronautDuty]
                            SET DutyEndDate = @PreviousDutyEndDate, 
                                IsCurrentDuty = 0
                            WHERE AstronautId = @TargetAstronautId AND IsCurrentDuty = 1;

                            INSERT INTO [AstronautDuty] (
                                AstronautId, 
                                Rank, 
                                DutyTitle, 
                                DutyStartDate, 
                                DutyEndDate,
                                IsCurrentDuty
                            ) 
                            VALUES (
                                @TargetAstronautId, 
                                @Rank, 
                                @DutyTitle, 
                                @DutyStartDate, 
                                NULL,
                                1    
                            );

                            SELECT CAST(SCOPE_IDENTITY() as int);
    ";

            var newDutyId = await _context.Connection.ExecuteScalarAsync<int>(sql, new
            {
                userName = request.Name,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate,
                PreviousDutyEndDate = DateTime.Now.AddDays(-1),
                CareerEndDate = request.DutyTitle == Duty.Retired.ToString() ? DateTime.Now : (DateTime?)null
            });

            return new CreateAstronautDutyResult()
            {
                Id = newDutyId
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}
