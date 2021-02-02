using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;
using System.Linq;

namespace ReadingRainbowAPI.DAL
{
    public class BookRepository : BaseRepository<Book>
    {
        public BookRepository(INeo4jDBContext context) : base(context){
        }

        public async Task AddOrUpdateAsync(Book book)
        {
            var found = await this.Single(p => p.Id == book.Id);
            if(found == null)
            {
                await Add(book);
            }
            else
            {
                await Update(p => p.Id == book.Id, book);
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await this.All();
        }

        public async Task<Book> GetBookAsync(string bookId)
        {
            return await this.Single(b=>b.Id == bookId);
        }

        public async Task<IEnumerable<Book>> GetBooksWhereAsync(string description)
        {
            return await this.Where(b=>b.Description.Contains(description));
        }

        public async Task DeleteBookAsync(Book book)
        {
            var found = await this.Single(p => p.Id == book.Id);
            if(found != null)
            {
               await this.Delete(p => p.Id == book.Id);
            }
        }

#region InLibrary Relationships

        public async Task CreateInLibraryRelationshipAsync(Book book, Person person, InLibrary inLibrary)
        {
            if((await this.GetRelated<Person, InLibrary>(b=>b.Id == book.Id, p=>p.Name == person.Name, inLibrary)).ToList().Count == 0)
            {
                await this.Relate<Person, InLibrary>(b=>b.Id == book.Id, p=>p.Name == person.Name, inLibrary);
            }
        }

        public async Task<IEnumerable<Person>> GetInLibraryPersonRelationshipAsync(Book book, InLibrary inLibrary)
        {
           return await this.GetAllRelated<Person, InLibrary>(b=>b.Id == book.Id, new Person(),  inLibrary);
        }

        public async Task DeleteInLibraryRelationshipAsync(Book book, Person person, InLibrary inLibrary)
        {
            await this.DeleteRelationship<Person, InLibrary>(b=>b.Id == book.Id, p=>p.Name == person.Name, inLibrary);
        }

#endregion InLibraryRelationships

#region WishList
        public async Task CreateWishlistRelationshipAsync(Book book, Person person, WishList wishList)
        {
            await this.Relate<Person, WishList>(b=>b.Id == book.Id, p=>p.Name == person.Name, wishList);
        }

        public async Task DeleteWishListRelationshipAsync(Book book, Person person, WishList wishList)
        {
            await this.DeleteRelationship<Person, WishList>(b=>b.Id == book.Id, p=>p.Name == person.Name, wishList);
        }
#endregion WishList

#region InGenre Relationships

        public async Task<IEnumerable<Genre>> GetInGenreBookRelationshipAsync(Book book, InGenre inGenre)
        {
           return await this.GetAllRelated<Genre, InGenre>(b=>b.Id == book.Id, new Genre(),  inGenre);
        }

#endregion InGenre

    }
}
