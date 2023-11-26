using System.Runtime.Serialization;

namespace CommunicationLibrary.Models.Base
{
    [DataContract]
    public class BaseEntity
    {
        [DataMember]
        public long? Id { get; set; }
    }
}
