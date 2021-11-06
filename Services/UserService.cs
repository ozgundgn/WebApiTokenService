using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using DataAccess;
using Models.Results;

namespace Services
{
    public static class UserService
    {
        public static string SetToken
        {
            set => Client.SetToken = value;
        }
        public static DataResult<List<User>> GetAll()
        {
            var result = WebService.GetAll("getall").Result;

            if (!string.IsNullOrEmpty(result))
            {
                return new DataResult<List<User>>()
                {
                    Success = true,
                    Data = JsonConvert.DeserializeObject<List<User>>(result)
                };
            }

            return new DataResult<List<User>>() { Success = false };
        }

        public static DataResult<AccessToken> GetToken(User user)
        {
            DataResult<AccessToken> tokenResult = new DataResult<AccessToken>();
            var result = WebService.GetToken(user).Result;
            if (!string.IsNullOrEmpty(result))
            {
                tokenResult.Success = true;
                tokenResult.Data = JsonConvert.DeserializeObject<AccessToken>(result);
            }

            return tokenResult;
        }
    }
}
