using Client.Models.BaseModel;

namespace Client.Models
{
    public class Book : BaseEntity
    {
        public string? Title { get; set; }

        public int? PagesNumber { get; set; }

        public decimal? Price { get; set; }
    }
}
