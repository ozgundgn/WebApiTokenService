using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Services
{
    public static class GenerateToken
    {
        private static AccessToken _accessToken;
        public static IConfiguration AppSetting { get; }

        static GenerateToken()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static AccessToken CreateToken()
        {
            if (_accessToken == null)
            {
                JwtOptions _jwtOptions = AppSetting.GetSection("JWT").Get<JwtOptions>();
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
                var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience,
                    notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpiration),
                    signingCredentials: signinCredentials);

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


