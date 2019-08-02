using System;
using System.Runtime.Serialization;

namespace GarminConnectExporter.Domain
{
	[DataContract]
	public class Activity
	{
		[DataMember(Name = "activityId")]
		public string ActivityId { get; set; }

		[DataMember(Name = "activityName")]
		public string ActivityName { get; set; }

		[DataMember(Name = "activityType")]
		public ActivityTypeContainer ActivityType { get; set; }
        
	    [DataMember(Name = "distance")]
	    public double? SumDistance { get; set; }

	    [DataMember(Name = "startTimeLocal")]
	    public DateTime? BeginTimestamp { get; set; }

	    [DataMember(Name = "duration")]
        public double? DurationInSeconds { get; set; }

	    [DataMember(Name = "startLatitude")]
	    public double? BeginLatitude { get; set; }

	    [DataMember(Name = "endLatitude")]
	    public double? EndLatitude { get; set; }

	    [DataMember(Name = "endLongitude")]
	    public double? EndLongitude { get; set; }

	    [DataMember(Name = "startLongitude")]
	    public double? BeginLongitude { get; set; }

	    [DataMember(Name = "movingDuration")]
        public double? MovingDurationInSeconds { get; set; }
    }
}