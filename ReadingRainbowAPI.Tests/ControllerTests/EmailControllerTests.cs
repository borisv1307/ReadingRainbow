using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using ReadingRainbowAPI.Dto;
using ReadingRainbowAPI.Middleware;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using ReadingRainbowAPI.Mapping;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ReadingRainbowAPI.ControllerTests
{

    public class EmailControllerTests
    {

        private Mock<PersonRepository> _personRepository;
        private Mock<ITokenClass> _tokenClass;       

        // Initalize Method used for all tests
        public EmailControllerTests()
        {
            _personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};
            _tokenClass = new Mock<ITokenClass>();

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
                HashedPassword = $"{personId}",
                Email = "k.lindseth@hotmail.com",
                EmailConfirmed = "False"

            };
        }

        
        [Fact]
        public void ConfirmEmailRoute_ValidToken()
        {   
            // Arrange
            var person = CreatePerson();

            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(person);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);

            _tokenClass
                .Setup(t=>t.CompareToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            _tokenClass
                .Setup(t=>t.CheckTokenDate(It.IsAny<string>()))
                .Returns(true);

            var emailController = new EmailController(_personRepository.Object, _tokenClass.Object);

            // Act
            var emailResult = emailController.ConfirmEmail("token", person.Name);

            // Assert
            Assert.True(emailResult != null);
            //Assert.True(emailResult.Contains("Confirm"));

        }

        [Fact]
        public void ConfirmEmailRoute_InValidToken()
        {   
            // Arrange
            var person = CreatePerson();

            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(person);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);

            _tokenClass
                .Setup(t=>t.CompareToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            _tokenClass
                .Setup(t=>t.CheckTokenDate(It.IsAny<string>()))
                .Returns(true);

            var emailController = new EmailController(_personRepository.Object, _tokenClass.Object);

            // Act
            var emailResult = emailController.ConfirmEmail("token", person.Name);

            // Assert
            Assert.True(emailResult != null);
        }

        [Fact]
        public void ConfirmEmailRoute_InValidDate()
        {   
            // Arrange
            var person = CreatePerson();

            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(person);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);

            _tokenClass
                .Setup(t=>t.CompareToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            _tokenClass
                .Setup(t=>t.CheckTokenDate(It.IsAny<string>()))
                .Returns(false);

            var emailController = new EmailController(_personRepository.Object, _tokenClass.Object);

            // Act
            var emailResult = emailController.ConfirmEmail("token", person.Name);

            // Assert
            Assert.True(emailResult != null);
        }
    }
}