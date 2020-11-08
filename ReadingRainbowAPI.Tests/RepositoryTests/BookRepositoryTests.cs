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
            var books = await _bookRepository.GetAllBooksAsync();
            Assert.True(books != null);
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
            
            // Act
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook, newPerson, new Relationships.InLibrary());
            var returnedPerson = (await _bookRepository.GetInLibraryPersonRelationshipAsync(newBook, new Relationships.InLibrary()))
                .ToList().FirstOrDefault();

            // Assert
            Assert.True(newPerson.Name == returnedPerson.Name);
        }
    }
}