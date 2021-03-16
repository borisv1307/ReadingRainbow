
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
using System.Text.Json;

namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/book")]
    public class BookController : ControllerBase
    {

        private readonly BookRepository _bookRepository;
        private readonly GenreRepository _genreRepository;

        private readonly IMapper _mapper;
 
        public BookController(BookRepository bookRepository, GenreRepository genreRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("Library/{bookId}")]
        public async Task<IActionResult> GetPeopleAsync(string bookId)
        {
            var book = new Book(){
                Id = bookId
            };
            var people = (await _bookRepository.GetInLibraryPersonRelationshipAsync(book, new InLibrary())).ToList();
            var peopleDto = _mapper.Map<List<Person>, List<PersonDto>>(people);

            return Ok(JsonSerializer.Serialize(peopleDto));
        }

        [HttpPost]
        [Route("AddUpdateBook")]
        public async Task<IActionResult> AddUpdateBookAsync(Book book)
        {
            await _bookRepository.AddOrUpdateAsync(book);

            foreach(var genre in book.Genres)
            {
                await _genreRepository.CreateInGenreRelationshipAsync(genre, book, new Relationships.InGenre());
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddBookToLibrary/{userName}")]
        public async Task<IActionResult> AddBookToLibrary(string userName, Book book)
        {
            await _bookRepository.AddOrUpdateAsync(book);

            foreach(var genre in book.Genres)
            {
                await _genreRepository.CreateInGenreRelationshipAsync(genre, book, new Relationships.InGenre());
            }

            await _bookRepository.CreateInLibraryRelationshipAsync(book, new Person() {Name = userName}, new InLibrary());

            return Ok();
        }

        [HttpPost]
        [Route("AddBooksToWishList/{userName}")]
        public async Task<IActionResult> AddBooksToWishList(string userName, Book book)
        {
            await _bookRepository.AddOrUpdateAsync(book);

            foreach(var genre in book.Genres)
            {
                await _genreRepository.CreateInGenreRelationshipAsync(genre, book, new Relationships.InGenre());
            }
            
            await _bookRepository.CreateWishlistRelationshipAsync(book, new Person() {Name = userName}, new WishList());

            return Ok();
        }

        [HttpPost]
        [Route("RemoveBookFromLibrary/{userName}")]
        public async Task<IActionResult> RemoveBookFromLibrary(string userName, Book book)
        {

            await _bookRepository.DeleteInLibraryRelationshipAsync(book, new Person() {Name = userName}, new InLibrary());

            return Ok();
        }

        [HttpPost]
        [Route("RemoveBookFromWishList/{userName}")]
        public async Task<IActionResult> RemoveBookFromWishList(string userName, Book book)
        {
            
            await _bookRepository.DeleteWishListRelationshipAsync(book, new Person() {Name = userName}, new WishList());

            return Ok();
        }

        [HttpGet]
        [Route("Book/{id}")]
        public async Task<IActionResult> FindBookAsync(string id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            // Get All Genre's associated with book
            book.Genres = (await _bookRepository.GetInGenreBookRelationshipAsync(book, new Relationships.InGenre())).ToList();

            return Ok(JsonSerializer.Serialize(book));
        }
 
        [HttpGet]
        [Route("Books")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var bookTitles = await _bookRepository.GetAllBooksAsync();

            return Ok(JsonSerializer.Serialize(bookTitles));
        }
 
    }
}