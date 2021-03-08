using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;
using System.Buffers.Text;
using Microsoft.Extensions.Configuration;  
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

 
namespace ReadingRainbowAPI.Middleware
{
    public interface ITokenClass
    {
        string CreateToken();
        bool CompareToken(string sentToken, string userToken);
        bool CheckTokenDate(string tokenDate);
        bool CheckPasswordDate(string passwordDate);
        string HashString(string str);
        string GetRandomStr();
        string GetPasswordExpirationDate();
      
    }
    public class TokenClass : ITokenClass
    {
        private double _tokenExpirationTime;
        private double _passwordExpirationTime;

        public TokenClass(IConfiguration config)
        {
            _tokenExpirationTime = Convert.ToDouble(config.GetSection("Email").GetSection("tokenHours").Value);   
            _passwordExpirationTime = Convert.ToDouble(config.GetSection("PasswordReset").GetSection("passwordHours").Value);   
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

        public bool CheckTokenDate(string tokenDate)
        {
            try{
                DateTime when = Convert.ToDateTime(tokenDate);
                if (when < DateTime.UtcNow.AddHours(_tokenExpirationTime)) {
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

        public bool CheckPasswordDate(string passwordDate)
        {
            try{
                DateTime when = Convert.ToDateTime(passwordDate);
                if (when < DateTime.UtcNow.AddHours(_passwordExpirationTime)) {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Checking password date: {ex} with date value {passwordDate}");
                return false;
            }

            return true;
        }

        public string GetPasswordExpirationDate()
        {
            return (-1 *_passwordExpirationTime).ToString();
        }

        public string HashString(string str)
        {
            return hasher(str);
        }

        public string GetRandomStr()
        {
            byte[] pwrange = new byte[128 / 16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(pwrange);
            }
            var rand = Convert.ToBase64String(pwrange);

            Console.WriteLine($"password: {rand}");

            return rand;
        }

        private string hasher(string inputStr)
        {
            var salt = Encoding.Unicode.GetBytes("DWWf5N37drcSD17RYM3Msw==");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputStr,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");

            return hashed;
        }
    }
}