namespace BaseLib;

/// <summary>
/// Общий интерфейс для математических библиотек
/// </summary>
/// <typeparam name="TRequest">Модель с входными данными</typeparam>
/// <typeparam name="TResponse">Модель с выходными данными</typeparam>
public interface IMathLibrary<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    /// <summary>
    /// Базовый 
    /// </summary>
    /// <param name="request">Модель запроса к библиотеке</param>
    /// <returns>Модель ответа библиотеки</returns>
    public TResponse Calculate(TRequest request);
}