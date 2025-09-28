namespace BaseLib;

/// <summary>
/// Общий интерфейс для математических библиотек
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IMathLibrary<TRequest, TResponse>
{
    /// <summary>
    /// Базовый 
    /// </summary>
    /// <param name="request">Модель запроса к библиотеке</param>
    /// <returns>Модель ответа библиотеки</returns>
    public TResponse Calulate(TRequest request);

    public void Initiate(); // Опционально
    
    public void Initiate(TRequest request); // Так же опционально
}