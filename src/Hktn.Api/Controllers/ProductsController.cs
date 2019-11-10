using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hktn.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Hktn.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;

        public ProductsController(IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }
        
        [HttpGet]
        public ActionResult<List<ProductModel>> GetBySellerId([FromQuery] int sellerId = 0)
        {
            if (sellerId == 0) return Ok(GetAll());
            
            return Ok(GetAll().Where(p => p.SellerId == sellerId).ToList());
        }
        
        [HttpGet("{id}")]
        public ActionResult<List<ProductModel>> Get([FromRoute] int id)
        {
            return Ok(GetAll().First(s => s.Id == id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductModel product)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync($"http://seller/sellers/{product.SellerId}");
            
            if (!result.IsSuccessStatusCode)
            {
                return NotFound();
            }
            
            var products = _memoryCache.Get<List<ProductModel>>("products") ?? new List<ProductModel>();
            products.Add(product);
            
            _memoryCache.Set("products", products);
            
            return Created($"products/{product.Id}", product);
        }
        
        private List<ProductModel> GetAll()
        {
            return _memoryCache.Get<List<ProductModel>>("products");
        }
    }
}