using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;
 
namespace ReadingRainbowAPI.Middleware
{
    public static class TokenClass
    {
        public static string CreateToken()
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = Guid.NewGuid().ToByteArray();
            return (Convert.ToBase64String(time.Concat(key).ToArray()));
        }

        public static bool CompareToken(string sentToken, string userToken)
        {
           // var data = Convert.FromBase64String(sentToken);
           // DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
           // if (when < DateTime.UtcNow.AddHours(-24)) {
           //     return false;
           // }

            if (userToken == sentToken)
            {
                return true;
            }

            return false;
        }

    }
}