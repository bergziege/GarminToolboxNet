using System.Runtime.Serialization;

namespace GarminConnectClient.Data
{
    [DataContract]
    public class Coordinate
    {
        [DataMember(Name = "value")]
        public double Value { get; set; }
    }
}