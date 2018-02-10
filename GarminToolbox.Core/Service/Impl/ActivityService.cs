using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GarminConnectClient;
using GarminConnectClient.Data;
using GarminToolbox.Core.Domain;
using GarminToolbox.Core.Persistence;

namespace GarminToolbox.Core.Service.Impl
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityMetadataDao _activityMetadataDao;
        private readonly IActivitySearchService _activitySearchService;
        private readonly ISessionService _sessionService;
        private readonly GarminConnectClient.IActivityService _activityService;

        public ActivityService(IActivityMetadataDao activityMetadataDao, IActivitySearchService activitySearchService, ISessionService sessionService, GarminConnectClient.IActivityService activityService)
        {
            _activityMetadataDao = activityMetadataDao;
            _activitySearchService = activitySearchService;
            _sessionService = sessionService;
            _activityService = activityService;
        }

        public void SyncLatestMetadata()
        {
            List<Activity> activities = _activitySearchService.FindAllActivities(new ActivitySearchFilters { Page = 0, MaxPages = 20, ActivitiesPerPage = 1 }, out IList<string> errors);

            Console.WriteLine(string.Join(Environment.NewLine, errors));

            foreach (Activity activity in activities)
            {
                ActivityMetadata metadata = new ActivityMetadata(
                    activity.ActivityId,
                    activity.ActivityName,
                    activity.ActivityType.Key.ToString(),
                    activity.ActivitySummary.BeginTimestamp.Value,
                    activity.ActivitySummary.EndTimestamp?.Value,
                    activity.ActivitySummary.SumDistance?.Value,
                    activity.ActivitySummary.BeginLatitude?.Value,
                    activity.ActivitySummary.EndLatitude?.Value,
                    activity.ActivitySummary.BeginLongitude?.Value,
                    activity.ActivitySummary.EndLongitude?.Value);
                if (!_activityMetadataDao.Exists(metadata.ActivityId))
                {
                    _activityMetadataDao.Insert(metadata);
                }
            }
        }

        public void SyncOriginalFiles()
        {
            IList<ActivityMetadata> activitiesWithoutOriginalFile = _activityMetadataDao.FindAllWithoutOriginalFile();
            
            string downloadDirectory = @"C:\Temp\garmin-connect-net-originals\";
            foreach (ActivityMetadata metadata in activitiesWithoutOriginalFile)
            {
                try
                {
                    _activityService.Export(metadata.ActivityId, downloadDirectory + metadata.ActivityId + ".zip",
                        ExportFileType.Original);
                    metadata.UpdateHasOriginal();
                    _activityMetadataDao.Update(metadata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " | " + metadata.ActivityId);
                }
            }
        }

        public void SyncGpx()
        {
            IList<ActivityMetadata> activitiesWithoutOriginalFile = _activityMetadataDao.FindAllWithoutGpx();
            
            string downloadDirectory = @"C:\Temp\garmin-connect-net-originals\";
            foreach (ActivityMetadata metadata in activitiesWithoutOriginalFile)
            {
                try
                {
                    _activityService.Export(metadata.ActivityId, downloadDirectory + metadata.ActivityId + ".gpx", ExportFileType.Gpx);
                    metadata.UpdateHasGpx();
                    _activityMetadataDao.Update(metadata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " | " + metadata.ActivityId);
                }
            }
        }

        public void SyncFiles()
        {
            SyncOriginalFiles();
            SyncGpx();
        }

        public void CleanFiles()
        {
            string downloadDirectory = @"C:\Temp\garmin-connect-net-originals\";
            IList<ActivityMetadata> metadatas = _activityMetadataDao.GetAll();
            foreach (ActivityMetadata metadata in metadatas.Where(x=>x.HasGpx))
            {
                FileInfo file = new FileInfo(downloadDirectory + metadata.ActivityId + ".gpx");
                if (file.Length == 0)
                {
                    file.Delete();
                    metadata.UpdateHasGpx(false);
                    _activityMetadataDao.Update(metadata);
                }
            }
        }
    }
}