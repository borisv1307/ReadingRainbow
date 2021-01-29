using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReadingRainbowAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReadingRainbowAPI.DAL;
using System;
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
            var user = await _personRepository.GetPersonAsync(name);
            if (user == null)
                return View("~/Views/Error.cshtml");

            if (TokenClass.CompareToken(token, user.Token))
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