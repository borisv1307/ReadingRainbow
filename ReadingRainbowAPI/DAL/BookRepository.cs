using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;
using Neo4jClient;

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
    }
}
