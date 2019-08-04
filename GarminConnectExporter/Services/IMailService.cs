using System.Threading.Tasks;
using GarminConnectExporter.Config;

namespace GarminConnectExporter.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailConfiguration mailSettings, string message);
    }
}