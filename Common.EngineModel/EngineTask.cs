using AlbatrosSoft.Common;
using System;
using System.Diagnostics;
using System.Threading;

namespace Common.EngineModel
{
    /// <summary>
    /// Establece la funcionalidad base de un motor de procesamiento de datos.
    /// </summary>
    public abstract class EngineTask
    {
        #region Propiedades
        private int _DelayTime;

        /// <summary>
        /// Tiempo de Retardo de la ejecución de una tarea de procesamiento.
        /// </summary>
        protected virtual int DelayTime
        {
            get
            {
                return this._DelayTime;
            }
            set
            {
                this._DelayTime = value;
            }
        }
        /// <summary>
        /// Indicador de modo de ejecución en Debug.
        /// </summary>
        protected virtual bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        #endregion Propiedades

        /// <summary>
        /// Inicia la ejecución de un motor de procesamiento de datos que ejecuta una tarea especifica.
        /// </summary>
        public virtual void Start()
        {
            while (true)
            {
                try
                {
                    this.Run();
                    if (this.DelayTime > 0)
                    {
                        Thread.Sleep(this.DelayTime);
                    }
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(string.Format("Error ejecutando tarea {0}:\n{1}", this, CommonUtils.GetErrorDetail(exc)), "UnknownError");
                }
            }
        }

        /// <summary>
        /// Escribe un mensaje en la consola de salida estándar del servicio para realizar traza y depuración.
        /// </summary>
        /// <param name="message">Mensaje de log.</param>
        public virtual void WriteDebugLog(string message)
        {
            if (this.IsDebug)
            {
                Trace.WriteLine(message, "Debug");
            }
        }

        /// <summary>
        /// Devuelve el nombre del tipo concreto que representa a la tarea.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Ejecuta el flujo principal de operaciones del motor de procesamiento.
        /// </summary>
        protected abstract void Run();
    }
}
