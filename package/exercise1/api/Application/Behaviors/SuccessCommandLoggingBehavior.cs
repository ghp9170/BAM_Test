using MediatR;
using StargateAPI.Domain.Entities;
using System.Diagnostics;
using System.Text.Json;
using StargateAPI.Application.Features.Interfaces;

namespace StargateAPI.Application.Behaviors
{
    public class SuccessCommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IStargateCommand
    {
        private readonly StargateContext _context;

        public SuccessCommandLoggingBehavior(StargateContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            try
            {
                timer.Start();
                // Let the request pass through to the actual Handler
                return await next();
               
            }
            finally 
            {
                timer.Stop();
                var requestName = typeof(TRequest).Name;
                var serializedParameters = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    WriteIndented = true
                });


                var logEntry = new SuccessCommandLog
                {
                    Parameters = $"[{requestName}] {serializedParameters}",
                    CommandTime = timer.ElapsedMilliseconds
                };


                _context.SuccessCommandLog.Add(logEntry);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}