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

        // Initalize Method used for all tests
        public PersonRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            _personRepository = new PersonRepository(fixture.dbContext);
        }

        private Person CreatePerson()
        {
            var random = new Random();
            var personId = random.Next();

            // Arrange
            return new Person(){
                Name = $"newPerson{personId}",
                Profile =$"This is new person number {personId}",
                Portrait = "Https://PortraitLink",
                HashedPassword = $"{personId}"
            };
        }

        [Fact]
        public async void GetPeopleAsync_Test()
        {
            var person1 = CreatePerson();
            var person2 = CreatePerson();
            await _personRepository.AddOrUpdatePersonAsync(person1);
            await _personRepository.AddOrUpdatePersonAsync(person2);

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
            await _personRepository.AddOrUpdatePersonAsync(newPerson);
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
            await _personRepository.AddOrUpdatePersonAsync(newPerson);
            newPerson.Profile = newPersonProfile;
            await _personRepository.AddOrUpdatePersonAsync(newPerson);

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

            await _personRepository.AddOrUpdatePersonAsync(newPerson);
            var returnedperson = await _personRepository.GetPersonAsync(newPerson.Name);

            Assert.True(newPerson.HashedPassword == returnedperson.HashedPassword);

            // Clean up
            await _personRepository.DeletePersonAsync(newPerson);
        }

    }
}