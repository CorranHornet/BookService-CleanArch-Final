using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BookService.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var stopwatch = Stopwatch.StartNew();

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                stopwatch.Stop();

                _logger.LogWarning(
                    "Validation failed for {RequestType} with {FailureCount} errors in {ElapsedMs}ms",
                    typeof(TRequest).Name,
                    failures.Count,
                    stopwatch.ElapsedMilliseconds);

                foreach (var failure in failures)
                {
                    _logger.LogWarning(
                        "Validation error on {Property}: {Error}",
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                throw new ValidationException(failures);
            }

            stopwatch.Stop();

            _logger.LogInformation(
                "Validation succeeded for {RequestType} in {ElapsedMs}ms",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds);

            return await next();
        }
    }
}