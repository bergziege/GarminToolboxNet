using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using GarminConnectClient.Data;
using Newtonsoft.Json;

namespace GarminConnectClient
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

		public List<Activity> FindAllActivities(out IList<string> errors)
		{
            errors = new List<string>();
			var activities = new List<Activity>();
		    int pageSize = 100;
		    int page = 0;
		    bool hasError = false;
            IList<Activity> pageResults = new List<Activity>();
			do
			{

                try
                {
                    pageResults = FindActivities(page * pageSize, pageSize);
                    activities.AddRange(pageResults);
                    page++;
                }
                catch (Exception ex)
                {
                    hasError = true;
                }
                
			} while (!hasError && pageResults.Any());

			return activities;
		}

		private static string BuildSearchUrl(int start, int limit)
		{
			const string serviceUrl = "https://connect.garmin.com/modern/proxy/activitylist-service/activities/search/activities";
			return $"{serviceUrl}?start={start}&limit={limit}";
		}

	}
}
