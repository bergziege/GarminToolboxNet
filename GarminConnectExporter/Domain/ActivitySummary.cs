using System.Runtime.Serialization;

namespace GarminConnectExporter.Domain
{
	
	[DataContract]
	public class ActivitySummary
	{
		//[DataMember(Name = "SumDuration")]
		//public SumDuration SumDuration { get; set; }

		[DataMember(Name = "SumDistance")]
		public SumDistance SumDistance { get; set; }

		[DataMember(Name = "BeginTimestamp")]
		public Timestamp BeginTimestamp { get; set; }

		[DataMember(Name = "EndTimestamp")]
		public Timestamp EndTimestamp { get; set; }

        [DataMember(Name = "BeginLatitude")]
        public Coordinate BeginLatitude { get; set; }

	    [DataMember(Name = "EndLatitude")]
        public Coordinate EndLatitude { get; set; }

	    [DataMember(Name = "EndLongitude")]
        public Coordinate EndLongitude { get; set; }

	    [DataMember(Name = "BeginLongitude")]
        public Coordinate BeginLongitude { get; set; }
	}
}
