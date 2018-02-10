using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using GarminToolbox.Core.Domain;

namespace GarminToolbox.Core.Persistence.Mapping
{
    public class ActivityMetadataMap : DommelEntityMap<ActivityMetadata>
    {
        public ActivityMetadataMap()
        {
            ToTable("activity_metadata");
            Map(x => x.Id).ToColumn("id").IsIdentity();
            Map(x => x.ActivityId).ToColumn("activity_id");
            Map(x => x.Name).ToColumn("name");
            Map(x => x.ActivityType).ToColumn("activity_type");
            Map(x => x.Start).ToColumn("begin");
            Map(x => x.End).ToColumn("end");
            Map(x => x.BeginLatitude).ToColumn("begin_latitude");
            Map(x => x.BeginLongitude).ToColumn("begin_longitude");
            Map(x => x.EndLatitude).ToColumn("end_latitude");
            Map(x => x.EndLongitude).ToColumn("end_longitude");
            Map(x => x.HasOriginal).ToColumn("has_original");
            Map(x => x.HasGpx).ToColumn("has_gpx");
        }
    }
}