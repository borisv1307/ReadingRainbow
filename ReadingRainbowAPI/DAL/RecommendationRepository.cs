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
        public RecommendationRepository(INeo4jDBContext context) : base(context){
        }
#region Recommendations
        public async Task<IEnumerable<Book>> JaccardLibraryRecommendations (Person person)
        {
           return await this.GetJaccardLibrary(p=>p.Name == person.Name);
        }
        public async Task<IEnumerable<Book>> JaccardWishListRecommendations (Person person)
        {
           return await this.GetJaccardWishList(p=>p.Name == person.Name);
        }

        public async  Task<IEnumerable<Book>> PopularWishLists()
        {
            return await this.GetPopularWishList();
        }

        public async Task<IEnumerable<Book>> PopularLibrarys()
        {
            return await this.GetPopularLibrary();
        }

        
        public virtual async Task<IEnumerable<Book>> GetJaccardWishList (Expression<Func<Person, bool>> query)
        {
            string name1 = query.Parameters[0].Name;

            return await _neoContext.Cypher
                .OptionalMatch ("(p1:Person)-[WISH_LISTS]->(b:Book)<-[WISH_LISTS]-(p2:Person)")
                .Where ((Person p1) => p1.Name == "+ name1 +")
                .With ("p1, p2, COUNT(b) AS intersection, COLLECT(b) AS i")
                .Match ("(p1)-[WISH_LISTS]->(b1:Book)")
                .With ("p1, p2, intersection, i, COLLECT(b1.Id) AS w1")
                .Match ("(p2)-[:WISH_LISTS]->(b2:Book)")
                .With ("p1, p2, intersection, i, w1, COLLECT(b2.Id) AS w2")
                .With ("p1, p2, intersection, w1, w2")
                .With ("p1, p2, intersection, [y IN w2 WHERE NOT y IN w1] AS unique, w1+[x IN w2 WHERE NOT x IN w1] AS union, w1, w2")
                .Match ("(p1)-[:IN_LIBRARY]->(b3:Book)")
                .With ("p1, p2, intersection, unique, union, w1, w2, COLLECT(b3.Id) AS l1")
                .Return ((unique, l1, union, intersection) => new{
                    Jaccard = Return.As<double>("((1.0*intersection/SIZE(union)))")
                    JaccardRecommendations = Return.As<Book>("[z IN unique WHERE NOT z in l1]")
                })
                .OrderByDescending("Jaccard")
                .Limit(5)
                .ResultsAsync;   
        }

        public virtual async Task<IEnumerable<Book>> GetJaccardLibrary (Expression<Func<Person, bool>> query)
        {
            string name1 = query.Parameters[0].Name;

            return await _neoContext.Cypher
                .OptionalMatch ("(p1:Person)-[IN_LIBRARY]->(b:Book)<-[IN_LIBRARY]-(p2:Person)")
                .Where ((Person p1) => p1.Name == "+ name1 +")
                .With ("p1, p2, COUNT(b) AS intersection, COLLECT(b) AS i")
                .Match ("(p1)-[IN_LIBRARY]->(b1:Book)")
                .With ("p1, p2, intersection, i, COLLECT(b1.Id) AS w1")
                .Match ("(p2)-[:IN_LIBRARY]->(b2:Book)")
                .With ("p1, p2, intersection, i, w1, COLLECT(b2.Id) AS w2")
                .With ("p1, p2, intersection, w1, w2")
                .With ("p1, p2, intersection, [y IN w2 WHERE NOT y IN w1] AS unique, w1+[x IN w2 WHERE NOT x IN w1] AS union, w1, w2")
                .Match ("(p1)-[:WISH_LISTS]->(b3:Book)")
                .With ("p1, p2, intersection, unique, union, w1, w2, COLLECT(b3.Id) AS l1")
                .Return ((unique, l1, union, intersection) => new{
                    Jaccard = Return.As<double>("((1.0*intersection/SIZE(union)))")
                    JaccardRecommendations = Return.As<Book>("[z IN unique WHERE NOT z in l1]")
                })
                .OrderByDescending("Jaccard")
                .Limit(5)
                .ResultsAsync;
        }


        public virtual async Task<IEnumerable<Book>> GetPopularWishList()
        {
            return await _neoContext.Cypher
                .Match ("(person:Person)-[r:WISH_LISTS]->(book:Book)")
                .With ("book, COUNT(r) AS popularity")
                .Return ((book, popularity) => new{
                    Book = book.As<Book>(),
                    Popularity = popularity.As<int>() }) 
                .OrderByDescending(popularity)
                .Limit(1)
                .ResultsAsync;
        }

        public virtual async Task<IEnumerable<Book>> GetPopularLibrary()
        {
            return await _neoContext.Cypher
                .Match ("(person:Person)-[r:IN_LIBRARY]->(book:Book)")
                .With ("book, COUNT(r) AS popularity")
                .Return ((book, popularity) => new{
                    Book = book.As<Book>(),
                    Popularity = popularity.As<int>() })
                .OrderByDescending(popularity)
                .Limit(1)
                .ResultsAsync;
        }

#endregion Recommendations
    }
}