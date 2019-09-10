using GarminConnectExporter.Config;
using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Services
{
    public interface ISessionService
    {
        Session Session { get; }
        bool SignIn(GarminConfiguration garminSettings);
        void SignOut();
    }
}