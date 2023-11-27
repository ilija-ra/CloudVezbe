using System.Runtime.Serialization;

namespace Communication.Models.Base
{
    [DataContract]
    public class BaseEntityViewModel
    {
        [DataMember]
        public long? Id { get; set; }
    }
}
