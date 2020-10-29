using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingRainbowAPI.Models;
using Neo4j.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;
using Neo4jClient;
using ReadingRainbowAPI.DAL;

namespace ReadingRainbowAPI.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BookRepository _bookRepository;
        private readonly IDriver _driver;
        private readonly IGraphClient _client;
 
        public BookController(BookRepository bookRepository)
        {
            // _client = client;

            _bookRepository = bookRepository;
        }
        
        [HttpGet]
        // [HttpGet("{id}")]
       // public async Task<ActionResult,book.> GetAsync(string id)
        [Route("catalog")]
        public async Task<ActionResult> Get(string UserName)
        {

            await _bookRepository.Single(p => p.Id == book.Id && p.Title == book.Title);

            var result = await _client.Cypher
                .Match(@"(book: Book { title: 'Test' }) -
                [:BELONGS_TO]->(userProfile: UserProfile)")
                .Return ((book, userProfile) => new{
                    Book = book.As<Book>(),
                    UserProfile = userProfile.CollectAs<Person>()
                }).ResultsAsync;
                  
            return Ok(result);
        }
 
        [HttpGet]
        [Route("FindfromNeo")]
        public async Task<IActionResult> GetAsync()
        {
            IResultCursor cursor;
            var bookTitles = new List<Book>();
            IAsyncSession session = _driver.AsyncSession();          

            try
             {

                cursor  = await session.RunAsync(@"MATCH (b:Book) RETURN b.title as Title, 
                    b.thumbnail as Thumbnail, b.smallThumbnail as SmallThumbnail,
                    b.publishDate as PublishDate, b.numberPages as NumberPages,
                    b.description as Description, b.isbn_10 as ISBN_10, b.isbn_13 as ISBN_13
                    Limit 10");  
    
                bookTitles = await cursor.ToListAsync<Book>(record =>  new Book {
                    Title = record["Title"].As<string>(),
                    // Authors.Add(record["Authors"].As<List<string>>()),
                    Thumbnail= record["Thumbnail"].As<string>(),
                    SmallThumbnail = record["SmallThumbnail"].As<string>(),
                    PublishDate= record["PublishDate"].As<string>(),
                    NumberPages= record["NumberPages"].As<string>(),
                    Description= record["Description"].As<string>(),
                    ISBN_10= record["ISBN_10"].As<string>(),      
                    ISBN_13 = record["ISBN_13"].As<string>(), 
                    // ISBN_Other = record["ISBN_Other"].As<string>(), 
                    // Cateogries.Add(record["Categories"].As<List<string>>())
                  });

                //await session.RunAsync(@"CREATE (a:Person {name:'Arthur', title:'King'})");
                //cursor = await session.RunAsync(@"MATCH (a:Person) WHERE a.name = 'Arthur' RETURN a.name AS name, a.title AS title");

                //newPerson = await cursor.ToListAsync (record =>
                //record["title"].As<string>()); 

            }
             finally{
                 await session.CloseAsync();
             }

             // var booksjson = JsonSerializer.Serialize(bookTitles);

            return Ok(bookTitles);
        }
 
    }
}