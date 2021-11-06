using Services;
using System;
using System.Linq;
using System.Threading.Channels;
using Models;
using Newtonsoft.Json;

namespace Console.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = UserService.GetToken(new User(){FirstName = "Ozgun",LastName = "Dogan"}).Data; 
            UserService.SetToken = token.Token;

            var result = UserService.GetAll();

            if (result.Success)
                result.Data.ForEach(x => System.Console.WriteLine(x.FirstName));
        }
    }
}
