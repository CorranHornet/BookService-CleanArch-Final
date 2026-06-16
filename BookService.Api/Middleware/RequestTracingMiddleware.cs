using System.Diagnostics;

namespace BookService.Api.Middleware
{
    public class RequestTracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTracingMiddleware> _logger;

        public RequestTracingMiddleware(RequestDelegate next, ILogger<RequestTracingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            context.Items["TraceId"] = traceId;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["TraceId"] = traceId,
                ["Path"] = context.Request.Path,
                ["Method"] = context.Request.Method
            }))
            {
                _logger.LogInformation("Incoming request");

                var sw = Stopwatch.StartNew();

                try
                {
                    await _next(context);
                    sw.Stop();

                    _logger.LogInformation(
                        "Request finished in {Elapsed}ms with status {StatusCode}",
                        sw.ElapsedMilliseconds,
                        context.Response.StatusCode);
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _logger.LogError(ex,
                        "Request failed after {Elapsed}ms",
                        sw.ElapsedMilliseconds);

                    throw;
                }
            }
        }
    }
}