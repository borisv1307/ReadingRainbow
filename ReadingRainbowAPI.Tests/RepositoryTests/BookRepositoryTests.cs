using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadingRainbowAPI.RepositoryTests
{
    [Collection("Database collection")]
    public class BookRepositoryTests
    {
        DatabaseFixture fixture;

        private BookRepository _bookRepository;
        private PersonRepository _personRepository;

        // Initalize Method used for all tests
        public BookRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _bookRepository = new BookRepository(fixture.dbContext);
            _personRepository = new PersonRepository(fixture.dbContext);
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
   
        private Person CreatePerson()
        {
            var random = new Random();
            var personId = random.Next();

            // Arrange
            return new Person(){
                Name = $"newPerson{personId}",
                Profile =$"This is new person number {personId}",
                Portrait = "https://portraitLink",
                HashedPassword = $"{personId}"
            };
        }

        [Fact]
        public async void GetBookAsync_Test()
        {
            // Arrange
            var book1 = CreateBook();
            var book2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(book1);
            await _bookRepository.AddOrUpdateAsync(book2);

            // Act
            var books = await _bookRepository.GetAllBooksAsync();

            // Assert
            Assert.True(books != null);

            // Clean up
            await _bookRepository.DeleteBookAsync(book1);
            await _bookRepository.DeleteBookAsync(book2);
        }

        [Fact]
        public async void AddBookAsync_Test()
        {
            // Arrange
            var newBook = CreateBook();

            // Act
            await _bookRepository.AddOrUpdateAsync(newBook);
            var returnedBook = await _bookRepository.GetBookAsync(newBook.Id);

            // Assert
            Assert.True(returnedBook != null);
            Assert.True(returnedBook.Title == newBook.Title);

            // Clean up
            await _bookRepository.DeleteBookAsync(newBook);
        }

        [Fact]
        public async void UpdateBookAsync_Test()
        {
            // Arrange
            var random = new Random();
            var newBookIdExt = random.Next();
            var newBookTitle = $"ReplacedBookText {newBookIdExt}";

            var newBook = CreateBook();

            // Act
            await _bookRepository.AddOrUpdateAsync(newBook);
            newBook.Title = newBookTitle;
            await _bookRepository.AddOrUpdateAsync(newBook);

            var returnedBook = await _bookRepository.GetBookAsync(newBook.Id);

            // Assert
            Assert.True(returnedBook != null);
            Assert.True(returnedBook.Title == newBookTitle);

            // Clean up
            await _bookRepository.DeleteBookAsync(newBook);
        }

        [Fact]
        public async void GetBookWhereAsync_Test()
        {
            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);

            var searchText = newBook.Description.Substring(0, (newBook.Description.Length/2));

            // Act
            var returnedBookList = new List<Book>(await _bookRepository.GetBooksWhereAsync(searchText));

            // Assert
            Assert.True(returnedBookList.Count != 0);
            Assert.True(returnedBookList.Where(b=>b.Description.Contains(searchText)).ToList().Count == returnedBookList.Count);

            // Clean up
            await _bookRepository.DeleteBookAsync(newBook);
        }

        [Fact]
        public async void DeleteBookAsync_Test()
        {
            // Arrange
            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);
            
            // Act
            await _bookRepository.DeleteBookAsync(newBook);
            var returnedBook = await _bookRepository.GetBookAsync(newBook.Id);

            // Assert
            Assert.True(returnedBook == null);
        }

        [Fact]
        public async void CreateInLibraryRelationship_Test()
        {
            // Arrange
            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);

            var newPerson = CreatePerson();
            await _personRepository.AddOrUpdatePersonAsync(newPerson);

            var inLibrary = new Relationships.InLibrary();
            
            // Act
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook, newPerson, inLibrary);
            var returnedPerson = (await _bookRepository.GetInLibraryPersonRelationshipAsync(newBook, inLibrary))
                .ToList().FirstOrDefault();

            // Assert
            Assert.True(newPerson.Name == returnedPerson.Name);

            // Clean up
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook, newPerson, inLibrary);
            await _bookRepository.DeleteBookAsync(newBook);
            await _personRepository.DeletePersonAsync(newPerson);
        }
    }
}