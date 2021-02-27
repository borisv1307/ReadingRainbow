using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;
using System.Buffers.Text;
using Microsoft.Extensions.Configuration;  

 
namespace ReadingRainbowAPI.Middleware
{
    public interface ITokenClass
    {
        string CreateToken();
        bool CompareToken(string sentToken, string userToken);
        bool CheckDate(string tokenDate);
      
    }
    public class TokenClass : ITokenClass
    {
        private double _expirationTime;

        public TokenClass(IConfiguration config)
        {
            _expirationTime = Convert.ToDouble(config.GetSection("Email").GetSection("tokenHours").Value);   
        }

        public string CreateToken()
        {
            var key = Guid.NewGuid().ToByteArray();
            var keyStr = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "-")
                .Replace("+", "_")
                .Replace("=", "");

            return keyStr;
        }

        public bool CompareToken(string sentToken, string userToken)
        {
            if (userToken == sentToken)
            {
                return true;
            }

            return false;
        }

        public bool CheckDate(string tokenDate)
        {
            try{
                DateTime when = Convert.ToDateTime(tokenDate);
                if (when < DateTime.UtcNow.AddHours(_expirationTime)) {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Checking token date: {ex} with date value {tokenDate}");
                return false;
            }

            return true;
        }
    }
}