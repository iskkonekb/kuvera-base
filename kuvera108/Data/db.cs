using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using kuvera108.Areas.SiteAdmin.Models;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace kuvera108.Data
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //Get
        TEntity FindById(string id);
        IEnumerable<TEntity> Get();
        TEntity Get(Func<TEntity, bool> predicate,
           params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetNT(Func<TEntity, bool> predicate);
        Task<TEntity> GetAsync(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
        //Create
        void Create(TEntity item);
        Task<int> CreateAsync(TEntity item);
        //Remove
        void Remove(TEntity item);
        Task<int> RemoveAsync(TEntity item);
        //Update
        void Update(TEntity item);
        Task<int> UpdateAsync(TEntity item);
    }

    public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
        public DbSet DbSet { get { return _dbSet; } }
        public EFGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> GetNT(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }
        public TEntity FindById(string id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Get(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetAsync(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return await query.FirstOrDefaultAsync();
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        public Task<int> CreateAsync(TEntity item)
        {
            _dbSet.Add(item);
            return _context.SaveChangesAsync();
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public Task<int> UpdateAsync(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
        public Task<int> RemoveAsync(TEntity item)
        {
            _dbSet.Remove(item);
            return _context.SaveChangesAsync();
        }
        public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}