using Application.Common.Interfaces;
using Domain.Entities.UserManagements;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class TokenService : ITokenService
    {
        private DateTime _expiration;

        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public string CreateToken(ApplicationUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(",", userRoles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var issuer = _config["JWT:ValidIssuer"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = creds,
                Issuer = issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            this._expiration = token.ValidTo;

            return tokenHandler.WriteToken(token);
        }

        public DateTime GetExpirationCurrentToken()
        {
            return _expiration;
        }
    }
}
