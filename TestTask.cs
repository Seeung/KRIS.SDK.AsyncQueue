using System.Threading;

namespace KRIS.SDK.AsyncQueue
{
    /// <summary>
    /// Тестовая задача
    /// </summary>
    public class TestTask : AsyncTaskInfo
    {
        /// <summary>
        /// Наименование задачи
        /// </summary>
        public override string Name => "Тестовая задача";

        /// <summary>
        /// Пользовательский метод задачи
        /// </summary>
        /// <returns></returns>
        public override object ThStart()
        {
            SetStatus("Тестирование");
            for(int i = 1; i <= 100; i++)
            {
                Thread.Sleep(1000);
                SetProgress(i);
            }

            return null;
        }
    }
}
