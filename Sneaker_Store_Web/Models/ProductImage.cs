namespace Sneaker_Store_Web.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public required string Url { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
