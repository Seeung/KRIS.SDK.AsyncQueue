namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Задача с параметрами
    /// </summary>
    /// <typeparam name="TParameters">Тип параметра</typeparam>
    public class AsyncTaskInfoParametrized<TParameters> : AsyncTaskInfo
    {
        /// <summary>
        /// Параметры
        /// </summary>
        public TParameters Parameters { get; set; }
    }
}
