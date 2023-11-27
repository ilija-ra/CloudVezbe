using Communication.Models.Base;
using System.Runtime.Serialization;

namespace Communication.Models
{
    [DataContract]
    public class BookViewModel : BaseEntityViewModel
    {
        [DataMember]
        public string? Title { get; set; }

        [DataMember]
        public string? Author { get; set; }

        [DataMember]
        public int? PagesNumber { get; set; }

        [DataMember]
        public int? PublicationYear { get; set; }

        [DataMember]
        public double? Price { get; set; }

        [DataMember]
        public int? Quantity { get; set; }
    }
}
