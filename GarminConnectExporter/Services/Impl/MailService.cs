using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GarminConnectExporter.Config;

namespace GarminConnectExporter.Services.Impl
{
    public class MailService : IMailService
    {
        public async Task SendMailAsync(MailConfiguration mailSettings, string message)
        {
            SmtpClient client = new SmtpClient(mailSettings.SmtpServer, mailSettings.SmtpServerPort);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailSettings.SmtpUser, mailSettings.SmtpPassword);

            MailMessage mailMessage = new MailMessage(mailSettings.Sender,mailSettings.Recipient,mailSettings.Subject,message);
            await client.SendMailAsync(mailMessage);
        }
    }
}