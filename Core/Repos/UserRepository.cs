using Core.Contexts;
using Core.Models.Auth;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repos
{
    public class UserRepository
    {
        protected readonly DbSet<User> _dbSet;
        protected readonly AuthDBContext _dbContext;
        protected readonly SimpleLoggerService _logger;
        protected readonly UserManager<User> _userManager;

        public UserRepository(
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
        /// Создаёт нового юзера в базе данных
        /// </summary>
        /// <param name="user">Объект юзера со всеми необходимыми данными</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns></returns>
        public async Task<bool> Create(User user, string password)
        {
            if (IsPasswordValid(password))
            {
                var res = await _userManager.CreateAsync(user, password);
                if (res.Succeeded)
                {
                    await _logger.LogAsync("Creating user", _logger.Success, "User created");
                    return true;
                }
            }
            await _logger.LogAsync("Creating user", _logger.Warning, "Password is too short");
            return false;
        }
        
        /// <summary>
        /// Возвращает список всех пользователей в базе данных
        /// </summary>
        /// <returns>Если успешно - массив пользователей, иначе - пустой массив</returns>
        public async Task<List<User>> GetAll()
        {
            var result = await _dbSet.AsNoTracking().ToListAsync();
            await _logger.LogAsync("Get all users", _logger.Success, $"Returned {result.Count} records");
            return result;
        }

        /// <summary>
        /// Возвращает пользователя с указанным ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Если успешно - пользователь, иначе - null</returns>
        public async Task<User?> GetById(int id)
        {
            var res = await _userManager.FindByIdAsync(id.ToString());
            await _logger.LogAsync("Get user by id", _logger.Success);
            return res;
        }

        /// <summary>
        /// Возвращает пользователя с указанным Email
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Если успешно - пользователь, иначе - null</returns>
        public async Task<User?> GetByEmail(string email)
        {
            var res = await _userManager.FindByEmailAsync(email);
            if (res != null)
                await _logger.LogAsync("Get user by email", _logger.Success);
            await _logger.LogAsync("Get user by email", _logger.Warning);
            return res;
        }

        /// <summary>
        /// Меняет пароль пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="currPassword">Актуальный пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns>Возвращает результат операции</returns>
        public async Task<bool> ChangePassword(int id, string currPassword, string newPassword)
        {
            var user = await GetById(id);
            if (user == null)
            {
                await _logger.LogAsync("Change user password", _logger.Warning, "User not found");
                return false;
            }
            
            var res = await _userManager.ChangePasswordAsync(user, currPassword, newPassword);
            if (res.Succeeded)
            {
                await _logger.LogAsync("Change user password", _logger.Success);
                return true;
            }
            await _logger.LogAsync("Change user password", _logger.Warning, "Password is not valid");
            return false;
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

            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                await _logger.LogAsync("Update user", _logger.Success);
                return user;
            }
            var errors = string.Join("; ", res.Errors.Select(x => x.Description));
            await _logger.LogAsync("Update user", _logger.Warning, errors);

            return null;
        }

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id">ID удаляемого пользователя</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> Delete(int id)
        {
            var user = await GetById(id);

            if (user is null)
            {
                await _logger.LogAsync("Delete user", _logger.Warning, "User not found");
                return false;
            }

            var res = await _userManager.DeleteAsync(user);
            if (res.Succeeded)
            {
                await _logger.LogAsync("Delete user", _logger.Success);
                return true;
            }
            var  errors = string.Join("; ", res.Errors.Select(x => x.Description));
            await _logger.LogAsync("Delete user", _logger.Warning, errors);
            return false;
        }
        
        private static bool IsPasswordValid(string password)
        {
            return password is { Length: >= 10 };
        }
    }
}
