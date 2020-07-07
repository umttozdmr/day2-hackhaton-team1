using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Hktn.Api.Oauth
{
    /// <summary>
    /// OAuthTokenProvider
    /// </summary>
    public class OAuthTokenProvider
    {
        /// <summary>
        /// To securing bearer tokens.
        /// </summary>
        public static string JwtSecureKey = "11111-1111-1111-11111-111111111";

        private const string ISSUER = "hktn-api";
        private const string ROLE = "general";

        /// <summary>
        /// Get user access token.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetAccessToken(TokenOptions options)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecureKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, options.Identifier, null, ISSUER),
                new Claim(ClaimTypes.Role, ROLE)
            };

            var token = new JwtSecurityToken(claims: claims,
                expires: new SystemClock().UtcNow.Add(TimeSpan.FromDays(900)).DateTime,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return $"Bearer {accessToken}";
        }
    }
}