using Common.Infrastructure.Mails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Support.Application.ApplicationInfos;
using Support.Domain.Exceptions;
using Support.Domain.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Support.Application.Mail.Commands
{
    public record SendMailSupportCommand(string Subject, string Body, IFormFileCollection Files, string UserFirstName, string UserLastName, string UserEmail, IEnumerable<string> RfaEmails) : IRequest;

    public record SendMailAccessRequestCommand(string FirstName, string LastName, string Email, IEnumerable<string> RfaEmails) : IRequest;

    public record UserSupportMail(string FirstName, string LastName, string Email);

    public class MailCommandHandler : IRequestHandler<SendMailSupportCommand>, IRequestHandler<SendMailAccessRequestCommand>
    {
        private readonly IEmailSenderService _repository;

        private readonly EmailConfiguration _emailConfig;

        private readonly IApplicationInfoRepository _applicationInfoRepository;

        public MailCommandHandler(IEmailSenderService repository, EmailConfiguration configuration, IApplicationInfoRepository applicationInfoRepository)
        {
            _repository = repository;
            _emailConfig = configuration;
            _applicationInfoRepository = applicationInfoRepository;
        }

        public async Task<Unit> Handle(SendMailSupportCommand request, CancellationToken cancellationToken)
        {
            var message = new Message(
                "ContactSupport",
                new string[] { _emailConfig.Addressee },
                request.Subject,
                request.Files,
                request.UserEmail,
                DateTime.Now,
                request.Body);

            var appInfo = await _applicationInfoRepository.Get();

            await _repository.SendMailSupport(message, new UserSupportMail(request.UserFirstName, request.UserLastName, request.UserEmail), appInfo, request.RfaEmails);

            return new Unit();
        }

        public async Task<Unit> Handle(SendMailAccessRequestCommand request, CancellationToken cancellationToken)
        {
            var appInfo = await _applicationInfoRepository.Get();
            var rfaProfiles = request.RfaEmails;
            if (rfaProfiles == default || !rfaProfiles.Any())
            {
                throw new NoRfaFoundException();
            }

            //Send mail to the rfa account
            await _repository.SendMail(
                new Message(
                    "AccessRequestRfa",
                    rfaProfiles,
                    "Nouvelle demande d'accès",
                    appInfo.NameApp.Value, request.FirstName, request.LastName, request.Email, _emailConfig.RedirectUrlAddUser
                ), appInfo
            );

            //Send mail to user account
            await _repository.SendMail(
                new Message(
                    "AccessRequestUser",
                    new List<string> { request.Email },
                    "Votre demande d'accès a été prise en compte."
                ), appInfo
            );
            return new Unit();
        }



    }
}
