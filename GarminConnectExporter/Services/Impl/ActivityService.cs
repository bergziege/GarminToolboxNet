﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using GarminConnectExporter.Domain;
using GarminConnectExporter.Infrastructure;
using GarminConnectExporter.Persistence;
using Activity = GarminConnectExporter.Domain.Activity;

namespace GarminConnectExporter.Services.Impl
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityMetadataDao _activityMetadataDao;
        private readonly IActivitySearchService _activitySearchService;
        private readonly ISessionService _sessionService;

        public ActivityService(IActivityMetadataDao activityMetadataDao, IActivitySearchService activitySearchService, ISessionService sessionService)
        {
            _activityMetadataDao = activityMetadataDao;
            _activitySearchService = activitySearchService;
            _sessionService = sessionService;
        }

        public void SyncLatestMetadata()
        {
            List<Activity> activities = _activitySearchService.FindActivities(0,200);

            foreach (Activity activity in activities)
            {
                ActivityMetadata metadata = new ActivityMetadata(
                    activity.ActivityId,
                    activity.ActivityName,
                    activity.ActivityType.Key.ToString(),
                    activity.BeginTimestamp,
                    null,
                    activity.SumDistance,
                    activity.BeginLatitude,
                    activity.EndLatitude,
                    activity.BeginLongitude,
                    activity.EndLongitude,
                    activity.DurationInSeconds,
                    activity.MovingDurationInSeconds);
                if (!_activityMetadataDao.Exists(metadata.ActivityId))
                {
                    _activityMetadataDao.Insert(metadata);
                }
                else
                {
                    Debug.WriteLine($"Activity {activity.ActivityId} exists.");
                }
            }
        }

        public void SyncOriginalFiles()
        {
            IList<ActivityMetadata> activitiesWithoutOriginalFile = _activityMetadataDao.FindAllWithoutOriginalFile();
            
            string downloadDirectory = @"P:\GarminToolboxNet\";
            foreach (ActivityMetadata metadata in activitiesWithoutOriginalFile)
            {
                try
                {
                    Export(metadata.ActivityId, downloadDirectory + metadata.ActivityId + ".zip",
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
            IList<ActivityMetadata> activitiesWithoutOriginalFile = _activityMetadataDao.FindAllWithoutGpxAndNotFailed();
            
            string downloadDirectory = @"P:\GarminToolboxNet\";
            foreach (ActivityMetadata metadata in activitiesWithoutOriginalFile)
            {
                try
                {
                    Export(metadata.ActivityId, downloadDirectory + metadata.ActivityId + ".gpx", ExportFileType.Gpx);
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
            string downloadDirectory = @"P:\GarminToolboxNet\";
            IList<ActivityMetadata> metadatas = _activityMetadataDao.GetAll();
            foreach (ActivityMetadata metadata in metadatas.Where(x=>x.HasGpx))
            {
                FileInfo file = new FileInfo(downloadDirectory + metadata.ActivityId + ".gpx");
                if (!file.Exists)
                {
                    metadata.UpdateHasGpx(false);
                    _activityMetadataDao.Update(metadata);
                }
                else
                {
                    if (file.Length == 0)
                    {
                        file.Delete();
                        metadata.UpdateHasGpx(false);
                        metadata.UpdateGpxDownloadFailed(true);
                        _activityMetadataDao.Update(metadata);
                    }
                }
            }
        }

        public void Export(string activityId, string fileName, ExportFileType fileType)
        {
            string url = BuildExportUrl(fileType, activityId);
            var request = HttpUtils.CreateRequest(url, _sessionService.Session.Cookies);
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
                    return String.Format("https://connect.garmin.com/modern/proxy/download-service/files/activity/{0}", activityId); // https://connect.garmin.com/modern/proxy/download-service/files/activity/3363902914
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }
    }
}