using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
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

        private void SetupMockBookRepo(Book returnedBook)
        {
            _bookRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Book, bool>>>()))
                    .ReturnsAsync(returnedBook);
            _bookRepository
                    .Setup(a=>a.Add(It.IsAny<Book>()))
                    .ReturnsAsync(true);
            _bookRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Book>()))
                  .ReturnsAsync(true);
        }

        private Book CreateBook(List<Genre> genres = null)
        {
            var random = new Random();
            var bookIdExt = random.Next();

            // Arrange
            var book = new Book(){
                Id = $"xbn56r{bookIdExt}",
                Title =$"Test Book Title {bookIdExt}",
                PublishDate  = DateTime.Now.ToShortDateString(),
                NumberPages  = $"{bookIdExt}",
                Description  = "Test Book Description",
            };

            if (genres != null)
            {
                book.Genres = genres;
            }

            return book;

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

        private Genre CreateGenre()
        {
            var random = new Random();
            var genreId = random.Next();

            // Arrange
            return new Genre(){
                Name = $"newGenre{genreId}",
                Description =$"This is new genre number {genreId}"

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
                
            SetupMockBookRepo(newBook);

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

        [Fact]
        public async void AddNewBooktoLibraryRoute_Test()
        {   
            // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var library = new List<Book>();
            var bookGenres = new List<Genre>();


            _bookRepository
            .Setup(x => 
                x.Relate<Person, InLibrary>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<InLibrary>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, InLibrary>(
                    (exp1, exp2, inLibrary) => { library.Add(newBook); }
                );

            _genreRepository 
            .Setup(x => 
                x.GetRelated<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>());

            _genreRepository
            .Setup(x => 
                x.Relate<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Genre, bool>>, Expression<Func<Book, bool>>, InGenre>(
                    (exp1, exp2, inLibrary) => { bookGenres.Add(genre1); }
                );
         
            SetupMockBookRepo(CreateBook());

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBookToLibrary(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(library.Count == 1);
            Assert.True(library.Where(b=>b.Title == newBook.Title).ToList().Count == 1);

            Assert.True(bookGenres.Count == 1);
            Assert.True(bookGenres.Where(b=>b.Description == genre1.Description).ToList().Count == 1);
            
        }

        [Fact]
        public async void AddNewBooktoWishListRoute_Test()
        {   
            // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var wishList = new List<Book>();
            var bookGenres = new List<Genre>();


            _bookRepository
            .Setup(x => 
                x.Relate<Person, WishList>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<WishList>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, WishList>(
                    (exp1, exp2, WishList) => { wishList.Add(newBook); }
                );

            _genreRepository 
            .Setup(x => 
                x.GetRelated<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>());

            _genreRepository
            .Setup(x => 
                x.Relate<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Genre, bool>>, Expression<Func<Book, bool>>, InGenre>(
                    (exp1, exp2, WishList) => { bookGenres.Add(genre1); }
                );
         
            SetupMockBookRepo(CreateBook());

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBooksToWishList(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(wishList.Count == 1);
            Assert.True(wishList.Where(b=>b.Title == newBook.Title).ToList().Count == 1);

            Assert.True(bookGenres.Count == 1);
            Assert.True(bookGenres.Where(b=>b.Description == genre1.Description).ToList().Count == 1);
            
        }

        [Fact]
        public async void AddExistingBooktoLibraryRoute_Test()
        {         
            // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var library = new List<Book>();

            _bookRepository
            .Setup(x => 
                x.Relate<Person, InLibrary>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<InLibrary>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, InLibrary>(
                    (exp1, exp2, inLibrary) => { library.Add(newBook); }
                );

            _genreRepository
            .Setup(x => 
                x.GetRelated<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>() { newBook  });
         
            SetupMockBookRepo(newBook);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBookToLibrary(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(library.Count == 1);
            Assert.True(library.Where(b=>b.Title == newBook.Title).ToList().Count == 1);
            
        }

        [Fact]
        public async void AddExistingBooktoWishList_Test()
        {         
            // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var wishList = new List<Book>();

            _bookRepository
            .Setup(x => 
                x.Relate<Person, WishList>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<WishList>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, WishList>(
                    (exp1, exp2, WishList) => { wishList.Add(newBook); }
                );

            _genreRepository
            .Setup(x => 
                x.GetRelated<Book, InGenre>(It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>() { newBook  });
         
            SetupMockBookRepo(newBook);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBooksToWishList(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(wishList.Count == 1);
            Assert.True(wishList.Where(b=>b.Title == newBook.Title).ToList().Count == 1);
            
        }

        [Fact]
        public async void AddBooktoLibraryAlreadyInLibraryRoute_Test()
        {  
                        // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var library = new List<Book>();
            var bookGenres = new List<Genre>();


            _bookRepository
            .Setup(x => 
                x.Relate<Person, InLibrary>(
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<InLibrary>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, InLibrary>(
                    (exp1, exp2, inLibrary) => { library.Add(newBook); }
                );

            _bookRepository
            .Setup(x =>
                x.GetRelated<Person, InLibrary>(
                    It.IsAny<Expression<Func<Book,bool>>>(),
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    It.IsAny<InLibrary>())
                )
                .ReturnsAsync(new List<Person>() {person});

            _genreRepository
            .Setup(x => 
                x.GetRelated<Book, InGenre>(
                    It.IsAny<Expression<Func<Genre, bool>>>(), 
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>() { newBook  });

            _genreRepository
            .Setup(x => 
                x.Relate<Book, InGenre>(
                    It.IsAny<Expression<Func<Genre, bool>>>(), 
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<InGenre>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Genre, bool>>, Expression<Func<Book, bool>>, InGenre>(
                    (exp1, exp2, inLibrary) => { bookGenres.Add(genre1); }
                );
         
            SetupMockBookRepo(newBook);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBookToLibrary(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(library.Count == 0);
            Assert.True(library.Where(b=>b.Title == newBook.Title).ToList().Count == 0);

            // Since Book already had genres associated more should not have been added
            Assert.True(bookGenres.Count == 0);
        }

        
        [Fact]
        public async void AddBooktoWishListAlreadyInWishListRoute_Test()
        {  
                        // Arrange
            var genre1 = CreateGenre();
            var newBook = CreateBook(new List<Genre>() {
                genre1
            });
            var person = CreatePerson();

            var inWishList = new List<Book>();
            var bookGenres = new List<Genre>();


            _bookRepository
            .Setup(x => 
                x.Relate<Person, WishList>(
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<WishList>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Book, bool>>, Expression<Func<Person, bool>>, WishList>(
                    (exp1, exp2, WishList) => { inWishList.Add(newBook); }
                );

            _bookRepository
            .Setup(x =>
                x.GetRelated<Person, WishList>(
                    It.IsAny<Expression<Func<Book,bool>>>(),
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    It.IsAny<WishList>())
                )
                .ReturnsAsync(new List<Person>() {person});

            _genreRepository
            .Setup(x => 
                x.GetRelated<Book, InGenre>(
                    It.IsAny<Expression<Func<Genre, bool>>>(), 
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<InGenre>()))
                .ReturnsAsync(new List<Book>() { newBook  });

            _genreRepository
            .Setup(x => 
                x.Relate<Book, InGenre>(
                    It.IsAny<Expression<Func<Genre, bool>>>(), 
                    It.IsAny<Expression<Func<Book, bool>>>(), 
                    It.IsAny<InGenre>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Genre, bool>>, Expression<Func<Book, bool>>, InGenre>(
                    (exp1, exp2, WishList) => { bookGenres.Add(genre1); }
                );
         
            SetupMockBookRepo(newBook);

            var bookController = new BookController(_bookRepository.Object, _genreRepository.Object, _mapper);

            // Act
            var result = await bookController.AddBooksToWishList(person.Name, newBook);
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            // Verify Correct functions were called / correct callbacks were performed
            Assert.True(inWishList.Count != 0);
            Assert.True(inWishList.Where(b=>b.Title == newBook.Title).ToList().Count != 0);

            // Since Book already had genres associated more should not have been added
            Assert.True(bookGenres.Count == 0);
        }

    }
}
