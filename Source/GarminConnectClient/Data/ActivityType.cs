using System.Runtime.Serialization;

namespace GarminConnectClient.Data
{

	/// <example>
	/// "activityType": {
	///		"display": "Running",
	///		"key": "running",
	///		"fieldNameDisplay": "Activity Type",
	///		"parent": {
	///			"display": "Running",
	///			"key": "running",
	///			"fieldNameDisplay": "Activity Type"
	///		}
	///	},
	/// </example>
	[DataContract]
	public class ActivityTypeContainer
	{
		[DataMember(Name = "typeKey")]
		public string Key { get; set; }
	}

}
