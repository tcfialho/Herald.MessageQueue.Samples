using Accounts.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Threading.Tasks;

namespace Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Create([FromBody]CreateAccountCommand onboardCommand)
        {
            var commandResult = await _mediator.Send(onboardCommand);

            return Accepted();
        }
    }
}