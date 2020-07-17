using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hktn.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;

namespace Hktn.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetBySellerId([FromQuery] int sellerId = 0)
        {
            var results = await GetAll();

            return Ok(sellerId == 0 ? results : results.Where(p => p.SellerId == sellerId).ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductModel>>> Get([FromRoute] int id)
        {
            var result = (await GetAll()).First(s => s.Id == id);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductModel product)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization,
                _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString());
            
            var result = await client.GetAsync($"http://seller/sellers/{product.SellerId}");
            
            if (!result.IsSuccessStatusCode)
            {
                return NotFound();
            }
            
            var products = _memoryCache.Get<List<ProductModel>>("products") ?? new List<ProductModel>();
            var lastId = products.Any() ? products.Max(p => p.Id) : 0;
            product.Id = lastId + 1;
            products.Add(product);
            
            _memoryCache.Set("products", products);
            
            return Created($"products/{product.Id}", product);
        }
        
        private async Task<List<ProductModel>> GetAll()
        {
            return await Task.Run(() => _memoryCache.Get<List<ProductModel>>("products"));
        }
    }
}