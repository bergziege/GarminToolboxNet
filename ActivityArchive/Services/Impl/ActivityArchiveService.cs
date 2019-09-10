using System.Collections.Generic;
using ActivityArchive.Domain;
using ActivityArchive.Persistence;

namespace ActivityArchive.Services.Impl
{
    public class ActivityArchiveService : IActivityArchiveService
    {
        private readonly IActivityMetadataDao _activityMetadataDao;

        public ActivityArchiveService(IActivityMetadataDao activityMetadataDao)
        {
            _activityMetadataDao = activityMetadataDao;
        }

        public IList<ActivityMetadata> GetActivitiesFromLastDays(int numberOfLastDays)
        {
            return _activityMetadataDao.FindAllWithinLastDays(numberOfLastDays);
        }

        public IList<ActivityMetadata> GetAllWithGpx()
        {
            return _activityMetadataDao.FindAllWithGpx();
        }
    }
}