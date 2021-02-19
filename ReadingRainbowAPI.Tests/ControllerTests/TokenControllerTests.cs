using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Configuration;
using ReadingRainbowAPI.Middleware;

namespace ReadingRainbowAPI.ControllerTests
{

    public class TokenControllerTests
    {
        private Mock<IEmailHelper> _emailHelper;
        private Mock<PersonRepository> _personRepository;

        private IConfiguration _config;
        private Person _newPerson;  

        // Initalize Method used for all tests
        public TokenControllerTests()
        {
            _personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};
        
            _newPerson = CreatePerson();

            _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            _emailHelper = new Mock<IEmailHelper>();
                                        
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(_newPerson);
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
                Email = "abc@google.com",
                HashedPassword = $"{personId}",
                EmailConfirmed = "True"

            };
        }

        [Fact]
        public async void GetTokenValidUser_Test()
        {   
            // Arrange
            var tokenController = new TokenController(_config, _personRepository.Object, _emailHelper.Object); 

            // Act
            var result = await tokenController.GetRandomToken(_newPerson.Name, _newPerson.HashedPassword);
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
            // Arrange 
            var tokenController = new TokenController(_config, _personRepository.Object, _emailHelper.Object); 

            // Act
            var result = await tokenController.GetRandomToken("RandomName", "RandomPassword");
            var okResult = result as OkResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void ReSendEmail_Test()
        {
            var linkString = $"Please confirm your account by clicking this link: <a href='https://localhost:5001/api/email/AddPerson'>link</a>";

            // Arrange
            var newPerson = CreatePerson();
            newPerson.EmailConfirmed = "False";

            // Setup Single base repository function to return nothing                
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(newPerson);
            _personRepository
                    .Setup(a=>a.Add(It.IsAny<Person>()))
                    .ReturnsAsync(true);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);
            _emailHelper
                    .Setup(e=>e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(true);
            _emailHelper
                    .Setup(e=>e.GenerateEmailLink(It.IsAny<Person>(), It.IsAny<string>()))
                    .Returns(linkString);

            var tokenController = new TokenController(_config, _personRepository.Object, _emailHelper.Object); 

            // Act
            var result = await tokenController.ResendEmail(newPerson.Name, newPerson.HashedPassword);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);

            Assert.Equal("success", okResult.Value.ToString());
        }

    }
}