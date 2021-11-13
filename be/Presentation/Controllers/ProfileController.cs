using Application.UserManagements.UserProfile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ProfileController:ApiControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult> Profile([FromRoute] string userId)
        {
            var query = new Details { UserId = userId };
            var result = await Mediator.Send(query).ConfigureAwait(false);

            return HandlerResult(result);
        }
    }
}
