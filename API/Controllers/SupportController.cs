using Common.Domain.Emails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Support.Application.ApplicationInfos.Outputs;
using Support.Application.ApplicationInfos.Queries;
using Support.Application.Mail.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getAppInfo")]
        public async Task<ApplicationInfoOutput> GetApplicationInfo()
        {
            return await _mediator.Send(new GetApplicationInfoQuery());
        }


        [HttpPost("SendMail")]
        public async Task SendRequest([FromForm] SendMailSupportCommand sendMailSupportCommand)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            var userEmail = new Email(HttpContext.User.FindFirst(ClaimTypes.Email).Value);

            await _mediator.Send(new SendMailSupportCommand(sendMailSupportCommand.Subject, sendMailSupportCommand.Body, files, sendMailSupportCommand.UserFirstName, sendMailSupportCommand.UserLastName, userEmail.Value, new List<string> { "df.dev1.mobile@gmail.com", "df.dev2.mobile@gmail.com" }));
        }


        [HttpPost("SendMailAccessRequest")]
        public async Task SendMailAccessRequest(SendMailAccessRequestCommand accessRequest)
        {
            await _mediator.Send(accessRequest);
        }
    }
}
