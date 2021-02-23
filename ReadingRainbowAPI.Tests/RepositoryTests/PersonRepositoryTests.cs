using System;
using Xunit;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadingRainbowAPI.RepositoryTests
{
    [Collection("Database collection")]
    public class PersonRepositoryTests
    {
        DatabaseFixture fixture;

        private PersonRepository _personRepository;
        private BookRepository _bookRepository;
        private FriendRepository _FriendRepository;

        // Initalize Method used for all tests
        public PersonRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _personRepository = new PersonRepository(fixture.dbContext);
            _bookRepository = new BookRepository(fixture.dbContext);
            _FriendRepository = new FriendRepository(fixture.dbContext);
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

        private Book CreateBook()
        {
            var random = new Random();
            var bookIdExt = random.Next();

            // Arrange
            return new Book(){
                Id = $"xbn56r{bookIdExt}",
                Title =$"Test Book Title {bookIdExt}",
                PublishDate  = DateTime.Now.ToShortDateString(),
                NumberPages  = $"{bookIdExt}",
                Description  = "Test Book Description",
            };
        }

        [Fact]
        public async void GetPeopleAsync_Test()
        {
            var person1 = CreatePerson();
            var person2 = CreatePerson();
            await _personRepository.AddPersonAsync(person1);
            await _personRepository.AddPersonAsync(person2);

            var people = await _personRepository.GetAllPeopleAsync();
            Assert.True(people != null);

            // Clean up
            await _personRepository.DeletePersonAsync(person1);
            await _personRepository.DeletePersonAsync(person2);
        }

        [Fact]
        public async void AddPersonAsync_Test()
        {
            // Arrange
            var newPerson = CreatePerson();

            // Act
            await _personRepository.AddPersonAsync(newPerson);
            var returnedPerson = await _personRepository.GetPersonAsync(newPerson.Name);

            // Assert
            Assert.True(returnedPerson != null);
            Assert.True(returnedPerson.Name == newPerson.Name);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

        [Fact]
        public async void UpdatePersonAsync_Test()
        {
            // Arrange
            var random = new Random();
            var NewPersonProfileExt = random.Next();
            var newPersonProfile = $"New Person's Updated Profile {NewPersonProfileExt}";

            var newPerson = CreatePerson();

            // Act
            await _personRepository.AddPersonAsync(newPerson);
            newPerson.Profile = newPersonProfile;
            await _personRepository.UpdatePersonAsync(newPerson);

            var returnedPerson = await _personRepository.GetPersonAsync(newPerson.Name);

            // Assert
            Assert.True(returnedPerson != null);
            Assert.True(returnedPerson.Profile == newPersonProfile);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

        [Fact]
        public async void GetPersonAsync_Test()
        {
            var newPerson = CreatePerson();

            await _personRepository.AddPersonAsync(newPerson);
            var returnedperson = await _personRepository.GetPersonAsync(newPerson.Name);

            Assert.True(newPerson.HashedPassword == returnedperson.HashedPassword);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

        [Fact]
        public async void AddFriendsAsync_Test()
        {
            // Arrange
            var person1 = CreatePerson();
            var person2 = CreatePerson();
            var friendsWith = new Relationships.FriendsWith(); 

            await _personRepository.AddPersonAsync(person1);
            await _personRepository.AddPersonAsync(person2);

            // Act
            await _personRepository.CreateFriendRelationshipAsync(person1, person2, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person2, person1, friendsWith);

            var friends = await _FriendRepository.GetConfirmedFriendsWithRelationshipAsync(person1, friendsWith);

            // Assert
            Assert.True(friends.Count() == 1);
            Assert.True(friends.FirstOrDefault().Name == person2.Name);

            // CleanUp
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person1, person2);
            await _personRepository.DeletePersonAsync(person1);
            await _personRepository.DeletePersonAsync(person2);
        }

        [Fact]
        public async void NoFriendsAsync_Test()
        {
            // Arrange
            var person1 = CreatePerson();
            var person2 = CreatePerson();

            await _personRepository.AddPersonAsync(person1);
            await _personRepository.AddPersonAsync(person2);

            // Act
            var friends = await _FriendRepository.GetConfirmedFriendsWithRelationshipAsync(person1, new Relationships.FriendsWith());

            // Assert
            Assert.True(friends.Count() == 0);

            // CleanUp
            await _personRepository.DeletePersonAsync(person1);
            await _personRepository.DeletePersonAsync(person2);
        }

        [Fact]
        public async void RemoveFriendAsync_Test()
        {
            // Arrange
            var person = CreatePerson();
            var friendsForever = CreatePerson();
            var noLongerFriend = CreatePerson();
            var friendsWith = new Relationships.FriendsWith();

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(friendsForever);
            await _personRepository.AddPersonAsync(noLongerFriend);

            await _personRepository.CreateFriendRelationshipAsync(person, noLongerFriend, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, friendsForever, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(noLongerFriend, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friendsForever, person, friendsWith);

            // Act
            var everyoneIsFriends = await _FriendRepository.GetConfirmedFriendsWithRelationshipAsync(person, friendsWith);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, noLongerFriend);
            var someoneIsLeftout = await _FriendRepository.GetConfirmedFriendsWithRelationshipAsync(person, friendsWith);

            // Assert
            Assert.True(everyoneIsFriends.Where(f=> f.Name == friendsForever.Name).ToList().Count == 1);
            Assert.True(everyoneIsFriends.Where(f=> f.Name == noLongerFriend.Name).ToList().Count == 1);
            Assert.True(everyoneIsFriends.ToList().Count == 2);

            Assert.True(someoneIsLeftout.Where(f=> f.Name == friendsForever.Name).ToList().Count == 1);
            Assert.True(someoneIsLeftout.Where(f=> f.Name == noLongerFriend.Name).ToList().Count == 0);
            Assert.True(someoneIsLeftout.ToList().Count == 1);

            // CleanUp
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friendsForever);
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.DeletePersonAsync(friendsForever);
            await _personRepository.DeletePersonAsync(noLongerFriend);
        }

        [Fact]
        public async void GetFriendsAsync_Test()
        {
            // Arrange 
            var person = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();
            var notaFriend = CreatePerson();

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(friend1);
            await _personRepository.AddPersonAsync(friend2);
            await _personRepository.AddPersonAsync(notaFriend);

            var friendsWith = new Relationships.FriendsWith();

            await _personRepository.CreateFriendRelationshipAsync(person, friend1, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend1, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, friend2, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend2, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend1, notaFriend, friendsWith);

            // Act
            var personsFriends = await _FriendRepository.GetConfirmedFriendsWithRelationshipAsync(person, friendsWith);

            // Assert
            Assert.True(personsFriends.Where(f=> f.Name == friend1.Name).ToList().Count == 1);
            Assert.True(personsFriends.Where(f=> f.Name == friend2.Name).ToList().Count == 1);
            Assert.True(personsFriends.Where(f=> f.Name == notaFriend.Name).ToList().Count == 0);
            Assert.True(personsFriends.ToList().Count == 2);

            // Clean Up
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend1);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend2);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(friend1, notaFriend);
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.DeletePersonAsync(friend1);
            await _personRepository.DeletePersonAsync(friend2);
            await _personRepository.DeletePersonAsync(notaFriend);
        }

        // Get requested friends (person already has some friends)
        [Fact]
        public async void GetRequestedFriendsAsync_Test()
        {
            // Arrange 
            var person = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();
            var requestedFriend = CreatePerson();

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(friend1);
            await _personRepository.AddPersonAsync(friend2);
            await _personRepository.AddPersonAsync(requestedFriend);

            var friendsWith = new Relationships.FriendsWith();

            await _personRepository.CreateFriendRelationshipAsync(person, friend1, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend1, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, friend2, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend2, person, friendsWith);

            // Only one-way relationship
            await _personRepository.CreateFriendRelationshipAsync(person, requestedFriend, friendsWith);

            // Act
            var personsRequestedFriends = await _FriendRepository.GetRequestedFriends(person, friendsWith);

            // Assert
            Assert.True(personsRequestedFriends.Where(f=> f.Name == requestedFriend.Name).ToArray().Length  == 1);
            Assert.True(personsRequestedFriends.ToArray().Length == 1);

            // Clean Up
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend1);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend2);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, requestedFriend);
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.DeletePersonAsync(friend1);
            await _personRepository.DeletePersonAsync(friend2);
            await _personRepository.DeletePersonAsync(requestedFriend);
        }

        [Fact]
        public async void GetFriendRequestsHasFriends_Async()
        {
            // Arrange 
            var person = CreatePerson();
            var friend1 = CreatePerson();
            var friend2 = CreatePerson();
            var requestingFriend = CreatePerson();

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(friend1);
            await _personRepository.AddPersonAsync(friend2);
            await _personRepository.AddPersonAsync(requestingFriend);

            var friendsWith = new Relationships.FriendsWith();

            await _personRepository.CreateFriendRelationshipAsync(person, friend1, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend1, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, friend2, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend2, person, friendsWith);

            // Only one-way relationship
            await _personRepository.CreateFriendRelationshipAsync(requestingFriend, person, friendsWith);

            // Act
            var friendRequests = await _FriendRepository.GetFriendRequests(person, friendsWith);

            // Assert
            Assert.True(friendRequests.Where(f=> f.Name == requestingFriend.Name).ToArray().Length  == 1);
            Assert.True(friendRequests.ToArray().Length == 1);

            // Clean Up
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend1);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(person, friend2);
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(requestingFriend, person);
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.DeletePersonAsync(friend1);
            await _personRepository.DeletePersonAsync(friend2);
            await _personRepository.DeletePersonAsync(requestingFriend);
        }

        [Fact]
        public async void GetFriendRequestsNoFriends_Async()
        {
            // Arrange 
            var person = CreatePerson();
            var requestingFriend = CreatePerson();

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(requestingFriend);

            var friendsWith = new Relationships.FriendsWith();

            // Only one-way relationship
            await _personRepository.CreateFriendRelationshipAsync(requestingFriend, person, friendsWith);

            // Act
            var friendRequests = await _FriendRepository.GetFriendRequests(person, friendsWith);

            // Assert
            Assert.True(friendRequests.Where(f=> f.Name == requestingFriend.Name).ToArray().Length  == 1);
            Assert.True(friendRequests.ToArray().Length == 1);

            // Clean Up
            await _FriendRepository.DeleteFriendsWithRelationshipAsync(requestingFriend, person);
            await _personRepository.DeletePersonAsync(person);
            await _personRepository.DeletePersonAsync(requestingFriend);
        }

        [Fact]
        public async void CreateFriendRequests_test()
        {
            // Arrange 
            var person = new Person(){
                Name = "user1",
                Portrait = @"Https://User1PortraitLink",
                HashedPassword = "lljasdlfhklhg",
                EmailConfirmed = "True",
                Email = "user1@email.com"

            };
            var friend =  new Person(){
                Name = "friend1",
                Portrait = @"Https://friend1PortraitLink",
                HashedPassword = "asdfasdffgsafasdf",
                EmailConfirmed = "True",
                Email = "friend1@email.com"

            };
            var requestedfriend =  new Person(){
                Name = "requestedfriend1",
                Portrait = @"Https://requestedfriend1PortraitLink",
                HashedPassword = "asdffghjkuiyliulyu",
                EmailConfirmed = "True",
                Email = "requestedfriend1@email.com"

            };
            var requestingfriend =  new Person(){
                Name = "requestingfriend1",
                Portrait = @"Https://requestingfriend1PortraitLink",
                HashedPassword = "asdffghjkuiyliulyu",
                EmailConfirmed = "True",
                Email = "requestingfriend1@email.com"

            };

            await _personRepository.AddPersonAsync(person);
            await _personRepository.AddPersonAsync(friend);
            await _personRepository.AddPersonAsync(requestedfriend);
            await _personRepository.AddPersonAsync(requestingfriend);

            var friendsWith = new Relationships.FriendsWith();

            // Only one-way relationship
            await _personRepository.CreateFriendRelationshipAsync(requestingfriend, person, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, requestedfriend, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(person, friend, friendsWith);
            await _personRepository.CreateFriendRelationshipAsync(friend, person, friendsWith);
        }

    
    }
}