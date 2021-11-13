using Application.Common.Contants;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class PerfomanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _watch;
        private readonly ILogger<TRequest> _logger;
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public PerfomanceBehaviour(ILogger<TRequest> logger, IUserService userService, IIdentityService identityService)
        {
            _watch = new Stopwatch();
            _logger = logger;
            _userService = userService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _watch.Start();
            var response = await next().ConfigureAwait(false);
            _watch.Stop();

            var requestTime = _watch.ElapsedMilliseconds;

            if (requestTime <= RequestTimeConstant.MAX_MILISECOND) return response;

            var requestName = typeof(TRequest).Name;
            var userId = _userService.UserId;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(_userService.UserId).ConfigureAwait(false);
            }

            _logger.LogInformation($"Long Running Request {requestName}: {userId} {userName} {request}");

            return response;
        }
    }
}
