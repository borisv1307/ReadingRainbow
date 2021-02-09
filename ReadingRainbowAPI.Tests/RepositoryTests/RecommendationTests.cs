using System;
using Xunit;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadingRainbowAPI.RepositoryTests
{
    [Collection("Database collection")]
    public class RecommendationTests
    {
        DatabaseFixture fixture;

        private BookRepository _bookRepository;
        private PersonRepository _personRepository;
        private RecommendationRepository _recommendationRepository;

        // Initalize Method used for all tests
        public RecommendationTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _bookRepository = new BookRepository(fixture.dbContext);
            _personRepository = new PersonRepository(fixture.dbContext);
            _recommendationRepository = new RecommendationRepository(fixture.dbContext);
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
        public async void PopularLibrary_Test()
        {
            //Arrange
            var newPerson = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson);

            var newPerson2 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson2);

            var newPerson3 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson3);

            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);
 
            var newBook2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook2);

            var newBook3 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook3);
 
            var inLibrary = new Relationships.InLibrary();
 
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook, newPerson, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook2, newPerson, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook3, newPerson, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook2, newPerson2, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook3, newPerson2, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook3, newPerson3, inLibrary);

            //Act
            var returnedBook = (await _recommendationRepository.GetPopularLibrary());
            //Assert
            Assert.True(newBook3 == returnedBook);
            //Cleanup
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook, newPerson, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook2, newPerson, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook3, newPerson, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook2, newPerson2, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook3, newPerson2, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook3, newPerson3, inLibrary);
            await _bookRepository.DeleteBookAsync(newBook);
            await _bookRepository.DeleteBookAsync(newBook2);
            await _bookRepository.DeleteBookAsync(newBook3);
            await _personRepository.DeletePersonAsync(newPerson); 
            await _personRepository.DeletePersonAsync(newPerson2); 
            await _personRepository.DeletePersonAsync(newPerson3);
        }

        [Fact]
        public async void PopularWishList_Test()
        {
            //Arrange
            var newPerson = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson);

            var newPerson2 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson2);

            var newPerson3 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson3);

            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);
 
            var newBook2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook2);

            var newBook3 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook3);
 
            var wishList = new Relationships.WishList();
 
            await _bookRepository.CreateWishlistRelationshipAsync(newBook, newPerson, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook2, newPerson, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook3, newPerson, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook2, newPerson2, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook3, newPerson2, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook3, newPerson3, wishList);

            //Act
            var returnedBook = (await _recommendationRepository.GetPopularWishList());
            //Assert
            Assert.True(newBook3 == returnedBook);
            //Cleanup
            await _bookRepository.DeleteWishListRelationshipAsync(newBook, newPerson, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook2, newPerson, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook3, newPerson, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook2, newPerson2, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook3, newPerson2, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook3, newPerson3, wishList);
            await _bookRepository.DeleteBookAsync(newBook);
            await _bookRepository.DeleteBookAsync(newBook2);
            await _bookRepository.DeleteBookAsync(newBook3);
            await _personRepository.DeletePersonAsync(newPerson); 
            await _personRepository.DeletePersonAsync(newPerson2); 
            await _personRepository.DeletePersonAsync(newPerson3);
        }

        [Fact]
        public async void JaccardWishListRecommendation_Test()
        {
            //Arrange
            var newPerson = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson);

            var newPerson2 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson2);

            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);
 
            var newBook2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook2);

            var newBook3 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook3);
 
            var wishList = new Relationships.WishList();
 
            await _bookRepository.CreateWishlistRelationshipAsync(newBook, newPerson2, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook2, newPerson2, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook3, newPerson2, wishList);
            await _bookRepository.CreateWishlistRelationshipAsync(newBook, newPerson, wishList);     

            var inLibrary = new Relationships.InLibrary();

            await _bookRepository.CreateInLibraryRelationshipAsync(newBook2, newPerson, inLibrary);

            //Act
            var returnedBook = (await _recommendationRepository.GetJaccardWishList(newPerson));
            //Assert
            Assert.True(newBook3 == returnedBook);
            //Cleanup
            await _bookRepository.DeleteWishListRelationshipAsync(newBook, newPerson2, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook2, newPerson2, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook3, newPerson2, wishList);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook, newPerson, wishList);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook2, newPerson, inLibrary);
            await _bookRepository.DeleteBookAsync(newBook);
            await _bookRepository.DeleteBookAsync(newBook2);
            await _bookRepository.DeleteBookAsync(newBook3);
            await _personRepository.DeletePersonAsync(newPerson); 
            await _personRepository.DeletePersonAsync(newPerson2);
        }

        [Fact]
        public async void JaccardLibraryRecommendation_Test()
        {
            //Arrange
            var newPerson = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson);

            var newPerson2 = CreatePerson();
            await _personRepository.AddPersonAsync(newPerson2);

            var newBook = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook);
 
            var newBook2 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook2);

            var newBook3 = CreateBook();
            await _bookRepository.AddOrUpdateAsync(newBook3);
 
            var inLibrary = new Relationships.InLibrary();
 
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook, newPerson2, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook2, newPerson2, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook3, newPerson2, inLibrary);
            await _bookRepository.CreateInLibraryRelationshipAsync(newBook, newPerson, inLibrary);     
            
            var wishList = new Relationships.WishList();

            await _bookRepository.CreateWishlistRelationshipAsync(newBook2, newPerson, wishList);

            //Act
            var returnedBook = (await _recommendationRepository.GetJaccardLibrary(newPerson));
            //Assert
            Assert.True(newBook3 == returnedBook);
            //Cleanup
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook, newPerson2, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook2, newPerson2, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook3, newPerson2, inLibrary);
            await _bookRepository.DeleteInLibraryRelationshipAsync(newBook, newPerson, inLibrary);
            await _bookRepository.DeleteWishListRelationshipAsync(newBook2, newPerson, wishList);
            await _bookRepository.DeleteBookAsync(newBook);
            await _bookRepository.DeleteBookAsync(newBook2);
            await _bookRepository.DeleteBookAsync(newBook3);
            await _personRepository.DeletePersonAsync(newPerson); 
            await _personRepository.DeletePersonAsync(newPerson2);
        }
    }
}