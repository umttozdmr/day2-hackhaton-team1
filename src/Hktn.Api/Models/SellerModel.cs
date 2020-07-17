using System.Collections.Generic;

namespace Hktn.Api.Models
{
    public class SellerModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}