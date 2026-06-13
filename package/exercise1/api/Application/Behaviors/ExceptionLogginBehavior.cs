using MediatR;
using StargateAPI.Domain.Entities;
using System.Text.Json;
// Add your using statements for StargateContext and ExceptionLog

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
                return await next();
            }
            catch (Exception ex)
            {
                // 1. We caught an error! Grab the parameters by serializing the TRequest object
                var requestName = typeof(TRequest).Name;
                var serializedParameters = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // 2. Create the log entry
                var logEntry = new ExceptionLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace ?? "No Stack Trace",
                    // We prepend the request name so you know exactly which command failed
                    Parameters = $"[{requestName}] {serializedParameters}"
                };

                // 3. Save it to the database
                _context.ExceptionLogs.Add(logEntry);
                await _context.SaveChangesAsync(cancellationToken);

                // 4. Rethrow the error so the Application doesn't swallow it
                throw;
            }
        }
    }
}