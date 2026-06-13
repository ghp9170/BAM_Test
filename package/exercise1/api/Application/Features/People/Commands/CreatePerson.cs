using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Application.Features.Interfaces;
using StargateAPI.Domain.Entities;
using StargateAPI.Presentation.Controllers;
using System.Windows.Input;

namespace StargateAPI.Application.Features.People.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>, IStargateCommand
    {
        public required string Name { get; set; } = string.Empty;
    }


    //I am purposly not wiring up this prepocesser. Dropping an Unique Index at the DB level, will be "faster" 
    //than checking if a person exists with the same name. If we need to run more indepth validation then 
    //we can wire up the preProcessor. Leaving it here incase we want it in the future or someone wonders
    //why I am not using it.
    //public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    //{
    //    private readonly StargateContext _context;
    //    public CreatePersonPreProcessor(StargateContext context)
    //    {
    //        _context = context;
    //    }
    //    public Task Process(CreatePerson request, CancellationToken cancellationToken)
    //    {
    //        var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

    //        if (person is not null) throw new BadHttpRequestException("Bad Request");

    //        return Task.CompletedTask;
    //    }
    //}

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;

        public CreatePersonHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name)) throw new ApplicationException("Bad Request");

            var newPerson = new Person()
                {
                   Name = request.Name
                };

                await _context.People.AddAsync(newPerson);

                await _context.SaveChangesAsync();

                return new CreatePersonResult()
                {
                    Id = newPerson.Id
                };
          
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
