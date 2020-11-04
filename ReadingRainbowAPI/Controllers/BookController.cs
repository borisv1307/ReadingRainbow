using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;

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
        // [HttpGet("{id}")]
        [Route("catalog")]
        public async Task<ActionResult> Get(string UserName)
        {
            var result = await _bookRepository.GetAllBooksAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBookAsync(Book book)
        {
            await _bookRepository.AddOrUpdateAsync(book);

            return Ok();
        }

        [HttpGet]
        [Route("FindBook/{id}")]
        public async Task<IActionResult> FindBookAsync(string id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            return Ok(book);
        }
 
        [HttpGet]
        [Route("FindfromNeo")]
        public async Task<IActionResult> GetAsync()
        {
            var bookTitles = (await _bookRepository.GetAllBooksAsync()).ToList();

            return Ok(bookTitles);
        }
 
    }
}