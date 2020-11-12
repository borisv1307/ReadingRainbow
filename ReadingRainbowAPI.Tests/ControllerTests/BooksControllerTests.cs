using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ReadingRainbowAPI.ControllerTests
{
    [Collection("Database collection")]
    public class BookControllerTests
    {
        DatabaseFixture fixture;

        private BookController _bookController;

        private PersonRepository _personRepository;
        private BookRepository _bookRepository;

        // Initalize Method used for all tests
        public BookControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _bookRepository = new BookRepository(fixture.dbContext);
            _personRepository = new PersonRepository(fixture.dbContext);

            _bookController = new BookController(_bookRepository);
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
                Portrait = "Https://PortaitLink",
                HashedPassword = $"{personId}"

            };
        }

        [Fact]
        public async void GetLibraryRoute_Test()
        {   
            // Arrange
            var book1 = CreateBook();
            var person1 = CreatePerson();
            var person2 = CreatePerson();

            await _bookRepository.AddOrUpdateAsync(book1);
            await _personRepository.AddOrUpdatePersonAsync(person1);
            await _personRepository.AddOrUpdatePersonAsync(person2);

            var inLibrary1 = new InLibrary();
            var inLibrary2 = new InLibrary();

            await _bookRepository.CreateInLibraryRelationshipAsync(book1, person1, inLibrary1);
            await _bookRepository.CreateInLibraryRelationshipAsync(book1, person2, inLibrary2);

            // Act

            var result = await _bookController.GetPeopleAsync(book1.Id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Clean up
            await _bookRepository.DeleteInLibraryRelationshipAsync(book1, person1, inLibrary1);
            await _bookRepository.DeleteInLibraryRelationshipAsync(book1, person2, inLibrary2);
            await _bookRepository.DeleteBookAsync(book1);
            await _personRepository.DeletePersonAsync(person1);
            await _personRepository.DeletePersonAsync(person2);
            
        }

        [Fact]
        public async void AddBookRoute_Test()
        {
            // Arrange
            var newBook = CreateBook();

            // Act
            await _bookController.AddUpdateBookAsync(newBook);
            var returnedBook = await _bookRepository.GetBookAsync(newBook.Id);

            // Assert
            Assert.True(returnedBook != null);
            Assert.True(returnedBook.Title == newBook.Title);

            // Clean up
            await _bookRepository.DeleteBookAsync(newBook);
        }

        [Fact]
        public async void FindBookRoute_Test()
        {
            // Arrange
            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);

            // Act
            var result = await _bookController.FindBookAsync(newBook.Id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

                        // Clean up
            await _bookRepository.DeleteBookAsync(newBook);
        }

        [Fact]
        public async void GetAllBookRoute_Test()
        {
            var book1 = CreateBook();
            var book2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(book1);
            await _bookRepository.AddOrUpdateAsync(book2);

            // Act
            var result = await _bookController.GetAllBooksAsync();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Clean up
            await _bookRepository.DeleteBookAsync(book1);
            await _bookRepository.DeleteBookAsync(book2);
           
        }

    }
}
