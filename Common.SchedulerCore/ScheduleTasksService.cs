// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2016/07/28.
// Producto o sistema	    :   AlbatrosSoft.SchedulerCore
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representa una interfaz que expone un servicio que agrupa y ejecuta un conjunto de 
//                  tareas programadas.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using AlbatrosSoft.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Common.SchedulerCore
{
    /// <summary>
    /// Representa una interfaz que expone un servicio que agrupa y ejecuta un conjunto de tareas programadas.
    /// </summary>
    public abstract class ScheduleTasksService
    {
        [ThreadStatic]
        private List<ISchedulable> _Tasks;

        /// <summary>
        /// Conjunto de tareas programadas disponibles en el servicio.
        /// </summary>
        public List<ISchedulable> Tasks
        {
            get
            {
                if (this._Tasks == null)
                {
                    this._Tasks = new List<ISchedulable>();
                }
                return this._Tasks;
            }
        }

        /// <summary>
        /// Determina si el servicio se encuentra en ejecución.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                var schedulerMetadata = SchedulerHelper.GetSchedulerMetadata();
                //Trace.WriteLine(string.Format("Scheduler Instance Code: {0}", schedulerMetadata.Started));
                return schedulerMetadata.Started;
            }
        }

        /// <summary>
        /// Inicia la ejecución del servicio de tareas programadas.
        /// </summary>
        public virtual void Run()
        {
            this.LoadTasks();
            if (this.Tasks.Any())
            {
                this.StartTasks();
            }
        }

        /// <summary>
        /// Establece el grupo de tareas a ejecutar dentro del servicio de tareas programadas.
        /// </summary>
        protected abstract void LoadTasks();

        /// <summary>
        /// Inicia la ejecucion de las tareas programadas habilitadas en el servicio.
        /// </summary>
        protected virtual void StartTasks()
        {
            //Parallel.ForEach<ISchedulable>(this.Tasks, t => this.StartTask(t));
            this.Tasks.ForEach(t => this.StartTask(t));
        }

        /// <summary>
        /// Inicia la ejecución de una tarea programada.
        /// </summary>
        /// <param name="task">Instancia de la tarea a iniciar.</param>
        private void StartTask(ISchedulable task)
        {
            if (task != null)
            {
                try
                {
                    Trace.WriteLine(string.Format("Iniciando tarea {0} [{1}]. . .", task.TaskName, CommonUtils.GetCurrentDate()), "Startup");
                    task.Run();
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(string.Format("Error al intentar iniciar tarea programada {0}:\n{1}", task.TaskName, CommonUtils.GetErrorDetail(exc)));
                }
            }
        }

        /// <summary>
        /// Agrega una nueva tarea al servicio de tareas programadas.
        /// </summary>
        /// <param name="task">Instancia de la tarea a ejecutar.</param>
        public virtual void AddTask(ISchedulable task)
        {
            Func<ISchedulable, bool> taskFilter = t => t.TaskName.Equals(task.TaskName, StringComparison.InvariantCultureIgnoreCase);
            if (!this.Tasks.Any(taskFilter))
            {
                this.Tasks.Add(task);
            }
        }

        /// <summary>
        /// Remueve una tarea programada del grupo de tareas que componen el servicio.
        /// </summary>
        /// <param name="task">Instancia de la tarea a remover del servicio.</param>
        public virtual void RemoveTask(ISchedulable task)
        {
            Func<ISchedulable, bool> taskFilter = t => t.TaskName.Equals(task.TaskName, StringComparison.InvariantCultureIgnoreCase);
            if (this.Tasks.Any(taskFilter))
            {
                var taskToRemove = this.Tasks.FirstOrDefault(taskFilter);
                if (taskToRemove != null)
                {
                    this.Tasks.Remove(taskToRemove);
                }
            }
        }
    }
}
