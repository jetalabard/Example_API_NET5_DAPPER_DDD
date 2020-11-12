using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Support.Domain.Message
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string TemplateKey { get; set; }

        public object[] Data { get; set; }

        public IFormFileCollection Files { get; set; }

        public Message(string templateKey, IEnumerable<string> to, string subject, IFormFileCollection files = null, params object[] data)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Files = files;
            TemplateKey = templateKey;
            Data = data;
        }

        public Message(string templateKey, IEnumerable<string> to, string subject, params object[] data)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Files = null;
            TemplateKey = templateKey;
            Data = data;
        }
    }
}
