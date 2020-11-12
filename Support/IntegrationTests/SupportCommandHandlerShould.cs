using FluentAssertions;
using Xunit;
using System;
using Support.Infrastructure.Repository;
using Support.IntegrationTests.Helpers;
using Support.Application.Mail.Commands;
using Support.Application.Mail;
using Support.Infrastructure;
using Common.Infrastructure.Mails;
using Support.Application.ApplicationInfos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Support.IntegrationTests
{
    public class SupportCommandHandlerShould : IntegrationTestBaseSupport
    {
        private readonly MailCommandHandler _sendMaiLCommandHandler;

        private readonly IEmailSenderService _emailSenderService;

        private readonly IApplicationInfoRepository _applicationInfoRepository;

        private readonly EmailConfiguration _emailConfiguration;

        public SupportCommandHandlerShould() : base()
        {
            _emailConfiguration = new EmailConfiguration
            {
                Addressee = "",
                FromName = "Contact",
                SmtpServer = "",
                Port = 587,
                UserName = "apikey",
                Password = "",
                FromAddress = "",
            };
            _applicationInfoRepository = new ApplicationInfoRepository(ExampleContext);
            _emailSenderService = new EmailSenderService(_emailConfiguration);
            _sendMaiLCommandHandler = new MailCommandHandler(_emailSenderService, _emailConfiguration, _applicationInfoRepository);
        }

        [Fact]
        public void SendMail()
        {
            Func<Task> sendMail = async () => await _sendMaiLCommandHandler.Handle(new SendMailSupportCommand("Subject", "Body", null, "Firstname", "Lastname", "@gmail.com", new List<string> { "@gmail.com", "@gmail.com" }), default);

            sendMail.Should().NotThrow();
        }


        [Fact]
        public void SendMailAccessRequest()
        {
            Func<Task> sendMail = async () => await _sendMaiLCommandHandler.Handle(new SendMailAccessRequestCommand("Firstname", "Lastname", "@gmail.com", new List<string> { "@gmail.com", "@gmail.com" }), default);

            sendMail.Should().NotThrow();
        }
    }
}
