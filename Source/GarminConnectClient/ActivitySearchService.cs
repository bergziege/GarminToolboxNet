using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GarminConnectClient.Data;

namespace GarminConnectClient
{
    public class ActivitySearchService : IActivitySearchService
    {
		private readonly ISessionService _sessionService;

		public ActivitySearchService(ISessionService sessionService)
		{
			this._sessionService = sessionService;
		}

		public ActivitySearchResultsContainer FindActivities()
		{
			return FindActivities(new ActivitySearchFilters());
		}

		public ActivitySearchResultsContainer FindActivities(ActivitySearchFilters filters)
		{
			string url = BuildSearchUrl(filters);
			Debug.WriteLine("FindActivities: {0}", (object)url);
			var request = HttpUtils.CreateRequest(url, _sessionService.Session.Cookies);
			var response = (HttpWebResponse)request.GetResponse();
			string responseText = response.GetResponseAsString();
			return ActivitySearchResultsContainer.ParseJson(responseText);
		}


		public List<Activity> FindAllActivities(out IList<string> errors)
		{
			return FindAllActivities(new ActivitySearchFilters(), out errors);
		}

		public List<Activity> FindAllActivities(ActivitySearchFilters filters, out IList<string> errors)
		{
            errors = new List<string>();
			var activities = new List<Activity>();
			ActivitySearchResultsContainer results = null;
			do
			{
			    //Thread.Sleep(TimeSpan.FromMilliseconds(250));
                filters.Page++;
				Debug.WriteLine("Searching page {0}", filters.Page);
                try
                {
                    results = FindActivities(filters);
                    activities.AddRange(results.Results.Activities.Select(a => a.Activity));
                }
                catch (Exception ex)
                {
                    results.Results.CurrentPage++;
                    errors.Add($"Page:{filters.Page}, PageSize:{filters.ActivitiesPerPage}");
                }
				Debug.WriteLine("Found page {0} or {1}", results.Results.CurrentPage, results.Results.TotalPages);
			} while (results.Results.CurrentPage < results.Results.TotalPages && results.Results.CurrentPage < filters.MaxPages);

			return activities;
		}

		private static string BuildSearchUrl(ActivitySearchFilters filters)
		{
			// Example URL
			// http://connect.garmin.com/proxy/activity-search-service-1.2/json/activities?currentPage=1&sortOrder=DESC&limit=100
			// http://connect.garmin.com/proxy/activity-search-service-1.2/json/activities?currentPage=1&sortOrder=DESC&limit=100&beginTimestamp%3E=2012-11-24T03:00:00

			const string serviceUrl = "http://connect.garmin.com/proxy/activity-search-service-1.2/json/activities";

			var queryString = filters.ToQueryString();
			return String.Format("{0}?{1}", serviceUrl, queryString);
		}

	}
}
