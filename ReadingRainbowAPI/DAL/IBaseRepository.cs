using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ReadingRainbowAPI.DAL
{

    public interface IBaseRepository<TEntity>
    {       
        Task<IEnumerable<TEntity>> All();
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> query);
        Task<TEntity> Single(Expression<Func<TEntity, bool>> query);
        Task Add(TEntity item);
        Task Update(Expression<Func<TEntity, bool>> query, TEntity newItem);
    }
}