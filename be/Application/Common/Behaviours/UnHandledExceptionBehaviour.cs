using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class UnHandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnHandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, $"UnHandled Request {requestName}: {request}");

                throw;
            }
        }
    }
}
