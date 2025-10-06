using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseLib.Helpers;

/// <summary>
/// Класс - хелпер по работе с моделями
/// </summary>
public static class DisplayNameHelper
{
    /// <summary>
    /// Метод для получения значения аттрибута DisplayName у свойства конкретного типа
    /// Пример использования: user.GetDisplayName("Name") --> "Имя"
    /// </summary>
    /// <param name="value">Сам объект</param>
    /// <param name="propertyName">Имя свойства</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <returns>Значение аттрибута DisplayName для заданного свойства типа</returns>
    /// <exception cref="ArgumentOutOfRangeException">Свойство с указанным названием не найдено</exception>
    public static string GetDisplayName<T>(this T value, string propertyName)
    {
        var propertyInfo = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(x => x.Name == propertyName);

        return propertyInfo?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
               ?? propertyInfo?.Name
               ?? throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Метод для получения значения аттрибута DisplayName у свойства конкретного типа
    /// Пример использования: user.GetDisplayName(x => x.Name) --> "Имя"
    /// </summary>
    /// <param name="value">Сам объект</param>
    /// <param name="propertySelector">Делегат для выбора свойства</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <returns>Значение аттрибута DisplayName для заданного свойства типа</returns>
    /// <exception cref="ArgumentException">Переданный делегат не указывает на свойство типа</exception>
    public static string GetDisplayName<T, TProperty>(this T value, Expression<Func<T, TProperty>> propertySelector)
    {
        if (propertySelector.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression must be a member expression", nameof(propertySelector));
        }

        var propertyName = memberExpression.Member.Name;

        return value.GetDisplayName(propertyName);
    }
}