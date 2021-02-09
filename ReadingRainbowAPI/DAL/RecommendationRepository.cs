using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;
using System.Linq;

namespace ReadingRainbowAPI.DAL
{
    public class RecommendationRepository : BaseRepository<Book>
    {
        public RecommendationRepository(INeo4jDBContext context) : base(context){
        }
#region Recommendations
        public async Task<IEnumerable<Book>> JaccardLibraryRecommendations (Person person)
        {
           return await this.GetJaccardLibrary<Person>(p=>p.Name == person.Name);
        }
        public async Task<IEnumerable<Book>> JaccardWishListRecommendations (Person person)
        {
           return await this.GetJaccardWishList<Person>(p=>p.Name == person.Name);
        }

        public async  Task<IEnumerable<Book>> PopularWishLists()
        {
            return await this.GetPopularWishList();
        }

        public async Task<IEnumerable<Book>> PopularLibrarys()
        {
            return await this.GetPopularLibrary();
        }

#endregion Recommendations
    }
}