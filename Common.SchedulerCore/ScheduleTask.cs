// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2016/07/28.
// Producto o sistema	    :   AlbatrosSoft.SchedulerCore
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representa una tarea programada que inicia un proceso que se ejecuta en segundo 
//                  plano con una periodicidad específica.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================
using Quartz;
using System;
using System.Globalization;

namespace Common.SchedulerCore
{
    /// <summary>
    /// Representa una tarea programada que inicia un proceso que se ejecuta en segundo plano con una periodicidad específica.
    /// </summary>
    /// <typeparam name="T">Tipo concreto de la tarea a ejecutar (debe implementar la interfaz <see cref="Quartz.IJob" />).</typeparam>
    public class ScheduleTask<T> : ISchedulable where T : IJob
    {
        /// <summary>
        /// Expresión Cron que establece la periodicidad de ejecución de la tarea.
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// Intervalo de tiempo de ejecución de la tarea (en segundos).
        /// </summary>
        public int SecondsInterval { get; set; }

        public int CustomerId { get; set; }

        /// <summary>
        /// Nombre de la tarea
        /// </summary>
        public string TaskName { get; set; }

        private bool IsScheduledByExpression
        {
            get
            {
                return !string.IsNullOrEmpty(this.CronExpression);
            }
        }

        private bool IsScheduledByTime
        {
            get
            {
                return this.SecondsInterval > 0;
            }
        }

        private bool HasCustomer
        {
            get
            {
                return this.CustomerId > 0;
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de una tarea que lanza un proceso que se ejecuta en segundo plano con una periodicidad específica.
        /// </summary>
        /// <param name="cronExpression">Expresión Cron que establece la periodicidad de ejecución de la tarea.</param>
        public ScheduleTask(string cronExpression)
        {
            if (string.IsNullOrEmpty(cronExpression))
            {
                throw new ArgumentException("CronExpression no puede ser un valor nulo. ", "cronExpression");
            }
            this.CronExpression = cronExpression;
            this.TaskName = typeof(T).Name;
        }

        /// <summary>
        ///  Inicializa una nueva instancia de una tarea que lanza un proceso que se ejecuta en segundo plano con una periodicidad específica.
        /// </summary>
        /// <param name="secondsInterval">ntervalo de tiempo de ejecución de la tarea (en segundos).</param>
        public ScheduleTask(int secondsInterval)
        {
            if (secondsInterval <= 0)
            {
                throw new ArgumentException("SecondsInterval debe ser un valor mayor que cero. ", "secondsInterval");
            }
            this.SecondsInterval = secondsInterval;
            this.TaskName = typeof(T).Name;
        }

        /// <summary>
        /// Inicializa una nueva instancia de una tarea que lanza un proceso que se ejecuta en segundo plano con una periodicidad específica.
        /// </summary>
        /// <param name="cronExpression">Expresion Cron</param>
        /// <param name="customerId">Id de Cliente</param>
        public ScheduleTask(string cronExpression, int customerId)
        {
            if (string.IsNullOrEmpty(cronExpression))
            {
                throw new ArgumentException("CronExpression no puede ser un valor nulo. ", "cronExpression");
            }
            if (customerId.Equals(0))
            {
                throw new ArgumentException("CustomerId debe ser diferente de 0. ", "customerId");
            }
            this.CronExpression = cronExpression;
            this.CustomerId = customerId;
            this.TaskName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", typeof(T).Name, this.CustomerId.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Inicia la ejecución de la tarea programada.
        /// </summary>
        public void Run()
        {
            if (this.IsScheduledByExpression)
            {
                if (this.HasCustomer)
                {
                    SchedulerHelper.RunTask<T>(this.TaskName, this.CronExpression, this.CustomerId);
                }
                else
                {
                    SchedulerHelper.RunTask<T>(this.TaskName, this.CronExpression);
                }
            }
            if (this.IsScheduledByTime)
            {
                SchedulerHelper.RunTask<T>(this.TaskName, this.SecondsInterval);
            }
        }

        /// <summary>
        /// Devuelve el nombre de la tarea programada.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.TaskName;
        }
    }
}
