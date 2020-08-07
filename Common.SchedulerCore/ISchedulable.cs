
namespace Common.SchedulerCore
{
    /// <summary>
    /// Establece la funcionalidad base de un proceso que se ejecuta en segundo plano con
    /// una periodicidad específica.
    /// </summary>
    public interface ISchedulable
    {
        /// <summary>
        /// Expresion Cron.
        /// </summary>
        string CronExpression { get; set; }
        /// <summary>
        /// Nombre de la tarea.
        /// </summary>
        string TaskName { get; set; }
        /// <summary>
        /// Inicializar tarea.
        /// </summary>
        void Run();
    }
}
