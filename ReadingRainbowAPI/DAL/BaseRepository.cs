using System.Threading.Tasks;
using System.Collections.Generic;
using System;

// Repo takes in database context as a constructor, uses context 
// Provides business logic around context add/insert objects
namespace ReadingRainbowAPI.DAL
{
    public abstract class BaseRepository : IBaseRepository
    {
      protected readonly INeo4jDBContext _neoContext;
 
        protected BaseRepository(INeo4jDBContext context)
        {
            _neoContext = context;
        }

/*
        public async Task CreateBook(Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException("Book was not entered");
            }
            // _dbCollection = _neoContext.GetCollection<TEntity>(typeof(TEntity).Name);
            await _neoContext.InsertOneAsync(book);
        }
 
        public void Delete(string id)
        {
            //ex. 5dc1039a1521eaa36835e541
 
            var objectId = new ObjectId(id);
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
 
        }
        public virtual void Update(TEntity obj)
        {
            _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj);
        }
 
        public async Task<TEntity> Get(string id)
        {
            var objectId = new ObjectId(id);
 
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
 
            _dbCollection = _neoContext.GetCollection<TEntity>(typeof(TEntity).Name);
 
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
 
        }
 
 
        public async Task<IEnumerable<TEntity>> Get()
        {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        } 
    } */
    }
}