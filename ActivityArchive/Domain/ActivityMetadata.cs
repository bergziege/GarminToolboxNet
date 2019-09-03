using System;

namespace ActivityArchive.Domain
{
    public class ActivityMetadata
    {
        public string ActivityId { get; set; }
        public string Name { get; set; }
        public string ActivityType { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public double? Distance { get; set; }
        public double? BeginLatitude { get; set; }
        public double? EndLatitude { get; set; }
        public double? BeginLongitude { get; set; }
        public double? EndLongitude { get; set; }
        public int Id { get; set; }
        public bool HasOriginal { get; set; }
        public bool HasGpx { get; set; }
        public bool GpxDownloadFailed { get; set; }
        public double? Duration { get; set; }
        public double? MovingDuration { get; set; }

        /// <summary>
        /// For Dapper
        /// </summary>
        public ActivityMetadata()
        {
        }

        public ActivityMetadata(string activityId, string name, string activityType, DateTime? start, DateTime? end, double? distance, double? beginLatitude, double? endLatitude, double? beginLongitude, double? endLongitude, double? duration, double? movingDuration)
        {
            ActivityId = activityId;
            Name = name;
            ActivityType = activityType;
            Start = start;
            End = end;
            Distance = distance;
            BeginLatitude = beginLatitude;
            EndLatitude = endLatitude;
            BeginLongitude = beginLongitude;
            EndLongitude = endLongitude;
            Duration = duration;
            MovingDuration = movingDuration;
        }

        public void UpdateHasOriginal(bool hasOriginal = true)
        {
            HasOriginal = hasOriginal;
        }

        public void UpdateHasGpx(bool hasGpx = true)
        {
            HasGpx = hasGpx;
        }

        public void UpdateGpxDownloadFailed(bool gpxDownloadFailed)
        {
            GpxDownloadFailed = gpxDownloadFailed;
        }
    }
}