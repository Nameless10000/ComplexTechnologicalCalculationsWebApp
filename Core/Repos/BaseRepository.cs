using Core.Models;
using Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Repos
{
    public abstract class BaseRepository<TDbSet, TDbContext>
        where TDbSet : Entity
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        protected readonly DbSet<TDbSet> _dbSet;
        protected readonly SimpleLoggerService _logger;
        protected BaseRepository(TDbContext context, SimpleLoggerService logger)
        {
            _context = context;
            _dbSet = context.Set<TDbSet>();
            _logger = logger;
        }

        public virtual async Task<TDbSet> GetByIDAsync(int id)
        {
            var res = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);

            if (res == null)
            {
                await _logger.LogAsync("Get by ID", "Return null", $"{typeof(TDbSet).Name} with ID = {id} not found");
                throw new NullReferenceException();
            }

            await _logger.LogAsync("Get by ID", "Success", $"Found {typeof(TDbSet).Name} with id {id}");
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
                await _logger.LogAsync($"Add new {typeof(TDbSet).Name}", "Success", $"New {typeof(TDbSet).Name} added with id {entity.Id}");

                return entity;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Add new {typeof(TDbSet).Name}", "Catch exception", $"{ex.Message}");
                return entity;
            }
        }

        public virtual async Task<TDbSet> UpdateAsync(TDbSet entity, int userId)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (existingEntity == null)
            {
                await _logger.LogAsync($"Update {typeof(TDbSet).Name}", "Null reference", $"{typeof(TDbSet).Name} with id {entity.Id} not found");
                throw new NullReferenceException();
            }

            entity.LastEditedDateTime = DateTime.UtcNow;
            entity.LastEditorID = userId;

            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
            await _logger.LogAsync($"Update {typeof(TDbSet).Name}", "Success", $"{typeof(TDbSet).Name} with id {entity.Id}");

            return existingEntity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Null reference", $"{typeof(TDbSet).Name} with id {id} not found");
                throw new NullReferenceException();
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Success",$"{typeof(TDbSet).Name} with if {id} was deleted");
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
