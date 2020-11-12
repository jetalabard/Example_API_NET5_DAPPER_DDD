using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserAccess.Application.Queries;
using UserAccess.Application.Users.Commands;
using UserAccess.Application.Users.Outputs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("role")]
        public async Task<RoleOutput> Role()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            return await _mediator.Send(new GetRoleQuery(userEmail));
        }

        [HttpGet("all")]
        public async Task<IEnumerable<UserOutput>> All()
        {
            return await _mediator.Send(new GetUsersQuery());
        }

        [HttpPut]
        public async Task Update(UpdateUserCommand updateUserCommand)
        {
            await _mediator.Send(updateUserCommand);
        }

        [HttpDelete]
        public async Task Delete(DeleteUserCommand deleteUserCommand)
        {
            await _mediator.Send(deleteUserCommand);
        }

        [HttpPut("activate")]
        public async Task Activate(ActivateUserCommand activateUserCommand)
        {
            await _mediator.Send(activateUserCommand);
        }

        [HttpPost("createByAdmin")]
        public async Task<UserOutput> Add(AddUserCommand addUserCommand)
        {
            return await _mediator.Send(addUserCommand);
        }

    }
}
