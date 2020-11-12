using Common.Infrastructure.Mails;
using Common.Infrastructure.Mails.Helpers;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Support.Application.Mail;
using Support.Domain.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Support.Domain.ApplicationInfos;
using Support.Application.Mail.Commands;
using System.Text;
using Support.Domain.Exceptions;

namespace Support.Infrastructure
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSenderService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        private static IDictionary<string, byte[]> FormFileCollectionToDictionary(IFormFileCollection files)
        {
            Dictionary<string, byte[]> result = null;
            if (files == null || files.Count == 0)
            {
                return result;
            }

            result = new Dictionary<string, byte[]>();
            foreach (var attachment in files)
            {
                using var ms = new MemoryStream();
                attachment.CopyTo(ms);
                result.Add(attachment.FileName, ms.ToArray());
            }

            return result;
        }

        public async Task SendMailSupport(Message message, UserSupportMail user, ApplicationInfo appInfo, IEnumerable<string> rfaEmails)
        {
            message.Data = BuildMessageData(message, user, appInfo, rfaEmails);
            var emailMessage = CreateEmailMessage(message, appInfo);

            await SendAsync(emailMessage);
        }


        private static object[] BuildMessageData(Message message, UserSupportMail user, ApplicationInfo appInfo, IEnumerable<string> rfaEmails)
        {
            var dateDeContact = (DateTime)message.Data[1];
            var listRfaString = new StringBuilder();
            if (rfaEmails != null && rfaEmails.Any())
            {
                foreach (var email in rfaEmails)
                {
                    listRfaString.Append($"{email}<br/>");
                }
            }
            return new object[] {
                appInfo.CodeApp,
                appInfo.NameApp.Value,
                user.FirstName,
                user.LastName,
                dateDeContact.ToLocalTime().ToShortDateString(),
                dateDeContact.ToLocalTime().ToShortTimeString(),
                message.Data[2],
                listRfaString.ToString() };
        }

        private MimeMessage CreateEmailMessage(Message message, ApplicationInfo appInfo)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
            emailMessage.To.AddRange(message.To);

            emailMessage.Subject = $"ARIS {appInfo.CodeApp} [{appInfo.NameApp}] : {message.Subject}";

            string htmlBody = MailHelper.RenderHtml(message.TemplateKey, message.Data);
            string textBody = MailHelper.RenderText(message.TemplateKey, message.Data);

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody, TextBody = textBody };

            var attachements = FormFileCollectionToDictionary(message.Files);

            if (attachements != null && attachements.Any())
            {
                foreach (var attachment in attachements)
                {
                    bodyBuilder.Attachments.Add(attachment.Key, attachment.Value);
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            SmtpClient client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw new EmailNotSendException();
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        public async Task SendMail(Message message, ApplicationInfo info)
        {
            var emailMessage = CreateEmailMessage(message, info);

            await SendAsync(emailMessage);
        }
    }
}
