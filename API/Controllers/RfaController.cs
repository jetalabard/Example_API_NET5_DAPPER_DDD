using MediatR;
using Microsoft.AspNetCore.Mvc;
using Support.Application.ApplicationInfos.Queries;
using UserAccess.Application.Rfas.Commands;
using Support.Application.Rfas.Outputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RfaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RfaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<RfaOutput>> All()
        {
            return await _mediator.Send(new GetAllRfasQuery());
        }

        [HttpGet("byEmail/{email}")]
        public async Task<RfaOutput> GetByEmail(string email)
        {
            return await _mediator.Send(new GetRfaQuery(email));
        }

        [HttpPost]
        public async Task<Unit> Add(AddRfaCommand addRfaCommand)
        {
            return await _mediator.Send(addRfaCommand);
        }

        [HttpPut]
        public async Task<Unit> Update(UpdateRfaCommand updateRfaCommand)
        {
            return await _mediator.Send(updateRfaCommand);
        }
    }
}
