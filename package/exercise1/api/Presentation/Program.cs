using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Application.Behaviors;
using StargateAPI.Application.Features.Astronaut.Commands;
using StargateAPI.Domain.Entities;
using System.Windows.Input;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StargateContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    //## Rules
    //A Person is uniquely identified by their Name.
    //For rule 1 I chose not to include a PreProcessor to verify the person exists.
    //Its faster to try and create the person and deal with the DB error, then it is to fetch the person and see if that person exists
    //If we were to expan the person to be multiple tables with children tables, the above statement would be more so true. 
    //If the requirement for uniquely Identifiing a person by their name changes, then we will add a preProcesser to handle that change

    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionLoggingBehavior<,>));
    //the below logs all DML commands and their times. We will also want to wire up timing for gets, but that will need 
    //to be down through telementry and azure telemetry logging or something special as that will crash the app if just wired up 
    //here by itself
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(SuccessCommandLoggingBehavior<,>));
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

//Since this is supposed to be a standalone project. We will spin up and 
//apply all changes to the DB for whomever runs this. Saves them time and effort themselves and makes it turnkey
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StargateContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


