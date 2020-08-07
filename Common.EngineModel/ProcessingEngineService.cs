// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2016/07/28.
// Producto o sistema	    :   Common.EngineModel

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representa una interfaz que expone un servicio que agrupa y ejecuta un conjunto de 
//                  tareas de procesamiento de datos.
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
using System.Threading.Tasks;

namespace Common.EngineModel
{
    /// <summary>
    /// Representa una interfaz que expone un servicio que agrupa y ejecuta un conjunto de tareas de procesamiento de datos.
    /// </summary>
    public abstract class ProcessingEngineService
    {
        [ThreadStatic]
        private List<EngineTask> _EngineTasks;

        /// <summary>
        /// Conjunto de tareas de procesamiento de datos disponibles en el servicio.
        /// </summary>
        public List<EngineTask> EngineTasks
        {
            get
            {
                if (this._EngineTasks == null)
                {
                    this._EngineTasks = new List<EngineTask>();
                }
                return this._EngineTasks;
            }
        }

        /// <summary>
        /// Inicia la ejecución del servicio de procesamiento de datos.
        /// </summary>
        public virtual void Run()
        {
            this.LoadEngineTasks();
            if (this.EngineTasks.Any())
            {
                this.StartEngineTasks();
            }
        }

        /// <summary>
        /// Establece el grupo de tareas a ejecutar dentro del servicio de procesamiento.
        /// </summary>
        protected abstract void LoadEngineTasks();

        /// <summary>
        /// Inicia la ejecucion de las tareas de procesamiento de datos habilitadas en el servicio.
        /// </summary>
        protected virtual void StartEngineTasks()
        {
            Parallel.ForEach<EngineTask>(this.EngineTasks, t => this.StartEngineTask(t));
        }

        /// <summary>
        /// Inicia la ejecución de una tarea de procesamiento de datos.
        /// </summary>
        /// <param name="task">Instancia de la tarea a iniciar.</param>
        private void StartEngineTask(EngineTask task)
        {
            if (task != null)
            {
                try
                {
                    Trace.WriteLine(string.Format("Iniciando motor {0} [{1}]. . .", task, CommonUtils.GetCurrentDate()), "Startup");
                    task.Start();
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(string.Format("Error al intentar iniciar motor de procesamiento:\n{0}", CommonUtils.GetErrorDetail(exc)));
                }
            }
        }

        /// <summary>
        /// Agrega una nueva tarea al servicio de procesamiento de datos.
        /// </summary>
        /// <param name="task">Instancia de la tarea a ejecutar.</param>
        protected void AddEngineTask(EngineTask task)
        {
            if (!this.EngineTasks.Any(s => s.GetType().Name.Equals(task.GetType().Name)))
            {
                this.EngineTasks.Add(task);
            }
        }
    }
}
