using System.Threading.Tasks;
using Mailer.Config;

namespace Mailer.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailConfiguration mailSettings, string message);
    }
}