using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReadingRainbowAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReadingRainbowAPI.DAL;
using System.Net;
using ReadingRainbowAPI.Middleware;
 
namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/email")]
    public class EmailController : Controller
    {
        private readonly PersonRepository _personRepository;

        public EmailController(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
 
        [HttpGet]
        [Route("AddPerson/{token}/{name}")]
        public async Task<IActionResult> ConfirmEmail(string token, string name)
        {

            var decodedUserName = WebUtility.UrlDecode(name);

            var user = await _personRepository.GetPersonAsync(decodedUserName);
            if (user == null)
                return View("~/Views/Error.cshtml");

            var decodedToken = WebUtility.UrlDecode(token);
            if (TokenClass.CompareToken(decodedToken, user.Token))
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