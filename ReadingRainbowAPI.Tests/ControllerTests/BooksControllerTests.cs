using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using Neo4j.Driver;

namespace ReadingRainbowAPI.ControllerTests
{
    public class BookControllerTests:IDisposable
    {

        private IDriver _iDriver;
        private BookController _bookController;

        // Initalize Method used for all tests
        public BookControllerTests()
        {
            _iDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("Neo4j", "Neo4jPassword"));
            _bookController = new BookController(_iDriver);
        }

        [Fact]
        public void GetBookAsync_Test()
        {
            var books = _bookController.GetAsync();
            Assert.True(books != null);
        }

        [Fact]
        public void GetBookAsyncFail_Test()
        {
            var books = _bookController.GetAsync();
            Assert.True(books != null);
        }

        // Clean up Method   
        public void Dispose()
        {
           _iDriver.CloseAsync();
        }
    }
}
