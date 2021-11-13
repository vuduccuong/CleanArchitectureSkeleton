using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandlerResult<TResponse>(ResultBase<TResponse> result)
        {
            if (result == null)
                return NotFound();

            if (!result.IsSuccess)
                return BadRequest();

            if (result.Value == null)
                return NotFound();

            return Ok(result.Value);
        }
    }
}
