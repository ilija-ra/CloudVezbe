﻿using System.Runtime.Serialization;

namespace CommunicationLibrary.Models
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public long? Id { get; set; }

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
    }
}