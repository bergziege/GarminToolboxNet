namespace GarminConnectClient
{
    public interface ISessionService
    {
        Session Session { get; }
        bool SignIn(string userName, string password);
        void SignOut();
    }
}