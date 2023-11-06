using Client.Models.BaseModel;

namespace Client.Models
{
    public class Person : BaseEntity
    {
        public long? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
