using System.Collections.Generic;
using ActivityArchive.Domain;

namespace ActivityArchive.Services
{
    public interface IActivityArchiveService
    {
        IList<ActivityMetadata> GetActivitiesFromLastDays(int numberOfLastDays);
        IList<ActivityMetadata> GetAllWithGpx();
    }
}