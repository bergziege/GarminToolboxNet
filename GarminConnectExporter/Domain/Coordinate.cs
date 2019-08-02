using System.Runtime.Serialization;

namespace GarminConnectExporter.Domain
{
    [DataContract]
    public class Coordinate
    {
        [DataMember(Name = "value")]
        public double Value { get; set; }
    }
}