using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityArchive.Domain;
using ActivityArchive.Services;

namespace ReportGenerator.Services.Impl
{
    public class ReportService : IReportService
    {
        private readonly IActivityArchiveService _activityService;

        public ReportService(IActivityArchiveService activityService)
        {
            _activityService = activityService;
        }

        public string CreateTextReportForLastSevenDays()
        {
            IList<ActivityMetadata> activitiesFromLastDays = _activityService.GetActivitiesFromLastDays(7);

            StringBuilder reportBuilder = new StringBuilder();
            foreach (IGrouping<DateTime?, ActivityMetadata> activityMetadatas in activitiesFromLastDays.OrderByDescending(x=>x.Start?.Date).GroupBy(x=>x.Start?.Date))
            {
                reportBuilder.AppendLine($"-- {activityMetadatas.Key?.ToShortDateString()} ------");
                reportBuilder.AppendLine($"Gesamt KM: {activityMetadatas.Where(x => x.Distance.HasValue).Sum(x => x.Distance)}");
                foreach (ActivityMetadata activityMetadata in activityMetadatas)
                {
                    reportBuilder.AppendLine(
                        $"{activityMetadata.ActivityType} - {activityMetadata.Distance}KM - {activityMetadata.Duration} - {activityMetadata.MovingDuration}");
                }

                reportBuilder.AppendLine();
            }
            return reportBuilder.ToString();
        }
    }
}