using System;
using System.Collections.Generic;

namespace Presentation.Dtos.AuthenticateDto
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public DateTime Expiration { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
