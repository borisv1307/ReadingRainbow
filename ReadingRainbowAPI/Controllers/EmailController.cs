using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReadingRainbowAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReadingRainbowAPI.DAL;
using System.Net;
using ReadingRainbowAPI.Middleware;
using System;
using System.Web;
 
namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/email")]
    public class EmailController : Controller
    {
        private readonly PersonRepository _personRepository;
        private readonly ITokenClass _tokenClass;

        public EmailController(PersonRepository personRepository, ITokenClass tokenClass)
        {
            _personRepository = personRepository;
            _tokenClass = tokenClass;
        }
 
        [HttpGet]
        [Route("AddPerson/{token}/{name}")]
        public async Task<IActionResult> ConfirmEmail(string token, string name)
        {

            var decodedUserName = HttpUtility.UrlDecode(name);
            Console.WriteLine($"user name encoded: {name}, user name decoded {decodedUserName}");
            
            var user = await _personRepository.GetPersonAsync(decodedUserName);
            if (user == null)
                return View("~/Views/Error.cshtml");


            Console.WriteLine($"User Compare Token is {user.Token} should match {token}");

            var decodedToken = HttpUtility.UrlDecode(token);

            Console.WriteLine($"User Compare Token is {user.Token} should match decoded token {decodedToken}");

            if (_tokenClass.CompareToken(decodedToken, user.Token) && _tokenClass.CheckDate(user.TokenDate))
            {
                user.EmailConfirmed = "True";
                await _personRepository.UpdatePersonAsync(user);
                return View("~/Views/ConfirmEmail.cshtml"); 
            }
            else
                return View("~/Views/Error.cshtml");          
        }

    }
}