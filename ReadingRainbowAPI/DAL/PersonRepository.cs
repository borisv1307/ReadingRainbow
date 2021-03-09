using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;
using System.Linq.Expressions;
using System;

namespace ReadingRainbowAPI.DAL
{
    public class PersonRepository : BaseRepository<Person>
    {
        public PersonRepository(INeo4jDBContext context) : base(context){
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            var found = await this.Single(p => p.Name == person.Name);
            if(found != null)
            {
               await Update(p => p.Name == person.Name, person);
               return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddPersonAsync(Person person)
        {
            var found = await this.Single(p => p.Name == person.Name);
            if(found == null)
            {
                // Each person that is added, their email will be default not be confirmed
                person.EmailConfirmed = "False";
                await Add(person);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            return await this.All();
        }

        public async Task<Person> GetPersonAsync(string personName)
        {
            return await this.Single(p=>p.Name == personName);
        }

        
        public async Task<Person> GetPersonByEmailAsync(string email)
        {
            return await this.Single(p=>p.Email == email);
        }

        public async Task<IEnumerable<Person>> GetPeopleWhereAsync(string profile)
        {
            return await this.Where(b=>b.Profile.Contains(profile));
        }

        public async Task DeletePersonAsync(Person person)
        {
            var found = await this.Single(p => p.Name == person.Name);
            if(found != null)
            {
               await this.Delete(p => p.Name == person.Name);
            }
        }

#region FriendsWith
        public async Task CreateFriendRelationshipAsync(Person person, Person friend, FriendsWith friendsWith)
        {
            // Creates relationship person1 -> person2
            await this.Relate<Person, FriendsWith>(p1=>p1.Name == person.Name, p2=>p2.Name == friend.Name, friendsWith);
        }

#endregion FriendsWith

#region InLibrary

        public async Task<IEnumerable<Book>> GetInLibraryBookRelationshipAsync(Person person, InLibrary inLibrary)
        {
           return await this.GetAllRelated(p=>p.Name == person.Name, new Book(), inLibrary);
        }

#endregion InLibrary

#region WishList
        public async Task<IEnumerable<Book>> GetWishListRelationshipAsync(Person person, WishList wishList)
        {
            return await this.GetAllRelated(p=>p.Name == person.Name, new Book(), wishList);
        }

#endregion WishList
    }
}