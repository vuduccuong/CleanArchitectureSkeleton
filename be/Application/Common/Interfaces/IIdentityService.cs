using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<IEnumerable<string>> GetRoleAsync(string userId);
        Task<string> GetUserNameAsync(string userId);
    }
}
