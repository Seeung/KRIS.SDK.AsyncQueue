using System;
using System.Threading.Tasks;
using System.Windows.Threading;

using KRIS.SDK.AsyncQueue.Exceptions;

namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Асинхронная задача
    /// </summary>
    public class AsyncTaskInfo
    {
        /// <summary>
        /// Конструктор асинхронной задачи
        /// </summary>
        public AsyncTaskInfo()
        {
            CurrentDispatcher = Dispatcher.CurrentDispatcher;
            Initialize();
        }

        /// <summary>
        /// Конструктор асинхронной задачи
        /// </summary>
        /// <param name="collection">Коллекция задач для привязки</param>
        public AsyncTaskInfo(ITaskCollection collection)
        {
            BingingCollection(collection);
            Initialize();
        }

        /// <summary>
        /// Текущий поток
        /// </summary>
        public Dispatcher CurrentDispatcher { get; private set; }

        /// <summary>
        /// ID задачи
        /// </summary>
        public uint ID = 0;

        /// <summary>
        /// Название задачи
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        private string _status = String.Empty;

        /// <summary>
        /// Статус задачи
        /// </summary>
        public string Status { get => _status; set => SetStatus(value); }

        /// <summary>
        /// Прогресс задачи
        /// </summary>
        private int _Progress = 0;

        /// <summary>
        /// Прогресс задачи
        /// </summary>
        public virtual int Progress { get => _Progress; private set => _Progress = value; }

        /// <summary>
        /// Удалить после успешного завершения
        /// </summary>
        public virtual bool IsDeleteAfterSuccessfulCompletion => false;

        /// <summary>
        /// Действие при успешном завершении задачи
        /// </summary>
        private Action<AsyncTaskInfo> ActionComplete;

        /// <summary>
        /// Действие при ошибке завершения задачи
        /// </summary>
        private Action<AsyncTaskInfo> ActionFailed;

        /// <summary>
        /// Тип размещения задачи
        /// </summary>
        public ETaskType TaskType { get; protected set; }

        /// <summary>
        /// Статус выполнения задачи
        /// </summary>
        public ETaskStatus TaskStatus { get; protected set; }

        /// <summary>
        /// Поток задачи
        /// </summary>
        private Task<bool> TaskThread;

        /// <summary>
        /// Результат выполнения задачи
        /// </summary>
        public object Result { get; protected set; }

        /// <summary>
        /// Исключение при выполнении здачи
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// Коллекция задач
        /// </summary>
        public ITaskCollection TaskCollection { get; protected set; }

        /// <summary>
        /// Стостяние связи с коллекцией задач
        /// </summary>
        public bool IsTaskCollectionBinging => TaskCollection != null;

        /// <summary>
        /// Инициализация
        /// </summary>
        public virtual void Initialize()
        {
            TaskThread = new Task<bool>(ThBody);
            TaskStatus = ETaskStatus.Wait;
            CustomInitialize();
        }

        /// <summary>
        /// Пользовательская инициализация
        /// </summary>
        public virtual void CustomInitialize() { }

        /// <summary>
        /// Запустить задачу
        /// </summary>
        public void Start()
        {
            TaskStatus = ETaskStatus.Working;
            OnStart?.Invoke(this);
            
            TaskThread.Start();
            TaskThread.ContinueWith(
                    new Action<Task<bool>>((t) => InvokeShutdownTask())
                );
        }

        /// <summary>
        /// Основной метод задачи
        /// </summary>
        public bool ThBody()
        {
            try
            {
                Result = ThStart();
                return true;
            }
            catch(Exception ex)
            {
                this.Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Пользовательский метод задачи
        /// </summary>
        /// <returns></returns>
        public virtual object ThStart() { return null; }

        /// <summary>
        /// Вызвать завершение задачи
        /// </summary>
        private void InvokeShutdownTask() => CurrentDispatcher.Invoke(new Action(() => ShutdownTask()));

        /// <summary>
        /// Метод успешного завершения задачи
        /// </summary>
        private void ShutdownTask()
        {
            if(Exception == null)
            {
                TaskStatus = ETaskStatus.Complete;
                OnComplete?.Invoke(this);
                ActionComplete?.Invoke(this);
            }
            else
            {
                TaskStatus = ETaskStatus.Failed;
                OnFailed?.Invoke(this, Exception);
                ActionFailed?.Invoke(this);
            }
        }

        /// <summary>
        /// Привязать коллекцию задач
        /// </summary>
        /// <param name="collection">Коллекция задач для привязки</param>
        public void BingingCollection(ITaskCollection Collection) => TaskCollection = Collection;

        /// <summary>
        /// Отправить задачу в очередь
        /// </summary>
        public void SendToQueue()
        {
            if (!IsTaskCollectionBinging)
                throw new TaskCollectionNotLinkedException();
            TaskCollection.AddTask(this);
        }

        /// <summary>
        /// Обновить прогресс задачи
        /// </summary>
        /// <param name="progress">Прогресс</param>
        protected void SetProgress(int progress)
        {
            this.Progress = progress;
            CurrentDispatcher.Invoke(new Action(() => OnPogress?.Invoke(this)));
        }

        /// <summary>
        /// Обновить статус задачи
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(string status)
        {
            this._status = status;
            OnPogress?.Invoke(this);
        }

        /// <summary>
        /// Получить результат работы
        /// </summary>
        /// <typeparam name="T">Тип результата</typeparam>
        /// <returns></returns>
        public T GetResult<T>() => (T)Result;

        /// <summary>
        /// Установить действие при успешном завершении задачи
        /// </summary>
        /// <param name="ActionComplete">Действие при успешном завершении задачи</param>
        public void SetActionComplete(Action<AsyncTaskInfo> ActionComplete) => this.ActionComplete = ActionComplete;

        /// <summary>
        /// Установить действие при ошибке завершения задачи
        /// </summary>
        /// <param name="ActionFailed">Действие при ошибке завершения задачи</param>
        public void SetActionFailed(Action<AsyncTaskInfo> ActionFailed) => this.ActionFailed = ActionFailed;

        /// <summary>
        /// Установить действия
        /// </summary>
        /// <param name="ActionComplete">Действие при успешном завершении задачи</param>
        /// <param name="ActionFailed">Действие при ошибке завершения задачи</param>
        public void SetAction(Action<AsyncTaskInfo> ActionComplete, Action<AsyncTaskInfo> ActionFailed)
        {
            this.ActionComplete = ActionComplete;
            this.ActionFailed = ActionFailed;
        }

        /// <summary>
        /// Примести задачу к данному типу
        /// </summary>
        /// <typeparam name="TCast">Тип задачи</typeparam>
        /// <returns></returns>
        public TCast Cast<TCast>() where TCast : AsyncTaskInfo, new() => (TCast)this;

        /// <summary>
        /// События завершения задачи с ошибкой
        /// </summary>
        public event Delegates.Del_Failed OnFailed;

        /// <summary>
        /// Событие запуска задачи
        /// </summary>
        public event Delegates.Del_Task OnStart;

        /// <summary>
        /// Событие успешного завершения задачи
        /// </summary>
        public event Delegates.Del_Task OnComplete;

        /// <summary>
        /// События изменения прогресса задачи
        /// </summary>
        public event Delegates.Del_Task OnPogress;
    }
}
