using System;
using Xunit;
using ReadingRainbowAPI.Controllers;

namespace ReadingRainbowAPI.ControllerTests
{
    [Collection("Database collection")]
    public class BookControllerTests
    {
        DatabaseFixture fixture;

        private BookController _bookController;

        // Initalize Method used for all tests
        public BookControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            _bookController = new BookController(fixture.dbDriver);
        }

        [Fact]
        public void GetBookAsync_Test()
        {
            var books = _bookController.GetAsync();
            Assert.True(books != null);
            // Assert.True(_neoUserName == "Neo4j");
        }

        // Clean up Method   
        // public void Dispose()
        //{
          // _iDriver.CloseAsync();
        //}
    }
}
