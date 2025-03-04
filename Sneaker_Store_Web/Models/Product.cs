using System.ComponentModel.DataAnnotations;

namespace Sneaker_Store_Web.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required, StringLength(100)]
        public required string ProductName { get; set; }
        [Range(0.01, 50000000.0)]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }
        public int BrandId { get; set; }
         public Brand? Brand { get; set; } 
    }   
}
