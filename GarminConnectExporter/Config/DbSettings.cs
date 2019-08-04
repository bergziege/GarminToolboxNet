using Org.BouncyCastle.Asn1.Mozilla;

namespace GarminConnectExporter.Config
{
    public class DbSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}