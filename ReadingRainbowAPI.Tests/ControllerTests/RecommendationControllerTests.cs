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
    public class RecommendationControllerTests
    {
        private Mock<RecommendationRepository> _RecommendationRepository;
        private Mock<PersonRepository> _personRepository;
        private Mock<BookRepository> _bookRepository;
        private IMapper _mapper;
        private Mock<IEmailHelper> _emailHelper;

        // Initalize Method used for all tests
        public RecommendationControllerTests()
        {
            _RecommendationRepository = new Mock<RecommendationRepository>(new Mock<INeo4jDBContext>().Object) { CallBase = true };

            _personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object) { CallBase = true };

            _bookRepository = new Mock<BookRepository>(new Mock<INeo4jDBContext>().Object) { CallBase = true };
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();

            _emailHelper = new Mock<IEmailHelper>();
        }
        private Book CreateBook()
        {
            var random = new Random();
            var bookIdExt = random.Next();

            // Arrange
            return new Book()
            {
                Id = $"xbn56r{bookIdExt}",
                Title = $"Test Book Title {bookIdExt}",
                PublishDate = DateTime.Now.ToShortDateString(),
                NumberPages = $"{bookIdExt}",
                Description = "Test Book Description",
            };
        }

        private Person CreatePerson()
        {
            var random = new Random();
            var personId = random.Next();

            // Arrange
            return new Person()
            {
                Name = $"newPerson{personId}",
                Profile = $"This is new person number {personId}",
                Portrait = "Https://PortaitLink",
                HashedPassword = $"{personId}",
                Email = "k.lindseth@hotmail.com",
                EmailConfirmed = "False"
            };
        }

        [Fact]
        public async void RequestPopLib_Test()
        {
            //Arrage
            var book1 = CreateBook();
            var book2 = CreateBook();
            // var book3 = CreateBook();

            var person1 = CreatePerson();
            // var person2 = CreatePerson();

            var popularLibrList = new PopularityResult();

            _personRepository
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync((Person)null);
            _personRepository
                    .Setup(a => a.Add(It.IsAny<Person>()))
                    .ReturnsAsync(true);
            _personRepository
                  .Setup(x => x.Update(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Person>()))
                  .ReturnsAsync(true);
            _emailHelper
                    .Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(true);

            _RecommendationRepository
            .Setup(x =>
                x.GetPopularLibrary<PopularityResult>()).ReturnsAsync(popularLibrList);

            var RecommendationController = new RecommendationController(_RecommendationRepository.Object, _mapper);

            var result = await RecommendationController.RequestPopLib();
            var okResult = result as OkObjectResult;
            Assert.True(okResult != null);
        }
    }
}