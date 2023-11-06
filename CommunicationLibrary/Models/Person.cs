using System.Runtime.Serialization;

namespace Client.Models
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public string? FirstName { get; set; }

        [DataMember]
        public string? LastName { get; set; }

        [DataMember]
        public DateTime? DateOfBirth { get; set; }
    }
}
