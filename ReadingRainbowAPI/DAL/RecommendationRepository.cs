using ReadingRainbowAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReadingRainbowAPI.Relationships;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace ReadingRainbowAPI.DAL
{
    public class RecommendationRepository : BaseRepository<Book>
    {
        public RecommendationRepository(INeo4jDBContext context) : base(context)
        {
        }
        #region Recommendations
        public async Task<IEnumerable<JaccardRec>> JaccardLibraryRecommendations(Person person)
        {
            return await this.GetJaccardLibrary(p => p.Name == person.Name);
        }
        public async Task<IEnumerable<JaccardRec>> JaccardWishListRecommendations(Person person)
        {
            return await this.GetJaccardWishList(p => p.Name == person.Name);
        }

        public async Task<IEnumerable<PopularityResult>> PopularWishLists()
        {
            return await this.GetPopularWishList();
        }

        public async Task<IEnumerable<PopularityResult>> PopularLibrarys()
        {
            return await this.GetPopularLibrary();
        }

        #endregion Recommendations
    }
}