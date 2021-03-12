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
        private IMapper _mapper;

        // Initalize Method used for all tests
        public RecommendationControllerTests()
        {
            _RecommendationRepository = new Mock<RecommendationRepository>(new Mock<INeo4jDBContext>().Object) { CallBase = true };

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();

        }

        [Fact]
        public async void RequestPopLib_Test()
        {
            //Arrage
            var popularity1 = new PopularityResult();
            var popularity2 = new PopularityResult();

            List<PopularityResult> popularLibrList = new List<PopularityResult>();

            _RecommendationRepository
            .Setup(x =>
                x.GetPopularLibrary()).ReturnsAsync(popularLibrList);

            var RecommendationController = new RecommendationController(_RecommendationRepository.Object, _mapper);

            var result = await RecommendationController.RequestPopLib();
            var okResult = result as OkObjectResult;
            Assert.True(okResult != null);
        }
    }
}