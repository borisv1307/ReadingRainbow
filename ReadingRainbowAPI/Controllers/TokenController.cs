using ReadingRainbowAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadingRainbowAPI.DAL;
using System.Threading.Tasks;
using System;


namespace ReadingRainbowAPI.Controllers
{
    
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private PersonRepository _personRepository;

        public TokenController(IConfiguration config, PersonRepository personRepository)
        {
            _config = config;
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetRandomToken(string username, string password)
        {
            var person = await _personRepository.GetPersonAsync(username);

            if (person == null || string.IsNullOrEmpty(person.HashedPassword)) 
            {
                return Ok();  
            }

            if (person.HashedPassword.Equals(password))
            {
                var jwt = new JwtService(_config);
                var token = jwt.GenerateSecurityToken(username);
                return Ok(token);
            }

                return Ok();         
        }
    }
}
