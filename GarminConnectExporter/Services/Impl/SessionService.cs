using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using GarminConnectExporter.Config;
using GarminConnectExporter.Domain;
using GarminConnectExporter.Infrastructure;

namespace GarminConnectExporter.Services.Impl
{
    public class SessionService : ISessionService
    {
		private const string ClientId = "GarminConnect";

		public Session Session { get; private set; }

		public bool SignIn(GarminConfiguration garminSettings)
		{
			try
			{
				Session = new Session();
				var signInResponse = PostLogInRequest(garminSettings.Username, garminSettings.Password);
                var ticketUrl = GetServiceTicketUrl(signInResponse);
                return ProcessTicket(ticketUrl);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error signing in. {0}", ex.Message);
			}

			return false;
		}

		private HttpWebResponse PostLogInRequest(string userName, string password)
		{
			var request = HttpUtils.CreateRequest(GetLogInUrl(), Session.Cookies);
            request.Headers.Add("origin", "https://sso.garmin.com");
			request.WriteFormData(BuildLogInFormData(userName, password));
			return (HttpWebResponse)request.GetResponse();
		}

		private static string GetLogInUrl()
		{
			var qs = HttpUtils.CreateQueryString();
			qs.Add("service", "https://connect.garmin.com");
			qs.Add("clientId", ClientId);
			return "https://sso.garmin.com/sso/signin?" + qs;
		}

		private static NameValueCollection BuildLogInFormData(string userName, string password)
		{
			var data = HttpUtils.CreateQueryString();
			data.Add("username", userName);
			data.Add("password", password);
			data.Add("embed", "true");
			data.Add("_eventId", "submit");
			return data;
		}

		private bool ProcessTicket(string ticketUrl)
		{
			var request = HttpUtils.CreateRequest(ticketUrl, Session.Cookies);
			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception("Invalid ticket URL.");

			return IsDashboardUri(response.ResponseUri);
		}

		private static bool IsDashboardUri(Uri uri)
		{
        		 if(uri != null && uri.LocalPath == "/dashboard")
        		    return uri.Host == "connect.garmin.com" && uri.LocalPath == "/dashboard";
        		 else
			    return uri.Host == "connect.garmin.com" && uri.LocalPath == "/modern/";
		}

		private string GetServiceTicketUrl(HttpWebResponse signInResponse)
		{
			var content = signInResponse.GetResponseAsString();
			return ParseServiceTicketUrl(content);
		}

		private static string ParseServiceTicketUrl(string content)
		{
			// var response_url                 = 'http://connect.garmin.com/post-auth/login?ticket=ST-XXXXXX-XXXXXXXXXXXXXXXXXXXX-cas';
		    content = content.Replace("\\", "");
		    content = content.Replace("\"", "'");
			var re = new Regex(@"response_url\s*=\s*'(?<url>[^']*)'");
			var m = re.Match(content);
			if (!m.Success)
				throw new Exception("Servcie ticket URL not found.");

			return m.Groups["url"].Value;
		}

		public void SignOut()
		{
			var request = HttpUtils.CreateRequest("https://sso.garmin.com/sso/logout?service=http%3A%2F%2Fconnect.garmin.com%2F", Session.Cookies);
			request.GetResponse();
		}
	}
}
