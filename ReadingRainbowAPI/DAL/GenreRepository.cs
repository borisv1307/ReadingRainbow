using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;

namespace ReadingRainbowAPI.DAL
{
    public class GenreRepository : BaseRepository<Genre>
    {
        public GenreRepository(INeo4jDBContext context) : base(context){
        }

        public async Task AddOrUpdateGenreAsync(Genre genre)
        {
            var found = await this.Single(p => p.Name == genre.Name);
            if(found == null)
            {
                await Add(genre);
            }
            else
            {
                await Update(p => p.Name == genre.Name, genre);
            }
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await this.All();
        }

        public async Task<Genre> GetGenreAsync(string genreName)
        {
            return await this.Single(b=>b.Name == genreName);
        }

        public async Task<IEnumerable<Genre>> GetGenresWhereAsync(string description)
        {
            return await this.Where(b=>b.Description.Contains(description));
        }

        public async Task DeleteGenreAsync(Genre genre)
        {
            var found = await this.Single(p => p.Name == genre.Name);
            if(found != null)
            {
               await this.Delete(p => p.Name == genre.Name);
            }
        }

#region InGenre Relationships

        public async Task CreateInGenreRelationshipAsync(Genre genre, Book book, InGenre inGenre)
        {
            if(!(await this.CheckRelated<Book, InGenre>(g=>g.Name == genre.Name, b=>b.Id == book.Id, inGenre)))
            {
                await this.Relate<Book, InGenre>(g=>g.Name == genre.Name, b=>b.Id == book.Id, inGenre);                
            }
        }

        public async Task<IEnumerable<Book>> GetInGenreBookRelationshipAsync(Genre genre, InGenre inGenre)
        {
           return await this.GetAllRelated<Book, InGenre>(g=>g.Name == genre.Name, new Book(),  inGenre);
        }

        public async Task DeleteInGenreRelationshipAsync(Genre genre, Book book, InGenre inGenre)
        {
            await this.DeleteRelationship<Book, InGenre>(g=>g.Name == genre.Name, b=>b.Id == book.Id, inGenre);
        }

#endregion InGenre

    }
}
