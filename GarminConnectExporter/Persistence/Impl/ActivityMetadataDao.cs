using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dommel;
using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Persistence.Impl
{
    public class ActivityMetadataDao : IActivityMetadataDao
    {
        private readonly IDbConnection _connection;

        public ActivityMetadataDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Insert(ActivityMetadata activityMetadata)
        {
            _connection.Insert(activityMetadata);
        }

        public IList<ActivityMetadata> GetAll()
        {
            return _connection.GetAll<ActivityMetadata>().ToList();
        }

        public bool Exists(string activityId)
        {
            ActivityMetadata firstOrDefault = _connection.Select<ActivityMetadata>(x=>x.ActivityId == activityId).FirstOrDefault();
            return firstOrDefault != null;
        }

        public IList<ActivityMetadata> FindAllWithoutOriginalFile()
        {
            return _connection.Select<ActivityMetadata>(x => !x.HasOriginal).ToList();
        }

        public void Update(ActivityMetadata activityMetadata)
        {
            _connection.Update(activityMetadata);
        }

        public IList<ActivityMetadata> FindAllWithoutGpx()
        {
            return _connection.Select<ActivityMetadata>(x => !x.HasGpx).ToList();
        }

        public IList<ActivityMetadata> FindAllWithoutGpxAndNotFailed()
        {
            return _connection.Select<ActivityMetadata>(x => !x.HasGpx && !x.GpxDownloadFailed).ToList();
        }
    }
}