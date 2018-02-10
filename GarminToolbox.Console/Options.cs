using CommandLine;
using CommandLine.Text;

namespace GarminConnectClient.Demo
{
	sealed class Options
	{
		[Option('u', "UserName", Required = true, HelpText = "Garmin Connect user name.")]
		public string UserName { get; set; }

		[Option('p', "Password", Required = false, HelpText = "Garmin Connect password.")]
		public string Password { get; set; }
	}

}
