using System.Linq.Expressions;
using System.Security.Claims;
using Core.Models;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Core.Repos
{
    public abstract class BaseRepository<TDbSet, TDbContext>
        where TDbSet : Entity
        where TDbContext : DbContext
    {
        /// <summary>
        /// Базовый репозиторий для работы с базами данных
        /// </summary>
        protected readonly TDbContext _context;
        protected readonly DbSet<TDbSet> _dbSet;
        protected readonly SimpleLoggerService _logger;
        protected readonly HttpContext? _httpContext;
        protected readonly int _userID;
        
        protected BaseRepository(TDbContext context, SimpleLoggerService logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = context.Set<TDbSet>();
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
            _ = int.TryParse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out _userID);
        }

        /// <summary>
        /// Метод для получения конкретной записи по его ID
        /// </summary>
        /// <param name="id">Идентификатор записи, которую нужно получить</param>
        /// <param name="includeSelectors">Селектор для навигационных полей</param>
        /// <returns>Если успешно - содержимое искомой записи таблицы, иначе - null</returns>
        public virtual async Task<TDbSet?> GetByIDAsync<TProperty>(int id, params Expression<Func<TDbSet, TProperty>>[] includeSelectors)
        {
            var callRes = _dbSet.AsQueryable();
            
            foreach (var includeSelector in includeSelectors)
            {
                callRes = callRes.Include(includeSelector);
            }

            var entity = await callRes.FirstOrDefaultAsync(a => a.Id == id);
            
            if (entity == null)
            {
                await _logger.LogAsync("Get by ID", "Return null", $"{typeof(TDbSet).Name} with ID = {id} not found");
                return null;
            }
            
            await _logger.LogAsync("Get by ID", "Success", $"Found {typeof(TDbSet).Name} with id {id}");
            return entity;
        }

        /// <summary>
        /// Метод для добавления записи в базу
        /// </summary>
        /// <param name="entity">Объект который нужно сохранить</param>
        /// <returns>Если успешно - полное содержимое сохранённого объекта, иначе - null</returns>
        public virtual async Task<TDbSet?> AddAsync(TDbSet entity)
        {
            try
            {
                entity.CreationDateTime = DateTime.UtcNow;
                entity.CreatorID = _userID;

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                await _logger.LogAsync($"Add new {typeof(TDbSet).Name}", "Success", $"New {typeof(TDbSet).Name} added with id {entity.Id}");

                return entity;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Add new {typeof(TDbSet).Name}", "Catch exception", $"{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Метод для изменения существующей записи в базе
        /// </summary>
        /// <param name="entity">Измененный объект</param>
        /// <returns>Если успешно - измененный объект, иначе - null</returns>
        public virtual async Task<TDbSet?> UpdateAsync(TDbSet entity)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (existingEntity == null)
            {
                await _logger.LogAsync($"Update {typeof(TDbSet).Name}", "Null reference", $"{typeof(TDbSet).Name} with id {entity.Id} not found");
                return null;
            }

            entity.LastEditedDateTime = DateTime.UtcNow;
            entity.LastEditorID = _userID;

            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
            await _logger.LogAsync($"Update {typeof(TDbSet).Name}", "Success", $"{typeof(TDbSet).Name} with id {entity.Id}");

            return existingEntity;
        }

        /// <summary>
        /// Метод для удаления записи из базы
        /// </summary>
        /// <param name="id">ID удаляемой записи</param>
        /// <returns>Если успешно - true, иначе - false</returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Null reference", $"{typeof(TDbSet).Name} with id {id} not found");
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Success",$"{typeof(TDbSet).Name} with if {id} was deleted");
            return true;
        }

        /// <summary>
        /// Метод для отметки записи как удаленной
        /// </summary>
        /// <param name="id">ID помечаемой записи</param>
        /// <returns>Если успешно - true, иначе - false</returns>
        public virtual async Task<bool> MarkAsDeletedAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
            if (entity == null)
            {
                await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Null reference", $"{typeof(TDbSet).Name} with id {id} not found");
                return false;
            }

            entity.DeletedDateTime = DateTime.UtcNow;
            entity.DeletedByID = _userID;
            
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync($"Delete {typeof(TDbSet).Name}", "Success",$"{typeof(TDbSet).Name} with id {id} was deleted");
            return true;
        }
        
        /// <summary>
        /// Метод для получения содержимого таблицы постранично
        /// </summary>
        /// <param name="pageNumber">Номер нужно страницы</param>
        /// <param name="pageSize">Количество записей на странице</param>
        /// <returns>Список записей, ограниченный переданными параметрами</returns>
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
