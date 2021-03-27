using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using GarminConnectExporter.Infrastructure;
using Newtonsoft.Json;
using Activity = GarminConnectExporter.Domain.Activity;


namespace GarminConnectExporter.Services.Impl
{
    public class ActivitySearchService : IActivitySearchService
    {
		private readonly ISessionService _sessionService;

		public ActivitySearchService(ISessionService sessionService)
		{
			this._sessionService = sessionService;
		}

		public List<Activity> FindActivities(int start, int limit)
		{
			string url = BuildSearchUrl(start, limit);
			Debug.WriteLine("FindActivities: {0}", (object)url);
			var request = HttpUtils.CreateRequest(url, _sessionService.Session.Cookies);
			var response = (HttpWebResponse)request.GetResponse();
			string responseText = response.GetResponseAsString();
		    return JsonConvert.DeserializeObject<List<Activity>>(responseText);
        }

		private static string BuildSearchUrl(int start, int limit)
		{
			const string serviceUrl = "https://connect.garmin.com/proxy/activitylist-service/activities/search/activities";
			return $"{serviceUrl}?start={start}&limit={limit}";
		}

	}
}
