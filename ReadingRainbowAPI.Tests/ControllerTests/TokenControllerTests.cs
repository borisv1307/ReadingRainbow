using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Configuration;

namespace ReadingRainbowAPI.ControllerTests
{

    public class TokenControllerTests
    {
        private TokenController _tokenController; 
        private Person _newPerson;  

        // Initalize Method used for all tests
        public TokenControllerTests()
        {
            var personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};
        
            _newPerson = CreatePerson();

            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                                        
            personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(_newPerson);

            _tokenController = new TokenController(config, personRepository.Object);
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
        public async void GetTokenValidUser_Test()
        {   
            // Act
            var result = await _tokenController.GetRandomToken(_newPerson.Name, _newPerson.HashedPassword);
            var okResult = result as OkObjectResult;
            var token = okResult.Value as string;

            // Assert
            Assert.True(okResult != null);
            Assert.True(token != null);
            Assert.Equal(200, okResult.StatusCode);

       }

        [Fact]
        public async void GetTokenInValidUser_Test()
        {
            // Act
            var result = await _tokenController.GetRandomToken("RandomName", "RandomPassword");
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}