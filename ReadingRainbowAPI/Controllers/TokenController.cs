using ReadingRainbowAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthTest.API.Controllers
{
    
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public string GetRandomToken(string email)
        {
            var jwt = new JwtService(_config);
            var token = jwt.GenerateSecurityToken(email);
            return token;
        }
    }
}
