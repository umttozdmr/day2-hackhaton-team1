namespace Hktn.Api.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}