using System.ComponentModel.DataAnnotations;

namespace Sneaker_Store_Web.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        [Required, StringLength(50)]
        public required string BrandName { get; set; }
        public List<Product>? Products { get; set; }
    }
}
