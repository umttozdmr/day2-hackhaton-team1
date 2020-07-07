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
    public class SellersController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellersController(IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
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
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization,
                _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString());
            
            var result = await client.GetAsync($"http://product/products?sellerId={id}");

            return Ok(await result.Content.ReadAsStringAsync());
        }

        private async Task<List<SellerModel>> GetAll()
        {
            return await Task.Run(() => _memoryCache.Get<List<SellerModel>>("sellers"));
        }
    }
}