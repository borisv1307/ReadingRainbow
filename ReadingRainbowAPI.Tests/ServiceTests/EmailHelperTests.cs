using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Services;
using ReadingRainbowAPI.Middleware;
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


        [Fact]
        public async void SendEmail_Valid_Test()
        {
            // Act
            var emailSent = await _emailHelper.SendEmail("userName", _validEmail, "confirmationLink");

            // Assert
            Assert.True(emailSent);
        }
    }
}