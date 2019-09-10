using System.Collections.Generic;
using ActivityArchive.Domain;

namespace ActivityArchive.Persistence
{
    public interface IActivityMetadataDao
    {
        void Insert(ActivityMetadata activityMetadata);

        IList<ActivityMetadata> GetAll();
        bool Exists(string activityId);
        IList<ActivityMetadata> FindAllWithoutOriginalFile();
        void Update(ActivityMetadata activityMetadata);
        IList<ActivityMetadata> FindAllWithoutGpx();
        IList<ActivityMetadata> FindAllWithoutGpxAndNotFailed();
        IList<ActivityMetadata> FindAllWithinLastDays(int numberOfLastDays);
        IList<ActivityMetadata> FindAllWithGpx();
    }
}