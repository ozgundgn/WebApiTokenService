using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace WebAPIApp
{
    public class TokenHelper
    {
        private static IConfiguration _config;
        private static AccessToken _accessToken;
        private TokenHelper(IConfiguration config)
        {
            _config = config;
        }
        public  AccessToken GenerateToken()
        {
            if (_accessToken == null)
            {
                JwtOptions _jwtOptions = _config.GetSection("JWT").Get<JwtOptions>();
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
                var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpiration), signingCredentials: signinCredentials);

                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
                _accessToken = new AccessToken()
                {
                    Token = tokenHandler,
                    Expiration = DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpiration)
                };
            }

            return _accessToken;
        }
    }
}
