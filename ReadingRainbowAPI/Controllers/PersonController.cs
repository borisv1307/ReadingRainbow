using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Middleware;
using ReadingRainbowAPI.Relationships;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ReadingRainbowAPI.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System;
using System.Text.RegularExpressions;

namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/person")]
    public class PersonController : ControllerBase
    {

        private readonly PersonRepository _personRepository;

        private readonly IMapper _mapper;

        private readonly IEmailHelper _emailHelper;

        private readonly ITokenClass _tokenClass;
 
        public PersonController(PersonRepository personRepository, IMapper mapper, IEmailHelper emailHelper, ITokenClass tokenClass)
        {
            _personRepository = personRepository;
            _mapper = mapper;
            _emailHelper = emailHelper;
            _tokenClass = tokenClass;
        }
        
        [HttpGet]
        [Route("Library/{username}")]
        public async Task<ActionResult> GetBooksAsync(string username)
        {
            var person = new Person(){
                Name = username
            };
            var books = await _personRepository.GetInLibraryBookRelationshipAsync(person, new InLibrary());
            Console.WriteLine($"books {books}" );
            return Ok(JsonSerializer.Serialize(books));
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdatePersonAsync(Person person)
        {
            var success = await _personRepository.UpdatePersonAsync(person);
            
            return Ok(success);
        }

        [HttpGet]
        [Route("Person/{username}")]
        public async Task<IActionResult> FindPersonAsync(string username)
        {
            var person = await _personRepository.GetPersonAsync(username);
            var personDto = _mapper.Map<PersonDto>(person);
            Console.WriteLine($"PersonDto {personDto}" );
            return Ok(JsonSerializer.Serialize(personDto));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string userName, string hashedPassword, string previousPassword) 
        {
            Console.WriteLine($"person {userName}" );

            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok("User was not found");
            }

            var dbPreviousPassword = user.HashedPassword;

            if (user.ChangePassword.ToLower() == "true")
            {
                // Check Date
                if (!_tokenClass.CheckPasswordDate(user.PasswordExpiration))
                {
                    return Ok("Temporary Password has expired");
                }

                // Hash password sent in
                var usersHashedPassword = _tokenClass.HashString(previousPassword);
                //verify against hased password in database
                if (!usersHashedPassword.Equals(dbPreviousPassword))
                {
                    return Ok("Password is incorrect");
                }

            }
            else
            {
                if (!previousPassword.Equals(dbPreviousPassword))
                {
                    return Ok("Password is incorrect");
                }
            }

            // Remove password reset required restriction
            user.ChangePassword = "False";

            // Change password to user's new password
            user.HashedPassword = hashedPassword;

            var success = await _personRepository.UpdatePersonAsync(user);

            if (success)
            {
                // send email informing user of password change
                var resetPassordBody = _emailHelper.ChangePasswordBody(user);
                bool emailResponse = await _emailHelper.SendEmail(user.Name, user.Email, resetPassordBody, 
                    _emailHelper.ChangePasswordSubject());
            }

            Console.WriteLine($"success {success}" );
            return Ok(success);
        }

        
        [AllowAnonymous]
        [HttpPost]
        [Route("AddPerson")]
        public async Task<IActionResult> AddPersonAsync(Person person)
        {
            Console.WriteLine($"person {person.Name}" );

            if (!CheckEmailAddress(person.Email))
            {
                return Ok("Email in incorrect format");
            }

            // Make sure Email Address Does not below to anyone else
            var inUse = await _personRepository.GetPersonByEmailAsync(person.Email);
            if (inUse != null)
            {
                return Ok("Email Address already in Use, select distinct email address");
            }

            person.ChangePassword = "false";

            var success = await _personRepository.AddPersonAsync(person);

            if (success)
            {
                person.Token = _tokenClass.CreateToken();
                person.TokenDate = DateTime.UtcNow.ToString();
                await UpdatePersonAsync(person);

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

                var confirmationLink = _emailHelper.ConfirmationLinkBody(person, callBackUrl);
                bool emailResponse = await _emailHelper.SendEmail(person.Name, person.Email, confirmationLink, 
                    _emailHelper.ConfirmationLinkSubject());

             
                if (emailResponse)
                {
                    Console.WriteLine($"Valid Email Address {person.Email}");
                }
                else
                {
                    Console.WriteLine($"Invalid Email Address {person.Email}");
                    return Ok($"Invalid Email Address {person.Email}");
                }
            }

            Console.WriteLine($"success {success}" );
            return Ok(success);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(Person uEmail)
        {
            // sending person in as an object but all we need it the email
            // this is done so middleware doesn't try to url decode it for it to
            // handle + signs in email addresses correctly
            Console.WriteLine($"User Email {uEmail.Email}" );

            // Find user in database
            var user = await _personRepository.GetPersonByEmailAsync(uEmail.Email);
            if (user == null)
            {
                return Ok("User was not found");
            }

            user.ChangePassword = "true";
            user.PasswordExpiration = DateTime.UtcNow.ToString();

            // Generate and hash temp password, send plain text password
            var pwStr = _tokenClass.GetRandomStr();
            user.HashedPassword = _tokenClass.HashString(pwStr);

            var success = await _personRepository.UpdatePersonAsync(user);
            var passwordExpirationDate = _tokenClass.GetPasswordExpirationDate();

            if (success)
            {
                var resetPasswordBody = _emailHelper.ResetPasswordBody(user, pwStr, passwordExpirationDate);
                bool emailResponse = await _emailHelper.SendEmail(user.Name, user.Email, resetPasswordBody, 
                    _emailHelper.ResetPasswordSubject());
            }

            Console.WriteLine($"success {success}" );
            return Ok(success);
        }

        private bool CheckEmailAddress(string email)
        {          
            string emailRegex = @"^(.+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
      
            Regex re = new Regex(emailRegex);
            if (!re.IsMatch(email))
            {
                Console.WriteLine($"Invalid Email Address {email}");
                return false;
            }
            
            return true;
        }
 
        [HttpGet]
        [Route("People")]
        public async Task<IActionResult> GetAllPeopleAsync()
        {
            var people = (await _personRepository.GetAllPeopleAsync()).ToList();
            var peopleDto = _mapper.Map<List<Person>, List<PersonDto>>(people);

            return Ok(JsonSerializer.Serialize(peopleDto));
        }
    }
}