// Adapted from https://github.com/pcmantinker/Neo4jRepository

using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using System.Linq;
using System.Linq.Expressions;
using Neo4jClient;

// Repo takes in database context as a constructor, uses context for database client connection
namespace ReadingRainbowAPI.DAL
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : Neo4jEntity, IEntity, new()
    {
        protected readonly IGraphClient _neoContext;

        protected BaseRepository(INeo4jDBContext context)
        {
            _neoContext = context.GetClient();
        }

        // Get All Entity Values
        public virtual async Task<IEnumerable<TEntity>> All()
        {
            TEntity entity = new TEntity();

            return await _neoContext.Cypher
                .Match("(e:" + entity.Label + ")")
                .Return(e => e.As<TEntity>())
                .ResultsAsync;
        }

        public virtual async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> query)
        {
            string name = query.Parameters[0].Name;
            TEntity entity = (TEntity)Activator.CreateInstance(query.Parameters[0].Type);
            Expression<Func<TEntity, bool>> newQuery = PredicateRewriter.Rewrite(query, "e");

            return await _neoContext.Cypher
                .Match("(e:" + entity.Label + ")")
                .Where(newQuery)
                .Return(e => e.As<TEntity>())
                .ResultsAsync;
        }

        public virtual async Task<TEntity> Single(Expression<Func<TEntity, bool>> query)
        {
            IEnumerable<TEntity> results = await Where(query);
            return results.FirstOrDefault();
        }

        // Adds Entity to table
        public virtual async Task<bool> Add(TEntity item)
        {
            await _neoContext.Cypher
                    .Create("(e:" + item.Label + " $item)")
                    .WithParam("item", item)
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public virtual async Task<bool> Update(Expression<Func<TEntity, bool>> query, TEntity newItem)
        {
            string name = query.Parameters[0].Name;

            TEntity itemToUpdate = await this.Single(query);
            this.CopyValues(itemToUpdate, newItem);

            await _neoContext.Cypher
               .Match("(" + name + ":" + newItem.Label + ")")
               .Where(query)
               .Set(name + " = $item")
               .WithParam("item", itemToUpdate)
               .ExecuteWithoutResultsAsync();

            return true;
        }

        private void CopyValues(TEntity target, TEntity source)
        {
            Type t = typeof(TEntity);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }

        public virtual async Task<bool> Delete(Expression<Func<TEntity, bool>> query)
        {
            string name = query.Parameters[0].Name;
            TEntity entity = (TEntity)Activator.CreateInstance(query.Parameters[0].Type);

            await _neoContext.Cypher
                .Match("(" + name + ":" + entity.Label + ")")
                .Where(query)
                .DetachDelete(name)
                .ExecuteWithoutResultsAsync();

            return true;
        }

        public virtual async Task<bool> Relate<TEntity2, TRelationship>(Expression<Func<TEntity, bool>> query1,
        Expression<Func<TEntity2, bool>> query2, TRelationship relationship)
            where TEntity2 : Neo4jEntity, new()
            where TRelationship : Neo4jRelationship, new()
        {
            string name1 = query1.Parameters[0].Name;
            TEntity entity1 = (TEntity)Activator.CreateInstance(query1.Parameters[0].Type);
            string name2 = query2.Parameters[0].Name;
            TEntity2 entity2 = (TEntity2)Activator.CreateInstance(query2.Parameters[0].Type);

            object properties = new object();

            await _neoContext.Cypher
                .Match("(" + name1 + ":" + entity1.Label + ")", "(" + name2 + ":" + entity2.Label + ")")
                .Where(query1)
                .AndWhere(query2)
                .Create("(" + name1 + ")-[r:" + relationship.Name + "]->(" + name2 + ")")
                .ExecuteWithoutResultsAsync();

            return true;
        }


        public virtual async Task<IEnumerable<TEntity2>> GetRelated<TEntity2, TRelationship>(Expression<Func<TEntity, bool>> query1, Expression<Func<TEntity2, bool>> query2, TRelationship relationship)
            where TEntity2 : Neo4jEntity, new()
            where TRelationship : Neo4jRelationship, new()
        {
            string name1 = query1.Parameters[0].Name;
            TEntity entity1 = (TEntity)Activator.CreateInstance(query1.Parameters[0].Type);
            string name2 = query2.Parameters[0].Name;
            TEntity2 entity2 = (TEntity2)Activator.CreateInstance(query2.Parameters[0].Type);

            Expression<Func<TEntity2, bool>> newQuery = PredicateRewriter.Rewrite(query2, "e");

            try
            {
                return await _neoContext.Cypher
                    .Match("(" + name1 + ":" + entity1.Label + ")-[:" + relationship.Name + "]-(" + name2 + ":" + entity2.Label + ")")
                    .Where(query1)
                    .AndWhere(query2)
                    .Return(e => e.As<TEntity2>())
                    .ResultsAsync;
            }
            catch (Exception)
            {
                return new List<TEntity2>();
            }
        }

        public virtual async Task<IEnumerable<TEntity2>> GetAllRelated<TEntity2, TRelationship>(Expression<Func<TEntity, bool>> query1, TEntity2 entity2, TRelationship relationship)
            where TEntity2 : Neo4jEntity, new()
            where TRelationship : Neo4jRelationship, new()
        {
            string name1 = query1.Parameters[0].Name;
            TEntity entity1 = (TEntity)Activator.CreateInstance(query1.Parameters[0].Type);


            return await _neoContext.Cypher
                .Match("(" + name1 + ":" + entity1.Label + ")-[:" + relationship.Name + "]-(e:" + entity2.Label + ")")
                .Where(query1)
                .Return(e => e.As<TEntity2>())
                .ResultsAsync;
        }

        public virtual async Task<bool> DeleteRelationship<TEntity2, TRelationship>(Expression<Func<TEntity, bool>> query1, Expression<Func<TEntity2, bool>> query2, TRelationship relationship)
            where TEntity2 : Neo4jEntity, new()
            where TRelationship : Neo4jRelationship, new()
        {
            string name1 = query1.Parameters[0].Name;
            TEntity entity1 = (TEntity)Activator.CreateInstance(query1.Parameters[0].Type);
            string name2 = query2.Parameters[0].Name;
            TEntity2 entity2 = (TEntity2)Activator.CreateInstance(query2.Parameters[0].Type);

            await _neoContext.Cypher
                .Match("(" + name1 + ":" + entity1.Label + ")-[r:" + relationship.Name + "]->(" + name2 + ":" + entity2.Label + ")")
                .Where(query1)
                .AndWhere(query2)
                .Delete("r")
                .ExecuteWithoutResultsAsync();

            return true;
        }

        public virtual async Task<IEnumerable<PopularityResult>> GetPopularWishList()
        {
            return await _neoContext.Cypher
            .Match("(person:Person)-[r:WISH_LISTS]->(book:Book)")
            .With("book, COUNT(r) AS popularity")
            .Return((book, popularity) => new PopularityResult
            {
                Book = book.As<Book>(),
                Popularity = popularity.As<int>()
            })
            .OrderByDescending(nameof(PopularityResult.Popularity))
            .Limit(10)
            .ResultsAsync;
        }

        public virtual async Task<IEnumerable<PopularityResult>> GetPopularLibrary()
        {
            return await _neoContext.Cypher
            .Match("(person:Person)-[r:IN_LIBRARY]->(book:Book)")
            .With("book, COUNT(r) AS popularity")
            .Return((book, popularity) => new PopularityResult
            {
                Book = book.As<Book>(),
                Popularity = popularity.As<int>()
            })
            .OrderByDescending(nameof(PopularityResult.Popularity))
            .Limit(10)
            .ResultsAsync;
        }

        public virtual async Task<IEnumerable<JaccardRec>> GetJaccardLibrary(Expression<Func<Person, bool>> query)
        {
            string name1 = query.Parameters[0].Name; //<---obtain user's name

            return await _neoContext.Cypher
            .OptionalMatch("(p1:Person)-[IN_LIBRARY]->(b:Book)<-[IN_LIBRARY]-(p2:Person)")
            .Where((Person p1) => p1.Name == "+ name1 +")
            .With("p1, p2, COUNT(b) AS intersection, COLLECT(b) AS i")
            .Match("(p1)-[IN_LIBRARY]->(b1:Book)")
            .With("p1, p2, intersection, i, COLLECT(b1) AS w1")
            .Match("(p2)-[:IN_LIBRARY]->(b2:Book)")
            .With("p1, p2, intersection, i, w1, COLLECT(b2) AS w2")
            .With("p1, p2, intersection, w1, w2")
            .With("p1, p2, intersection, [y IN w2 WHERE NOT y IN w1] AS unique, w1+[x IN w2 WHERE NOT x IN w1] AS union, w1, w2")
            .Match("(p1)-[:WISH_LISTS]->(b3:Book)")
            .With("p1, p2, intersection, unique, union, w1, w2, COLLECT(b3) AS l1")
            .With("p1, p2, intersection, unique, union, w1, w2, [z IN unique WHERE NOT z in l1] AS bookList")
            .With("p1, p2, intersection, unique, union, w1, w2, l1, bookList, ((1.0*intersection/SIZE(union))) AS jaccardIndex")
            .Where("jaccard > 0.2")
            .Return((jaccardIndex, bookList) => new JaccardRec
            {
                JaccardIndex = jaccardIndex.As<double>(),
                BookList = bookList.As<BookList>()
            })
            .OrderByDescending(nameof(JaccardRec.JaccardIndex))
            .Limit(10)
            .ResultsAsync;
        }
        public virtual async Task<IEnumerable<JaccardRec>> GetJaccardWishList(Expression<Func<Person, bool>> query)
        {
            string name1 = query.Parameters[0].Name; //<---obtain user's name

            return await _neoContext.Cypher
            .OptionalMatch("(p1:Person)-[WISH_LISTS]->(b:Book)<-[WISH_LISTS]-(p2:Person)")
            .Where((Person p1) => p1.Name == "+ name1 +")
            .With("p1, p2, COUNT(b) AS intersection, COLLECT(b) AS i")
            .Match("(p1)-[WISH_LISTS]->(b1:Book)")
            .With("p1, p2, intersection, i, COLLECT(b1) AS w1")
            .Match("(p2)-[:WISH_LISTS]->(b2:Book)")
            .With("p1, p2, intersection, i, w1, COLLECT(b2) AS w2")
            .With("p1, p2, intersection, w1, w2")
            .With("p1, p2, intersection, [y IN w2 WHERE NOT y IN w1] AS unique, w1+[x IN w2 WHERE NOT x IN w1] AS union, w1, w2")
            .Match("(p1)-[:IN_LIBRARY]->(b3:Book)")
            .With("p1, p2, intersection, unique, union, w1, w2, COLLECT(b3) AS l1")
            .With("p1, p2, intersection, unique, union, w1, w2, [z IN unique WHERE NOT z in l1] AS bookList")
            .With("p1, p2, intersection, unique, union, w1, w2, l1, bookList, ((1.0*intersection/SIZE(union))) AS jaccardIndex")
            .Where("jaccard > 0.2")
            .Return((jaccardIndex, bookList) => new JaccardRec
            {
                JaccardIndex = jaccardIndex.As<double>(),
                BookList = bookList.As<BookList>()
            })
            .OrderByDescending(nameof(JaccardRec.JaccardIndex))
            .Limit(10)
            .ResultsAsync;
        }
    }
}