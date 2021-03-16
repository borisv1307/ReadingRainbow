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

    public class FriendControllerTests
    {

        private Mock<PersonRepository> _personRepository;

        private Mock<IFriendRepository> _friendRepository;

        private IMapper _mapper;

        private Mock<IEmailHelper> _emailHelper;  
        private Mock<ITokenClass> _tokenClass;   

        // Initalize Method used for all tests
        public FriendControllerTests()
        {
            _personRepository = new Mock<PersonRepository>(new Mock<INeo4jDBContext>().Object){CallBase = true};

            _friendRepository = new Mock<IFriendRepository>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();

            _emailHelper = new Mock<IEmailHelper>();
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
        public async void RequestFriendRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend = CreatePerson();

            var requestedFriends = new List<Person>();
                            
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == user.Name))
                    .ReturnsAsync(user);
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == friend.Name))
                    .ReturnsAsync(friend);

            _personRepository
            .Setup(x => 
                x.Relate<Person, FriendsWith>(
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<FriendsWith>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Person, bool>>, Expression<Func<Person, bool>>, FriendsWith>(
                    (exp1, exp2, friendsWith) => { requestedFriends.Add(friend); }
                );

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);

            // Act
            var result = await friendController.RequestFriend(user.Name, friend.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void GetAllFriendsRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();

            var listOfFriends = new List<Person>()
            {
                friend1,
                friend2
            };
                            
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(user);

            _friendRepository
            .Setup(x => 
                x.GetConfirmedFriendsWithRelationshipAsync(
                    It.IsAny<Person>(), 
                    It.IsAny<FriendsWith>()))
                .ReturnsAsync(listOfFriends);

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);


            // Act
            var result = await friendController.GetFriends(user.Name);
            var okResult = result as OkObjectResult;
            var returnedFriendListJSON = okResult.Value as string;
            var returnedFriends = JsonSerializer.Deserialize<List<Person>>(returnedFriendListJSON);

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnedFriends.Count == listOfFriends.Count);
           
        }

        [Fact]
        public async void AcceptFriendRequestRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend = CreatePerson();

            var acceptedFriends = new List<Person>();
                            
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == user.Name))
                    .ReturnsAsync(user);
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == friend.Name))
                    .ReturnsAsync(friend);

            _personRepository
            .Setup(x => 
                x.Relate<Person, FriendsWith>(
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<Expression<Func<Person, bool>>>(), 
                    It.IsAny<FriendsWith>()))
                .ReturnsAsync(true)
                .Callback<Expression<Func<Person, bool>>, Expression<Func<Person, bool>>, FriendsWith>(
                    (exp1, exp2, requestedFriends) => { acceptedFriends.Add(friend); }
                );

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);

            // Act
            var result = await friendController.ConfirmFriendRequest(user.Name, friend.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        // Reject friend request
        [Fact]
        public async void RejectFriendRequestRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend = CreatePerson();

            var acceptedFriends = new List<Person>();
                            
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == user.Name))
                    .ReturnsAsync(user);
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == friend.Name))
                    .ReturnsAsync(friend);

            _friendRepository
            .Setup(x => 
                x.DeleteFriendsWithRelationshipAsync(
                    It.IsAny<Person>(), 
                    It.IsAny<Person>()))
                .Verifiable();

            // public async Task DeleteFriendsWithRelationshipAsync(Person person1, Person person2)

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);

            // Act
            var result = await friendController.RejectFriendRequest(user.Name, friend.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }

        // Remove friend
        [Fact]
        public async void RemoveFriendRequestRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend = CreatePerson();

            var acceptedFriends = new List<Person>();
                            
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == user.Name))
                    .ReturnsAsync(user);
            _personRepository 
                    .Setup(x => x.Single(p=>p.Name == friend.Name))
                    .ReturnsAsync(friend);

            _friendRepository
            .Setup(x => 
                x.DeleteFriendsWithRelationshipAsync(
                    It.IsAny<Person>(), 
                    It.IsAny<Person>()))
                .Verifiable();

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);

            // Act
            var result = await friendController.RemoveFriend(user.Name, friend.Name);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
        }


        // Get friend requests
                [Fact]
        public async void GetFriendsRequestsRoute_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();

            var listOfFriends = new List<Person>()
            {
                friend1,
                friend2
            };
                            
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(user);

            _friendRepository
            .Setup(x => 
                x.GetRequestedFriends(
                    It.IsAny<Person>(), 
                    It.IsAny<FriendsWith>()))
                .ReturnsAsync(listOfFriends);

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);


            // Act
            var result = await friendController.GetRequestedFriends(user.Name);
            var okResult = result as OkObjectResult;
            var returnedRequestedFriendsJSON = okResult.Value as string;
            var returnedRequestedFriends = JsonSerializer.Deserialize<List<Person>>(returnedRequestedFriendsJSON);

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnedRequestedFriends.Count == listOfFriends.Count);
           
        }

        [Fact]
        public async void GetRequestedFriends_Test()
        {
            // Arrange
            var user = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();

            var listOfFriends = new List<Person>()
            {
                friend1,
                friend2
            };
                            
            _personRepository 
                    .Setup(x => x.Single(It.IsAny<Expression<Func<Person, bool>>>()))
                    .ReturnsAsync(user);

            _friendRepository
            .Setup(x => 
                x.GetFriendRequests(
                    It.IsAny<Person>(), 
                    It.IsAny<FriendsWith>()))
                .ReturnsAsync(listOfFriends);

            var personController = new PersonController(_personRepository.Object, _mapper, _emailHelper.Object, _tokenClass.Object);
            var friendController = new FriendController(_personRepository.Object, _friendRepository.Object, _mapper);


            // Act
            var result = await friendController.GetFriendRequests(user.Name);
            var okResult = result as OkObjectResult;
            var returnedFriendRequestsJSON = okResult.Value as string;
            var returnedFriendRequests = JsonSerializer.Deserialize<List<Person>>(returnedFriendRequestsJSON);

            // Assert
            Assert.True(okResult != null);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnedFriendRequests.Count == listOfFriends.Count);
           
        }

    }
}