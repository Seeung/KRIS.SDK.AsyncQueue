namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Задача с результатом и параметрами
    /// </summary>
    /// <typeparam name="TParameters">Тип параметра</typeparam>
    /// <typeparam name="TReturnType">Тип результата</typeparam>
    public class AsyncTaskInfoParametrizedAndReturnTyped<TParameters, TReturnType> : AsyncTaskInfoParametrized<TParameters>
    {
        /// <summary>
        /// Получить результат
        /// </summary>
        /// <returns></returns>
        public TReturnType Return() => GetResult<TReturnType>();
    }
}
