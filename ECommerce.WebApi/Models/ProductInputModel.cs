
namespace ECommerce.WebApi.Models
{
    public class ProductInputModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }

       // public virtual Category Category { get; set; } = default!;
    }
}
