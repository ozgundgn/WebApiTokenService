using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private static IConfiguration _config;
        private static AccessToken _accessToken;
        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        NorthwindContext context = new NorthwindContext();

        [HttpGet("getall")]
        [Authorize]
        public List<User> GetAll()
        {
            return context.Set<User>().ToList();
        }

        public User GetSingle(int id) => context.Users.FirstOrDefault(u => u.Id == id);

        public IActionResult Post(User user)
        {
            if (user != null)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        public IActionResult Put(User user)
        {
            if (user != null && user.Id != 0)
            {
                var oldUser = context.Users.FirstOrDefault(u => u.Id == user.Id);

                typeof(User).GetProperties().ToList().ForEach(p =>
                {
                    if (p.Name != "Id" && (p.Name == "FirstName" || p.Name == "LastName"))
                    {
                        p.SetValue(oldUser, typeof(User).GetProperty(p.Name).GetValue(user));
                    }

                    context.SaveChanges();
                });
                return Ok();
            }
            return Ok("Hata oluştu!");
        }

        public IActionResult Delete(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                context.Users.Remove(user);
                return Ok("Silme işlemi başarılı!");
            }

            return Ok("Silinecek veri bulunamadı!");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var result = new UnauthorizedResult();
            if (user != null && user.FirstName == "Ozgun" && user.LastName == "Dogan")
            {
                var accessToken = GenerateToken();
                return Ok(accessToken);
            }

            return result;
        }

        public AccessToken GenerateToken()
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
