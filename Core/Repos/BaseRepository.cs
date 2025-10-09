using Core.Models;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected readonly SimpleLoggerService _logger;
        protected BaseRepository(TDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TDbSet>();
            _logger = new SimpleLoggerService();
        }

        public virtual async Task<TDbSet> GetByIDAsync(int id)
        {
            var res = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);

            if (res == null)
            {
                await _logger.LogAsync("Get by ID", "Return null", $"ID = {id}");
                throw new NullReferenceException();
            }

            await _logger.LogAsync("Get by ID", "Success", $"Found item with id {id}");
            return res;
        }

        public virtual async Task<TDbSet> AddAsync(TDbSet entity, int userId)
        {
            try
            {
                entity.CreationDateTime = DateTime.UtcNow;
                entity.CreatorID = userId;

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                await _logger.LogAsync("Add new item", "Success", $"New item added with id {entity.Id}");

                return entity;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync("Add new item", "Catch exception", $"{ex.Message}");
                return entity;
            }
        }

        public virtual async Task<TDbSet> UpdateAsync(TDbSet entity, int userId)
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id);
            if (existingEntity == null)
            {
                await _logger.LogAsync("Update item", "Null reference", $"Item with id {entity.Id} not found");
                throw new NullReferenceException();
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            existingEntity.LastEditedDateTime = DateTime.UtcNow;
            existingEntity.LastEditorID = userId;

            await _context.SaveChangesAsync();
            await _logger.LogAsync("Update item", "Success", $"Item with id {entity.Id}");

            return existingEntity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                await _logger.LogAsync("Delete item", "Null reference", $"item with id {id} not found");
                throw new NullReferenceException();
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync("Delete item","Success",$"Iten with if {id} was deleted");
            return true;
        }

        public virtual async Task<IEnumerable<TDbSet>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var res = await _dbSet
                .OrderBy(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            await _logger.LogAsync("Get paged", "Success", $"Returned page {pageNumber}");
            return res;
        }
    }
}
