using Domain.Entities.UserManagements;
using System;

namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUser user);
        public DateTime GetExpirationCurrentToken();
    }
}
