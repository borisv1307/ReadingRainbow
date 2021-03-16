using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Services;
using ReadingRainbowAPI.Middleware;
using ReadingRainbowAPI.Models;
using System.Linq;
using Moq;
using Microsoft.Extensions.Configuration;

namespace ReadingRainbowAPI.ServiceTests
{
    [Collection("Email Tests")]
    public class EmailHelperTests
    {

        private IEmailHelper _emailHelper;

        private string _validEmail;

        public EmailHelperTests()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            _emailHelper = new EmailHelper(config);

            _validEmail = config.GetSection("Email").GetSection("TestSendToEmail").Value;
        }

        private Person CreatePerson()
        {
            var random = new Random();
            var personId = random.Next();

            // Arrange
            return new Person(){
                Name = $"newPerson{personId}",
                Profile =$"This is new person number {personId}",
                Portrait = @"Https://PortraitLink",
                HashedPassword = $"{personId}",
                EmailConfirmed = "True",
                Email = $"{personId}@email.com"

            };
        }


        [Fact]
        public async void SendEmail_Valid_Test()
        {
            // Act
            var emailSent = await _emailHelper.SendEmail("userName", _validEmail, 
                "confirmationLink", _emailHelper.ConfirmationLinkSubject());

            // Assert
            Assert.True(emailSent);
        }

        [Fact]
        public void GenerateConfirmationLink_NullCallback()
        {
            // Arrange
            var person = CreatePerson();

            // Act
            var link = _emailHelper.ConfirmationLinkBody(person, null);

            // Assert
            Assert.True(link != null);

        }

        [Fact]
        public void GenerateConfirmationLink_StringCallback()
        {
            // Arrange
            var person = CreatePerson();
            var callbackUrl = "testing";

            // Act
            var link = _emailHelper.ConfirmationLinkBody(person, callbackUrl);

            // Assert
            Assert.True(link != null);
        }

        [Fact]
        public void GenerateResetPasswordLinkk_StringPassword()
        {
            // Arrange
            var person = CreatePerson();
            var testPW = "testing";
            var testExpirationDate = DateTime.UtcNow.ToShortDateString();

            // Act
            var link = _emailHelper.ResetPasswordBody(person, testPW, testExpirationDate);

            // Assert
            Assert.True(link != null);
        }


        [Fact]
        public void GenerateChangePasswordBody()
        {
            // Arrange
            var person = CreatePerson();

            // Act
            var link = _emailHelper.ChangePasswordBody(person);

            // Assert
            Assert.True(link != null);
        }
    }
}