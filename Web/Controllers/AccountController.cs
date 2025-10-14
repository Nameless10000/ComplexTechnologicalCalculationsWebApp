using System.Threading.Tasks;
using Core.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Auth;

namespace Web.Controllers
{
    // Контроллер для управления аккаунтом: регистрация, вход, выход, восстановление пароля
    public class AccountController : Controller
    {
        // UserManager отвечает за управление пользователями: создание, редактирование, поиск и проверка пароля
        private readonly UserManager<User> _userManager;

        // SignInManager отвечает за аутентификацию: вход, выход и cookie-сессии
        private readonly SignInManager<User> _signInManager;

        // Внедрение зависимостей через конструктор
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register

        // GET: /Account/Register
        // Отображение формы регистрации
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        // Обработка данных формы регистрации
        [HttpPost]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string phoneNumber,
            string password,
            string confirmPassword,
            bool acceptPolicy)
        {
            // Проверка согласия пользователя на обработку персональных данных
            if (!acceptPolicy)
            {
                ModelState.AddModelError("", "Необходимо согласие на обработку персональных данных");
                return View();
            }

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают");
                return View();
            }

            // Создание нового объекта пользователя
            var user = new User
            {
                UserName = email,       // логин пользователя
                Email = email,           // email
                PhoneNumber = phoneNumber,
                FullName = fullName
            };

            // Создание пользователя в базе с хешированием пароля
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Если регистрация успешна, автоматически авторизуем пользователя
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Если произошли ошибки (например, email уже занят), добавляем их в ModelState
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        #endregion

        #region Login

        // GET: /Account/Login
        // Отображение формы входа
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        // Обработка данных формы входа
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserSignInViewModel model)
        {
            // Проверяем, что пользователь что-то ввёл
            if (!ModelState.IsValid)
                return View(model);

            // Пытаемся выполнить вход через SignInManager
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Если вход успешен — отправляем пользователя на главную страницу
                return RedirectToAction("Index", "Home");
            }

            // Если ошибка — добавляем сообщение и возвращаем форму
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        #endregion

        #region Logout

        // POST: /Account/Logout
        // Выход из аккаунта
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Завершаем сессию пользователя, удаляя аутентификационные cookie
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        #endregion

        #region ForgotPassword (заготовка)

        // GET: /Account/ForgotPassword
        // Отображение формы для восстановления пароля
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        // Здесь можно добавить логику восстановления пароля через email
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // TODO: отправка ссылки для сброса пароля
            return RedirectToAction("Login");
        }

        #endregion
    }
}
