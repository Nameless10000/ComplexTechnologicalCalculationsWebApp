using Core.Contexts;
using Core.Models.Auth;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repos
{
    public class UserRepository
    {
        protected readonly DbSet<User> _dbSet;
        protected readonly AuthDBContext _dbContext;
        protected readonly SimpleLoggerService _logger;
        protected readonly UserManager<User> _userManager;

        protected UserRepository(
            AuthDBContext context,
            SimpleLoggerService logger,
            UserManager<User> userManager)
        {
            _dbSet = context.Users;
            _logger = logger;
            _userManager = userManager;
            _dbContext = context;
        }

        /// <summary>
        /// Возвращает список всех пользователей в базе данных
        /// </summary>
        /// <returns>Если успешно - массив польщователей, иначе - пустой массив</returns>
        public async Task<List<User>> GetAll()
        {
            var res = await _dbSet.ToListAsync();
            await _logger.LogAsync("Get all users", _logger.Success, $"Returned {res.Count} records");
            return res;
        }

        /// <summary>
        /// ВОзвращает пользователя с указанным ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Если успешно - пользователь, иначе - null</returns>
        public async Task<User?> GetByID(int id)
        {
            var res = await _userManager.FindByIdAsync(id.ToString());
            await _logger.LogAsync("Get user by id", _logger.Success);
            return res;
        }

        /// <summary>
        /// ВОзвращает пользователя с указанным Email
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Если успешно - пользователь, иначе - null</returns>
        public async Task<User?> GetByEmail(string email)
        {
            var res = await _userManager.FindByEmailAsync(email);
            await _logger.LogAsync("Get user by email", _logger.Success);
            return res;
        }

        /// <summary>
        /// Меняет пароль пользователя
        /// </summary>
        /// <param name="id">ID польщователя</param>
        /// <param name="currPassword">Актуальный пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns>Возвращает результат операции</returns>
        public async Task<bool> ChangePassword(int id, string currPassword, string newPassword)
        {
            var user = await GetByID(id);
            if (user == null)
            {
                await _logger.LogAsync("Change user password", _logger.Warning, "User not found");
                return false;
            }

            if (currPassword is not { Length: >= 10} || newPassword is not { Length: >= 10 })
            {
                await _logger.LogAsync("Change user password", _logger.Warning, "Password too short or null");
                return false;
            }

            await _userManager.ChangePasswordAsync(user, currPassword, newPassword);
            await _logger.LogAsync("Change user password", _logger.Success);
            return true;
        }

        /// <summary>
        /// Меняет данные пользователя
        /// </summary>
        /// <param name="user">Измененный объект</param>
        /// <returns>Если успешно - изменённый объект, иначе - null</returns>
        public async Task<User?> Update(User user)
        {
            var tracked = _dbContext.ChangeTracker.Entries<User>()
                .FirstOrDefault(x => x.Entity.Id == user.Id);

            if (tracked != null)
            {
                tracked.State = EntityState.Detached;
            }

            await _userManager.UpdateAsync(user);
            await _logger.LogAsync("Update user", _logger.Success);
            return user;
        }

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id">ID удаляемого пользователя</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> Delete(int id)
        {
            var user = await GetByID(id);

            if (user is null)
            {
                await _logger.LogAsync("Delete user", _logger.Warning, "User not found");
                return false;
            }

            await _userManager.DeleteAsync(user);
            await _logger.LogAsync("Delete user", _logger.Success);
            return true;
        }
    }
}
