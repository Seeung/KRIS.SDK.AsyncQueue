namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Задача с результатом
    /// </summary>
    /// <typeparam name="TReturnType">Тип результата</typeparam>
    public class AsyncTaskInfoReturnTyped<TReturnType> : AsyncTaskInfo
    {
        /// <summary>
        /// Получить результат
        /// </summary>
        /// <returns></returns>
        public TReturnType Return() => GetResult<TReturnType>();
    }
}
