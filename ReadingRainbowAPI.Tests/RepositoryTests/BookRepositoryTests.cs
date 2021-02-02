using System;
using Xunit;
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
            await _personRepository.AddPersonAsync(newPerson);

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

               [Fact]
        public async void RemoveWishListAsync_Test()
        {
            var person = CreatePerson();
            var booktostay1 = CreateBook();
            var booktostay2 = CreateBook();
            var booktogo = CreateBook();

            await _personRepository.AddPersonAsync(person);
            await _bookRepository.AddOrUpdateAsync(booktostay1);
            await _bookRepository.AddOrUpdateAsync(booktostay2);
            await _bookRepository.AddOrUpdateAsync(booktogo);

            var wishList = new Relationships.WishList();

            await _bookRepository.CreateWishlistRelationshipAsync(booktostay1, person, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(booktostay2, person, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(booktogo, person, wishList);

            // Act
            var allBooksInList = await _personRepository.GetWishListRelationshipAsync(person, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(booktogo, person, wishList);
            var twoBooksInList = await _personRepository.GetWishListRelationshipAsync(person, wishList);

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
            await _bookRepository.DeleteWishListRelationshipAsync(booktostay1, person, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(booktostay2, person, wishList);
            await _personRepository.DeletePersonAsync(person);
            await _bookRepository.DeleteBookAsync(booktostay1);
            await _bookRepository.DeleteBookAsync(booktostay2);
            await _bookRepository.DeleteBookAsync(booktogo);
        }

                
        [Fact]
        public async void AddWishListAsync_Test()
        {
            // Arrange
            var person = CreatePerson();
            var book = CreateBook();
            var wishList = new Relationships.WishList();

            await _personRepository.AddPersonAsync(person);
            await _bookRepository.AddOrUpdateAsync(book);
            await _bookRepository.CreateWishlistRelationshipAsync(book, person, wishList);

            // Act
            var wishListBooks = await _personRepository.GetWishListRelationshipAsync(person, wishList);

            // Assert
            Assert.True(wishListBooks.Where(b=> b.Id == book.Id).ToList().Count == 1);
            Assert.True(wishListBooks.ToList().Count == 1);

            // Clean up
            await _bookRepository.DeleteWishListRelationshipAsync(book, person, wishList);
            await _personRepository.DeletePersonAsync(person);
            await _bookRepository.DeleteBookAsync(book);
        }
    }
}