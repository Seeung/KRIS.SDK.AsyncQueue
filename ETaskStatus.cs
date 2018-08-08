namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Статус выполнения задачи
    /// </summary>
    public enum ETaskStatus
    {
        /// <summary>
        /// Ожидает старта
        /// </summary>
        Wait = 0,

        /// <summary>
        /// Выполняется
        /// </summary>
        Working = 1,

        /// <summary>
        /// Успешно завершена
        /// </summary>
        Complete = 2,

        /// <summary>
        /// Ошибка завершения
        /// </summary>
        Failed = 3
    }
}
