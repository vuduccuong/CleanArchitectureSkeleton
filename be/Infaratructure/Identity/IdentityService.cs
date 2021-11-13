using Application.Common.Interfaces;
using Domain.Entities.UserManagements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infaratructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClams;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClams,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClams = userClams;
            _authorizationService = authorizationService;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role).ConfigureAwait(false);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            var principal = await _userClams.CreateAsync(user).ConfigureAwait(false);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName).ConfigureAwait(false);

            return result.Succeeded;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId).ConfigureAwait(false);

            return user.UserName;
        }

        public async Task<IEnumerable<string>> GetRoleAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user == null)
                return null;

            return await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        }
    }
}