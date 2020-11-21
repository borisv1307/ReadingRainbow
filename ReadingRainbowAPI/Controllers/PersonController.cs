using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Relationships;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ReadingRainbowAPI.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/person")]
    public class PersonController : ControllerBase
    {

        private readonly PersonRepository _personRepository;

        private readonly IMapper _mapper;
 
        public PersonController(PersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("Library/{username}")]
        public async Task<ActionResult> GetBooksAsync(string username)
        {
            var person = new Person(){
                Name = username
            };
            var books = await _personRepository.GetInLibraryBookRelationshipAsync(person, new InLibrary());

            return Ok(books);
        }

        [HttpPost]
        [Route("AddUpdatePerson")]
        public async Task<IActionResult> AddUpdatePersonAsync(Person person)
        {
            await _personRepository.AddOrUpdatePersonAsync(person);

            return Ok();
        }

        [HttpGet]
        [Route("Person/{username}")]
        public async Task<IActionResult> FindPersonAsync(string username)
        {
            var person = await _personRepository.GetPersonAsync(username);
            var personDto = _mapper.Map<PersonDto>(person);

            return Ok(personDto);
        }
 
        [HttpGet]
        [Route("People")]
        public async Task<IActionResult> GetAllPeopleAsync()
        {
            var people = (await _personRepository.GetAllPeopleAsync()).ToList();
            var peopleDto = _mapper.Map<List<Person>, List<PersonDto>>(people);

            return Ok(peopleDto);
        }
 
    }
}