
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Relationships;

namespace ReadingRainbowAPI.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BookRepository _bookRepository;
 
        public BookController(INeo4jDBContext context)
        {
            _bookRepository = new BookRepository(context);
        }
        
        [HttpGet]
        [Route("Library/{bookId}")]
        public async Task<ActionResult> GetPeopleAsync(string bookId)
        {
            var book = new Book(){
                Id = bookId
            };
            var people = await _bookRepository.GetInLibraryPersonRelationshipAsync(book, new InLibrary());

            return Ok(people);
        }

        [HttpPost]
        [Route("AddUpdateBook")]
        public async Task<IActionResult> AddUpdateBookAsync(Book book)
        {
            await _bookRepository.AddOrUpdateAsync(book);

            return Ok();
        }

        [HttpGet]
        [Route("Book/{id}")]
        public async Task<IActionResult> FindBookAsync(string id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            return Ok(book);
        }
 
        [HttpGet]
        [Route("Books")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var bookTitles = await _bookRepository.GetAllBooksAsync();

            return Ok(bookTitles);
        }
 
    }
}