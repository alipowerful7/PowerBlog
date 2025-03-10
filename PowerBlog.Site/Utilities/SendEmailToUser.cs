using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace PowerBlog.Site.Utilities
{
    public class SendEmailToUser
    {
        private readonly IConfiguration _configuration;
        public SendEmailToUser(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string email, string subject, string userMessage)
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

            message.Subject = $"{subject}";
            message.Body = new TextPart("None")
            {
                Text = userMessage
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
