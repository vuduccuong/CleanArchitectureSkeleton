using Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        public LoggingBehaviour(ILogger<TRequest> logger, IUserService userService, IIdentityService identityService)
        {
            _logger = logger;
            _userService = userService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _userService.UserId;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(_userService.UserId).ConfigureAwait(false);
            }

            _logger.LogInformation($"Log Request {requestName}: {userId} {userName} {request}");
        }
    }
}
