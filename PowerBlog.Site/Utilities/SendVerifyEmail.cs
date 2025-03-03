using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace PowerBlog.Site.Utilities
{
    public class SendVerifyEmail
    {
        private readonly IConfiguration _configuration;
        public SendVerifyEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string email, string verificationCode)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            string smtpServer = emailSettings["SmtpServer"]!;
            int port = int.Parse(emailSettings["Port"]!);
            string senderEmail = emailSettings["SenderEmail"]!;
            string senderPassword = emailSettings["SenderPassword"]!;
            bool enableSSL = bool.Parse(emailSettings["EnableSSL"]!);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PowerBlog", senderEmail));

            string recipientEmail = email;
            message.To.Add(new MailboxAddress("", recipientEmail));

            message.Subject = "کد تایید حساب کاربری";
            message.Body = new TextPart("html")
            {
                Text = $"<h1>کد تایید شما:</h1><h3>{verificationCode}</h3>"
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(senderEmail, senderPassword);

                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
