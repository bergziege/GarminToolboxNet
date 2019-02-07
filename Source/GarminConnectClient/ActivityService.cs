using System;
using System.Net;

namespace GarminConnectClient
{
    public class ActivityService : IActivityService
    {
		private readonly ISessionService session;

		public ActivityService(ISessionService session)
		{
			this.session = session;
		}
        
		public void Export(string activityId, string fileName, ExportFileType fileType)
		{
			string url = BuildExportUrl(fileType, activityId);
			var request = HttpUtils.CreateRequest(url, session.Session.Cookies);
			var response = (HttpWebResponse)request.GetResponse();
			response.SaveResponseToFile(fileName);
		}

		private static string BuildExportUrl(ExportFileType fileType, string activityId)
		{
		    switch (fileType)
		    {
		        case ExportFileType.Gpx:
		            return String.Format("https://connect.garmin.com/modern/proxy/download-service/export/{0}/activity/{1}",
		                fileType.ToString().ToLower(), activityId);
		        case ExportFileType.Original:
                    return String.Format("https://connect.garmin.com/modern/proxy/download-service/files/activity/{0}",activityId); // https://connect.garmin.com/modern/proxy/download-service/files/activity/3363902914
                default:
		            throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
		    }
		}
	}
}
