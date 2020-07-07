using Hktn.Api.Models;
using Hktn.Api.Oauth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hktn.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Get the jwt authentication token by username and password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/auth/token
        ///     {
        ///        "username": "testusername",
        ///        "password": "12345678"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created jwt token.</returns>
        /// <response code="201">A newly created jwt token.</response>
        /// <response code="401">Unauthorized. Username or password does not match with our records.</response>
        [AllowAnonymous]
        public ActionResult<TokenModel> Token([FromBody] GetTokenModel model)
        {
            if (model.Username != "testuser" || model.Password != "1234567")
            {
                return Unauthorized();
            }

            var token = OAuthTokenProvider.GetAccessToken(new TokenOptions
            {
                Identifier = model.Username
            });

            return Created("", new TokenModel
            {
                Token = token
            });
        }
    }
}