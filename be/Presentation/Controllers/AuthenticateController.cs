using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.UserManagements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos.AuthenticateDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [AllowAnonymous]
    public class AuthenticateController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticateController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Login InMemory
        /// </summary>
        /// <param name="loginDto.LoginId">Default: cuongvd7</param>
        /// <param name="loginDto.Password">Default: G7e3KSMED!</param>
        /// <param name="loginDto">Default: lead1</param>
        /// <param name="loginDto">Default: G7e3KSMED!</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.LoginId).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "LoginId or Password are incorrect!"
                });
            }

            var resultLogin = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (resultLogin.IsLockedOut)
            {
                return Ok(new
                {
                    Status = "Account inactive",
                    Message = "Your account has been locked. Try again in 15 minutes."
                });
            }

            if (resultLogin.Succeeded)
            {
                var userInfo = CreateUserObject(user);

                await _userManager.SetAuthenticationTokenAsync(user, "LOGIN_PROVIDER", "TOKEN_NAME", userInfo.Token).ConfigureAwait(false);
                return Ok(userInfo);
            }
            else
            {
                var countLoginFailed = await _userManager.GetAccessFailedCountAsync(user).ConfigureAwait(false);
                if (countLoginFailed == 5)
                {
                    await _userManager.AccessFailedAsync(user).ConfigureAwait(false);

                    return Ok(new
                    {
                        Status = "Account inactive",
                        Message = "You inputted the wrong password 5 times in a row. Your account has been locked."
                    });
                }
            }

            return NotFound(new
            {
                Status = "Error",
                Message = "LoginId or Password are incorrect!"
            });
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return Ok();
        }

        private UserDto CreateUserObject(ApplicationUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                Image = "",
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName,
                Expiration = _tokenService.GetExpirationCurrentToken(),
                Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult()
            };
        }
    }
}
