using System;

namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Делегаты
    /// </summary>
    public class Delegates
    {
        /// <summary>
        /// Делегат задачи
        /// </summary>
        /// <param name="Task">Задача</param>
        public delegate void Del_Task(AsyncTaskInfo Task);

        /// <summary>
        /// Делегат задачи с исключением
        /// </summary>
        /// <param name="Task">Задача</param>
        /// <param name="ex">Исключение</param>
        public delegate void Del_Failed(AsyncTaskInfo Task, Exception ex);
    }
}
