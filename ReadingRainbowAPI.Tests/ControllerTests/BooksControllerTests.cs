using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using ReadingRainbowAPI.Mapping;
using ReadingRainbowAPI.Dto;
using System.Text.Json;


namespace ReadingRainbowAPI.ControllerTests
{
    public class BookControllerTests
    {
        private Mock<BookRepository> _bookRepository;
        private Mock<GenreRepository> _genreRepository;
        private IMapper _mapper;

        // Initalize Method used for all tests
        public BookControllerTests()
        {
            _bookRepository = new Mock<BookRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};
            _genreRepository = new Mock<GenreRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};

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
        public async void GetLibraryRoute_Test()
        {   
            // Arrange
            var book1 = CreateBook();
            var person1 = CreatePerson();
            var person2 = CreatePerson();

            var personList = new List<Person>()
            {
                person1,
                person2
            };

            _bookRepository
            .Setup(x => 
                x.GetAllRelated<Person, InLibrary>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Person>(), It.IsAny<InLibrary>()))
                .ReturnsAsync(personList);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.GetPeopleAsync(book1.Id);
            var okResult = result as OkObjectResult;
            var returnedPeoplejson = okResult.Value as string;
            var returnedPeople = JsonSerializer.Deserialize<List<PersonDto>>(returnedPeoplejson);

            // Assert
            Assert.True(okResult != null);
            Assert.True(returnedPeople.Count == 2);
            Assert.Equal(200, okResult.StatusCode);
            
        }

        [Fact]
        public async void AddBookRoute_Test()
        {
            // Arrange
            var newBook = CreateBook();
                
            _bookRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Book, bool>>>()))
                    .ReturnsAsync(newBook);
            _bookRepository
                    .Setup(a=>a.Add(It.IsAny<Book>()))
                    .ReturnsAsync(true);
            _bookRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Book>()))
                  .ReturnsAsync(true);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result  = await bookController.AddUpdateBookAsync(newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void FindBookRoute_Test()
        {
            // Arrange
            var newBook = CreateBook();
            var genreList = new List<Genre>()
            {
                new Genre() { Name = "name 1"}
            };
            
            _bookRepository 
                .Setup(x => x.Single(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(newBook);

                // this.GetAllRelated<Genre, InGenre>(b=>b.Id == book.Id, new Genre(),  inGenre);
            _bookRepository
                .Setup(x => 
                x.GetAllRelated<Genre, InGenre>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Genre>(), It.IsAny<InGenre>()))
                .ReturnsAsync(genreList);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.FindBookAsync(newBook.Id);
            var okResult = result as OkObjectResult;
            var returnedBookjson = okResult.Value as string;
            var returnedBook = JsonSerializer.Deserialize<Book>(returnedBookjson);

            // Assert
            Assert.True(okResult != null);
            Assert.True(returnedBook != null);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async void GetAllBookRoute_Test()
        {
            // Arrange
            var book1 = CreateBook();
            var book2 = CreateBook();

            var bookList = new List<Book>() {
                book1,
                book2             
            }; 

            _bookRepository.Setup(x => x.All()).ReturnsAsync(bookList);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.GetAllBooksAsync();
            var okResult = result as OkObjectResult;
            var returnedBookListjson = okResult.Value as string;
            var returnedBooks = JsonSerializer.Deserialize<List<Book>>(returnedBookListjson);

            // Assert
            Assert.True(okResult != null);
            Assert.True(returnedBooks.Count == 2);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
