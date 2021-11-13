using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public AuthorizationBehavior(IUserService userService, IIdentityService identityService)
        {
            _userService = userService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //Get Role tại Attribute
            var authorizeAttributes = request?.GetType().GetCustomAttributes<AuthorizeAttribute>();

            //Nếu không có role thì api đó public => next
            if (!authorizeAttributes.Any()) return await next().ConfigureAwait(false);

            //Nếu đăng nhập thì phải có userId
            if (string.IsNullOrEmpty(_userService.UserId)) throw new UnauthorizedAccessException();

            //Check Role
            var authWithRoles = authorizeAttributes.Where(a => !string.IsNullOrEmpty(a.Roles));
            if (authWithRoles.Any())
            {
                foreach (var roles in authWithRoles.Select(a => a.Roles.Split(',')))
                {
                    await CheckRoleBaseAsync(roles).ConfigureAwait(false);
                }
            }

            //Check Policy
            authorizeAttributes.Where(auth => !string.IsNullOrEmpty(auth.Policy)).ToList().ForEach(async policy =>
            await CheckPolicyBaseAsync(policy.Policy).ConfigureAwait(false));
            var authWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrEmpty(a.Policy));
            if (authWithPolicies.Any())
            {
                foreach (var policy in authWithPolicies.Select(p => p.Policy))
                {
                    await CheckPolicyBaseAsync(policy).ConfigureAwait(false);
                }
            }

            return await next().ConfigureAwait(false);
        }

        /// <summary>
        /// Check policy with user in DB
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        private async Task CheckPolicyBaseAsync(string policyName)
        {
            var curentPolicy = await _identityService.AuthorizeAsync(_userService.UserId, policyName).ConfigureAwait(false);
            if (!curentPolicy)
            {
                throw new ForbiddenAccessException();
            }
        }

        /// <summary>
        /// Check role with user in DB
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        private async Task<bool> CheckRoleBaseAsync(string[] roles)
        {
            var userId = _userService.UserId;
            bool auth = false;
            foreach (var role in roles)
            {
                var roleExist = await _identityService.IsInRoleAsync(userId, role).ConfigureAwait(false);
                if (!roleExist) continue;
                auth = true;
                break;
            }
            if (!auth) throw new ForbiddenAccessException();

            return auth;
        }
    }
}
