using Support.Application.Mail.Commands;
using Support.Domain.ApplicationInfos;
using Support.Domain.Message;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Support.Application.Mail
{
    public interface IEmailSenderService
    {
        Task SendMailSupport(Message message, UserSupportMail user, ApplicationInfo appInfo, IEnumerable<string> rfaEmails);

        Task SendMail(Message message, ApplicationInfo info);
    }
}
