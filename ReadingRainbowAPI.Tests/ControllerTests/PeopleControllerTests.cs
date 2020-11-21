using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using ReadingRainbowAPI.Dto;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using ReadingRainbowAPI.Mapping;

namespace ReadingRainbowAPI.ControllerTests
{

    public class PeopleControllerTests
    {

        private Mock<PersonRepository> _personRepository;
        private BookRepository _bookRepository;

        private IMapper _mapper;

        

        // Initalize Method used for all tests
        public PeopleControllerTests()
        {
            _personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
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

            var bookList = new List<Book>(){
                book1,
                book2
            };

            _personRepository
            .Setup(x => 
                x.GetAllRelated<Book, InLibrary>(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Book>(), It.IsAny<InLibrary>()))
                .ReturnsAsync(bookList);

            var personController = new PersonController(_personRepository.Object, _mapper);

            // Act
            var result = await personController.GetBooksAsync(person1.Name);
            var okResult = result as OkObjectResult;
            var returnedBookList = okResult.Value as List<Book>;

            // Assert
            Assert.True(okResult != null);
            Assert.True(returnedBookList.Count == 2);
            Assert.Equal(200, okResult.StatusCode);

       }

        [Fact]
        public async void AddPersonRoute_Test()
        {
            // Arrange
            var newPerson = CreatePerson();
                            
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(newPerson);
            _personRepository
                    .Setup(a=>a.Add(It.IsAny<Person>()))
                    .ReturnsAsync(true);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);

            var personController = new PersonController(_personRepository.Object, _mapper);

            // Act
            var result = await personController.AddUpdatePersonAsync(newPerson);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void FindPersonRoute_Test()
        {
            // Arrange
            var newPerson = CreatePerson();
                        
            _personRepository 
                .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(newPerson);

            var personController = new PersonController(_personRepository.Object, _mapper);

            // Act
            var result = await personController.FindPersonAsync(newPerson.Name);
            var okResult = result as OkObjectResult;
            var returnedPerson = okResult.Value as PersonDto;
            var wrongPersonType = okResult.Value as Person;

            // Assert
            Assert.True(okResult != null);
            Assert.True(wrongPersonType == null);
            Assert.True(returnedPerson != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void GetAllPeopleRoute_Test()
        {
            var person1 = CreatePerson();
            var person2 = CreatePerson();
            var personList = new List<Person>()
            {
                person1,
                person2
            }; 

            _personRepository.Setup(x => x.All()).ReturnsAsync(personList);

            var personController = new PersonController(_personRepository.Object, _mapper);

            // Act
            var result = await personController.GetAllPeopleAsync();
            var okResult = result as OkObjectResult;
            var returnedPeople = okResult.Value as List<PersonDto>;
            var wrongListType = okResult.Value as List<Person>;

            // Assert
            Assert.True(okResult != null);
            Assert.True(wrongListType == null);
            Assert.True(returnedPeople.Count == 2);
            Assert.Equal(200, okResult.StatusCode);
           
        }

    }
}