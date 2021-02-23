using ReadingRainbowAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadingRainbowAPI.DAL;
using System.Threading.Tasks;
using ReadingRainbowAPI.Middleware;
using System;


namespace ReadingRainbowAPI.Controllers
{
    
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private PersonRepository _personRepository;
        private IEmailHelper _emailHelper;

        public TokenController(IConfiguration config, PersonRepository personRepository, IEmailHelper emailHelper)
        {
            _config = config;
            _personRepository = personRepository;
            _emailHelper = emailHelper;
        }

        [HttpGet]
        public async Task<ActionResult> GetRandomToken(string username, string password)
        {
            var person = await _personRepository.GetPersonAsync(username);

            if (person == null || string.IsNullOrEmpty(person.HashedPassword)) 
            {
                return Ok();  
            }

            if (person.EmailConfirmed.ToLower() == "false")
            {
                return Ok("Confirm Email Address");
            }

            if (person.HashedPassword.Equals(password))
            {
                var jwt = new JwtService(_config);
                var token = jwt.GenerateSecurityToken(username);
                return Ok(token);
            }

                return Ok();         
        }
        
        [HttpPost]
        [Route("ReSendEmail")]
        public async Task<IActionResult> ResendEmail(string username, string password)
        {
            var person = await _personRepository.GetPersonAsync(username);

            if (person == null || string.IsNullOrEmpty(person.HashedPassword)) 
            {
                return Ok();  
            }

           // var token = TokenClass.CreateToken();
            person.Token = TokenClass.CreateToken();
            await _personRepository.UpdatePersonAsync(person);

            var callBackUrl = String.Empty;
            
            try
            {
                callBackUrl = Url.Action("ConfirmEmail", "Email");
                Console.WriteLine($"We got here with a url value of {callBackUrl}");
            } 
            catch (Exception ex)
            { 
                Console.WriteLine($"Exception occured when generating link for email {ex}");
            }

            var confirmationLink = _emailHelper.GenerateEmailLink(person, callBackUrl);       
            bool emailResponse = await _emailHelper.SendEmail(person.Name, person.Email, confirmationLink);
             
            if (emailResponse)
            {
                Console.WriteLine($"Valid Email Address {person.Email}");
            }
            else
            {
                Console.WriteLine($"Invalid Email Address {person.Email}");
                return Ok($"Invalid Email Address {person.Email}");
            }

            Console.WriteLine($"success");
            return Ok("success");
        }
    }
}
