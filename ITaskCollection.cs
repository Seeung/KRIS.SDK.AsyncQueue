using System.Collections;

namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Интерфейс коллекции задач
    /// </summary>
    public interface ITaskCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Коллекция задач
        /// </summary>
        AsyncTaskInfo[] Tasks { get; }

        /// <summary>
        /// Флаг удаления задачи при успешном завершении
        /// </summary>
        bool IsEnableDeleteAfterSuccessfulCompletion { get; set; }

        /// <summary>
        /// Создать задачу и привязать к текущей коллекции
        /// </summary>
        /// <typeparam name="T">Тип задачи</typeparam>
        /// <returns></returns>
        T CreateTask<T>() where T : AsyncTaskInfo, new();

        /// <summary>
        /// Получить задачу по ID
        /// </summary>
        /// <param name="ID">ID задачи</param>
        /// <returns></returns>
        AsyncTaskInfo GetTask(uint ID);

        /// <summary>
        /// Получить задачу по индексу
        /// </summary>
        /// <param name="Index">Индекс</param>
        /// <returns></returns>
        AsyncTaskInfo GetTask(int Index);

        /// <summary>
        /// Добавить задачу
        /// </summary>
        /// <param name="Task">Задача</param>
        void AddTask(AsyncTaskInfo Task);

        /// <summary>
        /// Событие добавления задачи
        /// </summary>
        event Delegates.Del_Task OnAdd;

        /// <summary>
        /// Событие запуска задачи
        /// </summary>
        event Delegates.Del_Task OnStart;

        /// <summary>
        /// Событие ошибки задачи
        /// </summary>
        event Delegates.Del_Failed OnFailed;

        /// <summary>
        /// Событие успешного завершения
        /// </summary>
        event Delegates.Del_Task OnComplete;

        /// <summary>
        /// Событие измененения прогресса
        /// </summary>
        event Delegates.Del_Task OnPogress;
    }
}
