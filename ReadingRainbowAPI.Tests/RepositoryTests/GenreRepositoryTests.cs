using System;
using Xunit;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadingRainbowAPI.RepositoryTests
{
    [Collection("Database collection")]
    public class GenreRepositoryTests
    {
        DatabaseFixture fixture;

        private GenreRepository _genreRepository;
        private BookRepository _bookRepository;

        // Initalize Method used for all tests
        public GenreRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _bookRepository = new BookRepository(fixture.dbContext);
            _genreRepository = new GenreRepository(fixture.dbContext);
        }

        private Book CreateBook()
        {
            var random = new Random();
            var bookIdExt = random.Next();

            // Arrange
            return new Book(){
                Id = $"xbn56r{bookIdExt}",
                Title =$"Test Book Title {bookIdExt}",
                PublishDate  = DateTime.Now.ToShortDateString(),
                NumberPages  = $"{bookIdExt}",
                Description  = "Test Book Description",
            };
        }

        private Genre CreateGenre()
        {
            var random = new Random();
            var genreIdExt = random.Next();

            // Arrange
            return new Genre(){
                Name = $"GenreName{genreIdExt}",
                Description =$"Test Genre Description for Genre Name Ending with {genreIdExt}"
            };
        } 

       //  public async Task CreateInGenreRelationshipAsync(Genre genre, Book book, InGenre inGenre)

      //  public async Task<IEnumerable<Book>> GetInGenreBookRelationshipAsync(Genre genre, InGenre inGenre)

      //  public async Task DeleteInGenreRelationshipAsync(Genre genre, Book book, InGenre inGenre)

        [Fact]
        public async void AddGenreAsync_Test()
        {
            // Arrange
            var newGenre = CreateGenre();

            // Act
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);
            var returnGenre = await _genreRepository.GetGenreAsync(newGenre.Name);

            // Assert
            Assert.True(returnGenre != null);
            Assert.True(returnGenre.Description == newGenre.Description);

            // Clean up
            await _genreRepository.DeleteGenreAsync(newGenre);
        }

        [Fact]
        public async void UpdateGenreAsync_Test()
        {
            // Arrange
            var random = new Random();
            var newGenreIdExt = random.Next();
            var newGenreDescription = $"ReplacedGenreText {newGenreIdExt}";

            var newGenre = CreateGenre();

            // Act
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);
            newGenre.Description = newGenreDescription;
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);

            var returnedGenre = await _genreRepository.GetGenreAsync(newGenre.Name);

            // Assert
            Assert.True(returnedGenre != null);
            Assert.True(returnedGenre.Description == newGenreDescription);

            // Clean up
            await _genreRepository.DeleteGenreAsync(newGenre);
        }

        [Fact]
        public async void GetGenreWhereAsync_Test()
        {
            var newGenre = CreateGenre();
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);

            var searchText = newGenre.Description.Substring(0, (newGenre.Description.Length/2));

            // Act
            var returnedGenreList = new List<Genre>(await _genreRepository.GetGenresWhereAsync(searchText));

            // Assert
            Assert.True(returnedGenreList.Count != 0);
            Assert.True(returnedGenreList.Where(b=>b.Description.Contains(searchText)).ToList().Count == returnedGenreList.Count);

            // Clean up
            await _genreRepository.DeleteGenreAsync(newGenre);
        }

        [Fact]
        public async void DeleteBookAsync_Test()
        {
            // Arrange
            var newGenre = CreateGenre();
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);
            
            // Act
            await _genreRepository.DeleteGenreAsync(newGenre);
            var returnedGenre = await _genreRepository.GetGenreAsync(newGenre.Name);

            // Assert
            Assert.True(returnedGenre == null);
        }

        [Fact]
        public async void CreateInGenreRelationship_Test()
        {
            // Arrange
            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);

            var newGenre = CreateGenre();
            await _genreRepository.AddOrUpdateGenreAsync(newGenre);

            var inGenre = new Relationships.InGenre();
            
            // Act
            await _genreRepository.CreateInGenreRelationshipAsync(newGenre, newBook, inGenre);
            var returnedBook = (await _genreRepository.GetInGenreBookRelationshipAsync(newGenre, inGenre))
                .ToList().FirstOrDefault();

            // Assert
            Assert.True(newBook.Title == returnedBook.Title);

            // Clean up
            await _genreRepository.DeleteInGenreRelationshipAsync(newGenre, newBook, inGenre);
            await _bookRepository.DeleteBookAsync(newBook);
            await _genreRepository.DeleteGenreAsync(newGenre);
        }

        [Fact]
        public async void RemoveInGenreAsync_Test()
        {
            var genre = CreateGenre();
            var booktostay1 = CreateBook();
            var booktostay2 = CreateBook();
            var booktogo = CreateBook();

            await _genreRepository.AddOrUpdateGenreAsync(genre);
            await _bookRepository.AddOrUpdateAsync(booktostay1);
            await _bookRepository.AddOrUpdateAsync(booktostay2);
            await _bookRepository.AddOrUpdateAsync(booktogo);

            var inGenre = new Relationships.InGenre();

            await _genreRepository.CreateInGenreRelationshipAsync(genre, booktostay1, inGenre);
            await _genreRepository.CreateInGenreRelationshipAsync(genre, booktostay2, inGenre);
            await _genreRepository.CreateInGenreRelationshipAsync(genre, booktogo, inGenre);

            // Act
            var allBooksInList = await _genreRepository.GetInGenreBookRelationshipAsync(genre, inGenre);
            await _genreRepository.DeleteInGenreRelationshipAsync(genre, booktogo, inGenre);
            var twoBooksInList = await _genreRepository.GetInGenreBookRelationshipAsync(genre, inGenre);

            // Assert
            Assert.True(allBooksInList.Where(b=> b.Id == booktostay1.Id).ToList().Count == 1);
            Assert.True(allBooksInList.Where(b=> b.Id == booktostay2.Id).ToList().Count == 1);
            Assert.True(allBooksInList.Where(b=> b.Id == booktogo.Id).ToList().Count == 1);
            Assert.True(allBooksInList.ToList().Count == 3);

            Assert.True(twoBooksInList.Where(b=> b.Id == booktostay1.Id).ToList().Count == 1);
            Assert.True(twoBooksInList.Where(b=> b.Id == booktostay2.Id).ToList().Count == 1);
            Assert.True(twoBooksInList.Where(b=> b.Id == booktogo.Id).ToList().Count == 0);
            Assert.True(twoBooksInList.ToList().Count == 2);

            // CleanUp
            await _genreRepository.DeleteInGenreRelationshipAsync(genre, booktostay1, inGenre);
            await _genreRepository.DeleteInGenreRelationshipAsync(genre, booktostay2, inGenre);
            await _genreRepository.DeleteGenreAsync(genre);
            await _bookRepository.DeleteBookAsync(booktostay1);
            await _bookRepository.DeleteBookAsync(booktostay2);
            await _bookRepository.DeleteBookAsync(booktogo);
        }

                
        [Fact]
        public async void GetInGenreBooksAsync_Test()
        {
            // Arrange
            var genre = CreateGenre();
            var book = CreateBook();
            var inGenre = new Relationships.InGenre();

            await _genreRepository.AddOrUpdateGenreAsync(genre);
            await _bookRepository.AddOrUpdateAsync(book);
            await _genreRepository.CreateInGenreRelationshipAsync(genre, book, inGenre);

            // Act
            var genreBooks = await _genreRepository.GetInGenreBookRelationshipAsync(genre, inGenre);

            // Assert
            Assert.True(genreBooks.Where(b=> b.Id == book.Id).ToList().Count == 1);
            Assert.True(genreBooks.ToList().Count == 1);

            // Clean up
            await _genreRepository.DeleteInGenreRelationshipAsync(genre, book, inGenre);
            await _genreRepository.DeleteGenreAsync(genre);
            await _bookRepository.DeleteBookAsync(book);
        }
    }
}