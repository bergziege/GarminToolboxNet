using System.Runtime.Serialization;

namespace GarminConnectExporter.Domain
{

	[DataContract]
	public class ActivityTypeContainer
	{
		[DataMember(Name = "typeKey")]
		public string Key { get; set; }
	}

}
