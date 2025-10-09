using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repos
{
    public abstract class BaseRepository<TDbSet, TDbContext> : IRepository<TDbSet> where TDbSet : Entity
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        protected readonly DbSet<TDbSet> _dbSet;
        protected BaseRepository(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TDbSet>();
        }

        public virtual async Task<TDbSet> GetByIDAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
        }

        public virtual async Task<TDbSet> AddAsync(TDbSet entity, int userId)
        {
            entity.CreationDateTime = DateTime.UtcNow;
            entity.CreatorID = userId;

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TDbSet> UpdateAsync(TDbSet entity, int userId)
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id);
            if (existingEntity == null)
                throw new ArgumentException($"Entity with ID {entity.Id} not found");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            existingEntity.LastEditedDateTime = DateTime.UtcNow;
            existingEntity.LastEditorID = userId;

            await _context.SaveChangesAsync();
            return existingEntity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<IEnumerable<TDbSet>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.OrderBy(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
