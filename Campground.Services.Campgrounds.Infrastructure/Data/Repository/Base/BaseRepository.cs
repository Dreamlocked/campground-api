using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure.Data.Repository.Base
{
    public class BaseRepository<T>(CampgroundContext dbContext) : IBaseRepository<T> where T : class
    {
        protected readonly CampgroundContext _dbContext = dbContext;

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var entities = _dbContext.Set<T>().Where(filter);

            _dbContext.Set<T>().RemoveRange(entities);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? top = null,
            int? skip = null,
            params string[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null) query = query.Where(filter);

            if (includeProperties.Length > 0) query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));

            if (orderBy != null) query = orderBy(query);

            if (skip.HasValue) query = query.Skip(skip.Value);

            if (top.HasValue) query = query.Take(top.Value);

            return await query.ToListAsync();
        }

    }
}
