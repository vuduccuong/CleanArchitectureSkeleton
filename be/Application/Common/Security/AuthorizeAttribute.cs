using Domain.Enums.Users;
using System;
using System.Linq;

namespace Application.Common.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class AuthorizeAttribute : Attribute
    {
        public string Roles { get; set; }
        public string Policy { get; set; }

        public AuthorizeAttribute(params ERole[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException(null, nameof(roles));

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}