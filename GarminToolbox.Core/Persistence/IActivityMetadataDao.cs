using System.Collections.Generic;
using GarminToolbox.Core.Domain;

namespace GarminToolbox.Core.Persistence
{
    public interface IActivityMetadataDao
    {
        void Insert(ActivityMetadata activityMetadata);

        IList<ActivityMetadata> GetAll();
        bool Exists(string activityId);
        IList<ActivityMetadata> FindAllWithoutOriginalFile();
        void Update(ActivityMetadata activityMetadata);
        IList<ActivityMetadata> FindAllWithoutGpx();
    }
}