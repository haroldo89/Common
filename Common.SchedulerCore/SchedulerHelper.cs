// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2016/07/28.
// Producto o sistema	    :   AlbatrosSoft.SchedulerCore
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Proporciona la funcionalidad necesaria para iniciar la ejecución de una tarea programada.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using AlbatrosSoft.Common;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;

namespace Common.SchedulerCore
{
    /// <summary>
    /// Proporciona la funcionalidad necesaria para iniciar la ejecución de una tarea programada.
    /// </summary>
    internal static class SchedulerHelper
    {
        private static ISchedulerFactory _SchedulerFactory;

        private static ISchedulerFactory SchedulerFactory
        {
            get
            {
                if (_SchedulerFactory == null)
                {
                    _SchedulerFactory = new StdSchedulerFactory();
                }
                return _SchedulerFactory;
            }
        }

        private static IScheduler _Scheduler;

        public static IScheduler Scheduler
        {
            get
            {
                if (_Scheduler == null)
                {
                    _Scheduler = SchedulerFactory.GetScheduler();
                }
                return _Scheduler;
            }
        }

        /// <summary>
        /// Inicia la ejecución en segundo plano de una tarea programada de acuerdo a la configuración establecida mediante
        /// una expresión Cron.
        /// </summary>
        /// <typeparam name="T">Tipo concreto de la tarea programada.</typeparam>
        /// <param name="taskName">Nombre de la tarea.</param>
        /// <param name="cronExpression">Expresión Cron que indica la programación de ejecución de la tarea.</param>
        public static void RunTask<T>(string taskName, string cronExpression) where T : IJob
        {
            if (!string.IsNullOrEmpty(taskName) && !string.IsNullOrEmpty(cronExpression))
            {
                if (!Scheduler.IsStarted)
                {
                    //Iniciar Scheduler
                    Scheduler.Start();
                    Trace.WriteLine(string.Format("Programador de tareas iniciado con éxito [{0}]. . .", CommonUtils.GetCurrentDate()));
                }
                bool existsJob = Scheduler.GetJobDetail(new JobKey(taskName)) != null;
                if (!existsJob)
                {
                    //Crear Tarea
                    JobBuilder jobBuilder = JobBuilder.Create<T>().WithIdentity(taskName);
                    if (jobBuilder != null)
                    {
                        IJobDetail jobDetail = jobBuilder.Build();
                        if (jobDetail != null)
                        {
                            //Crear Disparador de la Tarea
                            TriggerBuilder jobTriggerBuilder = TriggerBuilder.Create().WithIdentity(string.Format("{0}Trigger", taskName));
                            if (jobTriggerBuilder != null)
                            {
                                ITrigger jobTrigger = jobTriggerBuilder.StartNow()
                                                     .ForJob(jobDetail)
                                                     .WithCronSchedule(cronExpression)
                                                     .Build();
                                //Matricular Tarea en el Scheduler
                                Scheduler.ScheduleJob(jobDetail, jobTrigger);
                                Trace.WriteLine(string.Format("Tarea programada [{0}] ha sido iniciada con éxito (CronExpression: {1}) [{2}]. . .", taskName, cronExpression, CommonUtils.GetCurrentDate()));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución en segundo plano de una tarea programada de acuerdo a la configuración establecida mediante
        /// una expresión Cron.
        /// </summary>
        /// <typeparam name="T">Tipo concreto de la tarea programada.</typeparam>
        /// <param name="taskName">Nombre de la tarea.</param>
        /// <param name="cronExpression">Expresión Cron que indica la programación de ejecución de la tarea.</param>
        /// <param name="itemId">Id de Item</param>
        public static void RunTask<T>(string taskName, string cronExpression, int itemId) where T : IJob
        {
            if (!string.IsNullOrEmpty(taskName) && !string.IsNullOrEmpty(cronExpression))
            {
                if (!Scheduler.IsStarted)
                {
                    //Iniciar Scheduler
                    Scheduler.Start();
                    Trace.WriteLine(string.Format("Programador de tareas iniciado con éxito [{0}]. . .", CommonUtils.GetCurrentDate()));
                }
                bool existsJob = Scheduler.GetJobDetail(new JobKey(taskName)) != null;
                if (!existsJob)
                {
                    //Crear Tarea
                    JobBuilder jobBuilder = JobBuilder.Create<T>().WithIdentity(taskName);
                    if (jobBuilder != null)
                    {
                        IJobDetail jobDetail = jobBuilder
                            .UsingJobData("CustomerId", itemId)
                            .Build();
                        if (jobDetail != null)
                        {
                            //Crear Disparador de la Tarea
                            TriggerBuilder jobTriggerBuilder = TriggerBuilder.Create().WithIdentity(string.Format("{0}Trigger", taskName));
                            if (jobTriggerBuilder != null)
                            {
                                ITrigger jobTrigger = jobTriggerBuilder.StartNow()
                                                     .ForJob(jobDetail)
                                                     .WithCronSchedule(cronExpression)
                                                     .Build();
                                //Matricular Tarea en el Scheduler
                                Scheduler.ScheduleJob(jobDetail, jobTrigger);
                                Trace.WriteLine(string.Format("Tarea programada [{0}] ha sido iniciada con éxito (CronExpression: {1}) [{2}]. . .", taskName, cronExpression, CommonUtils.GetCurrentDate()));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución en segundo plano de una tarea programada con la periodicidad de tiempo indicada.
        /// </summary>
        /// <typeparam name="T">Tipo concreto de la tarea programada.</typeparam>
        /// <param name="taskName">Nombre de la tarea.</param>
        /// <param name="secondsInterval">Intervalo de tiempo de ejecución de la tarea (en segundos).</param>
        public static void RunTask<T>(string taskName, int secondsInterval) where T : IJob
        {
            if (!string.IsNullOrEmpty(taskName) && secondsInterval > 0)
            {
                if (!Scheduler.IsStarted)
                {
                    //Iniciar Scheduler
                    Scheduler.Start();
                    Trace.WriteLine(string.Format("Programador de tareas iniciado con éxito [{0}]. . .", CommonUtils.GetCurrentDate()));
                }
                bool existsJob = Scheduler.CheckExists(new JobKey(taskName));
                if (!existsJob)
                {
                    //Crear Tarea
                    JobBuilder jobBuilder = JobBuilder.Create<T>().WithIdentity(taskName);
                    if (jobBuilder != null)
                    {
                        IJobDetail jobDetail = jobBuilder.Build();
                        if (jobDetail != null)
                        {
                            //Crear Disparador de la Tarea
                            TriggerBuilder jobTriggerBuilder = TriggerBuilder.Create().WithIdentity(string.Format("{0}Trigger", taskName));
                            if (jobTriggerBuilder != null)
                            {
                                ITrigger jobTrigger = jobTriggerBuilder.StartNow()
                                                     //.ForJob(jobDetail)
                                                     .WithSimpleSchedule(s => s.WithIntervalInSeconds(secondsInterval).RepeatForever())
                                                     .Build();
                                //Matricular Tarea en el Scheduler
                                Scheduler.ScheduleJob(jobDetail, jobTrigger);
                                Trace.WriteLine(string.Format("Tarea programada [{0}] ha sido iniciada con éxito (SecondsInterval: {1}s) [{2}]. . .", taskName, secondsInterval, CommonUtils.GetCurrentDate()));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve un resumen de la configuración y el estado del servicio de tareas programadas activo.
        /// </summary>
        /// <returns></returns>
        public static SchedulerMetaData GetSchedulerMetadata()
        {
            return Scheduler.GetMetaData();
        }
    }
}
