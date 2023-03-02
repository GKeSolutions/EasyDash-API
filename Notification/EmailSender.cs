using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;

namespace Notification
{
    public class EmailSender
    {
        private readonly EmailConfiguration EmailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            EmailConfig = emailConfig;
        }

        public async Task<bool> SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            return await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmailConfig.DisplayName, EmailConfig.From));
            emailMessage.To.AddRange(message.To);
            if(message.Cc is not null) emailMessage.Cc.AddRange(message.Cc);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
        private async Task<bool> Send(MimeMessage mailMessage, CancellationToken ct = default)
        {
            
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            if (EmailConfig.UseSSL)
            {
                await client.ConnectAsync(EmailConfig.SmtpServer, EmailConfig.Port, SecureSocketOptions.SslOnConnect, ct);
            }
            else if (EmailConfig.UseStartTls)
            {
                await client.ConnectAsync(EmailConfig.SmtpServer, EmailConfig.Port, SecureSocketOptions.StartTls, ct);
            }
            //client.Connect(EmailConfig.SmtpServer, EmailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
            //client.AuthenticationMechanisms.Remove("XOAUTH2");
            //await client.AuthenticateAsync(EmailConfig.UserName, EmailConfig.Password, ct);

            await client.SendAsync(mailMessage, ct);
            await client.DisconnectAsync(true, ct);
            return true;
        }
    }
}