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
using Services;

namespace WebAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private static IConfiguration _config;
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
        [HttpGet]
        public User GetSingle(int id) => context.Set<User>().ToList().FirstOrDefault(x => x.Id == id);

        [HttpPost("post")]
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
        [HttpPut]
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
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var user = context.Users.FirstOrDefault(x=>x.Id==id);

            if (user != null)
            {
                var a=context.Remove(user);
                context.SaveChanges();
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
                var accessToken = GenerateToken.CreateToken();
                return Ok(accessToken);
            }

            return result;
        }



    }
}
