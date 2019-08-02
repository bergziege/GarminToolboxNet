using System.Net;

namespace GarminConnectExporter.Domain
{
	public class Session
	{
		public CookieContainer Cookies { get; private set; }

		public Session()
		{
			Cookies = new CookieContainer();
		}
	}
}
