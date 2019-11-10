using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hktn.Api.Controllers
{
    [ApiController]
    public class StatusController : ControllerBase
    {
        /// <summary>
        ///     The application is healthy or not.
        /// </summary>
        /// <returns>Build and environment specific object.</returns>
        [HttpGet("healthz")]
        public async Task<IActionResult> HealthzAsync()
        {
            return await Task.FromResult(Ok());
        }
    }
}