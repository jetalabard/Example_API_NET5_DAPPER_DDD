namespace Common.Infrastructure.Mails
{
    public class EmailConfiguration
    {
        public string FromName { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Addressee { get; set; }

        public string FromAddress { get; set; }

        public string RedirectUrlAddUser { get; set; }
    }
}
