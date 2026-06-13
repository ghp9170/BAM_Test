using MediatR;
using StargateAPI.Domain.Entities;
using System.Text.Json;

namespace StargateAPI.Application.Behaviors
{
    public class ExceptionLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly StargateContext _context;

        public ExceptionLoggingBehavior(StargateContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // Let the request pass through to the actual Handler
                return await next();
            }
            catch (Exception ex)
            {
                
                var requestName = typeof(TRequest).Name;
                var serializedParameters = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                
                var logEntry = new ExceptionLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace ?? "No Stack Trace",
                    Parameters = $"[{requestName}] {serializedParameters}"
                };

                
                _context.ExceptionLogs.Add(logEntry);
                await _context.SaveChangesAsync(cancellationToken);
                
                throw;
            }
        }
    }
}