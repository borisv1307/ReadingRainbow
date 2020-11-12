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
    public class PeopleControllerTests
    {
        DatabaseFixture fixture;

        private PersonController _personController;

        private PersonRepository _personRepository;
        private BookRepository _bookRepository;

        // Initalize Method used for all tests
        public PeopleControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _bookRepository = new BookRepository(fixture.dbContext);
            _personRepository = new PersonRepository(fixture.dbContext);

            _personController = new PersonController(_personRepository);
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
        public async void GetPersonLibraryRoute_Test()
        {   
            // Arrange
            var book1 = CreateBook();
            var book2 = CreateBook();
            var person1 = CreatePerson();

            await _bookRepository.AddOrUpdateAsync(book1);
            await _bookRepository.AddOrUpdateAsync(book2);
            await _personRepository.AddOrUpdatePersonAsync(person1);

            var inLibrary1 = new InLibrary();
            var inLibrary2 = new InLibrary();

            await _bookRepository.CreateInLibraryRelationshipAsync(book1, person1, inLibrary1);
            await _bookRepository.CreateInLibraryRelationshipAsync(book2, person1, inLibrary2);

            // Act

            var result = await _personController.GetBooksAsync(person1.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

             // Clean up
            await _bookRepository.DeleteInLibraryRelationshipAsync(book1, person1, inLibrary1);
            await _bookRepository.DeleteInLibraryRelationshipAsync(book2, person1, inLibrary2);
            await _bookRepository.DeleteBookAsync(book1);
            await _bookRepository.DeleteBookAsync(book2);
            await _personRepository.DeletePersonAsync(person1);
        }

        [Fact]
        public async void AddPersonRoute_Test()
        {
            // Arrange
            var newPerson = CreatePerson();

            // Act
            await _personController.AddUpdatePersonAsync(newPerson);
            var returnedPerson = await _personRepository.GetPersonAsync(newPerson.Name);

            // Assert
            Assert.True(returnedPerson != null);
            Assert.True(returnedPerson.Profile == newPerson.Profile);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

        [Fact]
        public async void FindPersonRoute_Test()
        {
            // Arrange
            var newPerson = CreatePerson();
            await _personRepository.AddOrUpdatePersonAsync(newPerson);

            // Act
            var result = await _personController.FindPersonAsync(newPerson.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

        [Fact]
        public async void GetAllPeopleRoute_Test()
        {
            var person1 = CreatePerson();
            var person2 = CreatePerson();
            await _personRepository.AddOrUpdatePersonAsync(person1);
            await _personRepository.AddOrUpdatePersonAsync(person2);

            // Act
            var result = await _personController.GetAllPeopleAsync();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Clean up
            await _personRepository.DeletePersonAsync(person1);
            await _personRepository.DeletePersonAsync(person2);
           
        }

    }
}