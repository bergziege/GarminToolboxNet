using System.Collections.Generic;
using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Services
{
    public interface IActivityService
    {
        void SyncLatestMetadata();
        void SyncFiles();
        void CleanFiles();
        void Export(string activityId, string fileName, ExportFileType fileType);
        IList<ActivityMetadata> GetActivitiesFromLastDays(int numberOfLastDays);
    }
}