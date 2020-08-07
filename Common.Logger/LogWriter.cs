using System;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace AlbatrosSoft.Common.Logger
{
    public static class LogWriter
    {
        #region Constantes
        /// <summary>
        /// Categoria por defecto.
        /// </summary>
        private static string DefaultLogCategory
        {
            get
            {
                return "Application";
            }
        }

        /// <summary>
        /// Codigo de evento por defecto.
        /// </summary>
        private static int DefaultEventCode
        {
            get
            {
                return 0;
            }
        }
        #endregion Constantes

        #region Log de Eventos de Windows
        /// <summary>
        /// Permite escribir un mensaje de Log en el visor de eventos de Windows.
        /// </summary>
        /// <param name="message">Texto del mensaje a escribir.</param>
        /// <param name="source">Nombre del ensamblado o aplicación origen del mensaje.</param>
        /// <param name="category">Categoría del mensaje (Application, Security, System, etc.)</param>
        public static void WriteLog(string message, string source, string category)
        {
            WriteLog(message, source, category, EventLogEntryType.Information, LogWriter.DefaultEventCode);
        }

        /// <summary>
        /// Permite escribir un mensaje de Log en el visor de eventos de Windows.
        /// </summary>
        /// <param name="message">Texto del mensaje a escribir.</param>
        /// <param name="source">Nombre del ensamblado o aplicación origen del mensaje.</param>
        /// <param name="category">Categoría del mensaje (Application, Security, System, etc.)</param>
        /// <param name="logEntryType">Tipo del mensaje.</param>
        public static void WriteLog(string message, string source, string category, EventLogEntryType logEntryType)
        {
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, category);
            }
            EventLog.WriteEntry(source, message, logEntryType, LogWriter.DefaultEventCode);
        }

        /// <summary>
        /// Permite escribir un mensaje de Log en el visor de eventos de Windows.
        /// </summary>
        /// <param name="message">Texto del mensaje a escribir.</param>
        /// <param name="source">Nombre del ensamblado o aplicación origen del mensaje.</param>
        /// <param name="category">Categoría del mensaje (Application, Security, System, etc.)</param>
        /// <param name="logEntryType">Tipo del mensaje.</param>
        /// <param name="eventId">Código de evento asociado al mensaje (0 por defecto).</param>
        public static void WriteLog(string message, string source, string category, EventLogEntryType logEntryType, int eventId)
        {
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, category);
            }
            EventLog.WriteEntry(source, message, logEntryType, eventId);
        }

        /// <summary>
        /// Permite instrumentar una excepción a través de un mensaje de Log en el visor de eventos de Windows.
        /// </summary>
        /// <param name="exc">Instancia de la excepción a instrumentar.</param>
        /// <param name="source">Nombre del ensamblado o aplicación origen de la excepción.</param>
        public static void WriteLog(Exception exc, string source)
        {
            var sbErrorMessage = new StringBuilder();
            sbErrorMessage.AppendLine(string.Format("Error: {0}", exc.Message));
            sbErrorMessage.AppendLine(string.Format("Tipo: {0}", exc.GetType().FullName));
            sbErrorMessage.AppendLine(string.Format("Origen: {0}", exc.Source));
            sbErrorMessage.AppendLine(string.Format("StackTrace:{0}{1}", Environment.NewLine, exc.StackTrace));
            WriteLog(sbErrorMessage.ToString(), source ?? exc.Source, LogWriter.DefaultLogCategory, EventLogEntryType.Error, LogWriter.DefaultEventCode);
        }
        #endregion Log de Eventos de Windows

        #region Archivo de Texto Plano
        public static void WriteLog(string message, string logFilePath)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine(message);
            }
        }

        public static void WriteLogToFile(Exception exc, string logFilePath)
        {
            var sbErrorMessage = new StringBuilder();
            sbErrorMessage.AppendLine(string.Format("Error: {0}", exc.Message));
            sbErrorMessage.AppendLine(string.Format("Tipo: {0}", exc.GetType().FullName));
            sbErrorMessage.AppendLine(string.Format("Origen: {0}", exc.Source));
            sbErrorMessage.AppendLine(string.Format("StackTrace:{0}{1}", Environment.NewLine, exc.StackTrace));
            WriteLog(sbErrorMessage.ToString(), logFilePath);
        }
        #endregion Archivo de Texto Plano
    }
}
