using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Services;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.Extensions.Configuration;

namespace ReadingRainbowAPI.ServiceTests
{
    [Collection("Token Tests")]
    public class JwTServiceTests
    {

        private JwtService _jwtService;
        public JwTServiceTests()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            _jwtService = new JwtService(config);
        }


        [Fact]
        public void GenerateSecurityToken_Test()
        {
            // Arrange
            var rand = new Random();
            var emailId = rand.Next();

            var newEmail = $"randomemail{emailId}@google.com";

            // Act
            var token = _jwtService.GenerateSecurityToken(newEmail);

            // Assert
            Assert.True(token != null);

        }
    }
}