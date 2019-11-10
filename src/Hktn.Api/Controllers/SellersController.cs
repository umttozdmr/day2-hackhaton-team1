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
    public class SellersController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public SellersController(IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<SellerModel>>> Get()
        {
            var result = await Task.Run(GetAll);
            
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<List<SellerModel>>> Get([FromRoute] int id)
        {
            return Ok((await GetAll()).First(s => s.Id == id));
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] SellerModel seller)
        {
            var sellers = _memoryCache.Get<List<SellerModel>>("sellers") ?? new List<SellerModel>();
            sellers.Add(seller);
            
            _memoryCache.Set("sellers", sellers);
            
            return Created($"sellers/{seller.Id}", seller);
        }
        
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProducts([FromRoute] int id)
        {
            var client = _httpClientFactory.CreateClient();

            var result = await client.GetAsync($"http://product/products?sellerId={id}");

            return Ok(await result.Content.ReadAsStringAsync());
        }

        private async Task<List<SellerModel>> GetAll()
        {
            return await Task.Run(() => _memoryCache.Get<List<SellerModel>>("sellers"));
        }
    }
}