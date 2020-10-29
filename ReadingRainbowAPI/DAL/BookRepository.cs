using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;

namespace ReadingRainbowAPI.DAL
{
    public class BookRepository : BaseRepository<Book>
    {
        public async Task AddOrUpdate(Book book)
        {
            var found = await this.Single(p => p.Id == book.Id && p.Title == book.Title);
            if(found == null)
            {
                await Add(book);
            }
            else
            {
               await Update(p => p.Id == book.Id && p.Title == book.Title, book);
            }
        }

        public async Task<IEnumerable<Book>> GetLibrary(Person person, InLibrary inLibrary)
        {
            
           return await this.GetRelated(p => p.Id == person.Name, inLibrary, new List<Book>());
        }
    }
}
