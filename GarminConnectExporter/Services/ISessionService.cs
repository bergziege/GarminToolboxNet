using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Services
{
    public interface ISessionService
    {
        Session Session { get; }
        bool SignIn(string userName, string password);
        void SignOut();
    }
}