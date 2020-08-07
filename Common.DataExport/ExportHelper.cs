using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlbatrosSoft.Common.DataExport.ObjectModel;

namespace AlbatrosSoft.Common.DataExport
{
    /// <summary>
    /// Fachada que expone servicios de exportación de datos hacia medios de almacenamiento externos.
    /// </summary>
    public static class ExportHelper
    {
        /// <summary>
        /// Permite exportar la información contenida en una estructura tipo tabla a un archivo externo.
        /// </summary>
        /// <param name="data">Set de datos a exportar.</param>
        /// <param name="outputFileName">Nombre del archivo de salida.</param>
        /// <param name="formatType">Formato del archivo de exportación.</param>
        public static void Export(DataTable data, string outputFileName, OutputFormat formatType)
        {
            var factory = DataExportProvider.GetFactory(formatType);
            if (factory != null)
            {
                var exporter = factory.CreateExporter();
                if (exporter != null)
                {
                    exporter.Export(data, outputFileName);
                }
            }
        }

        /// <summary>
        /// Permite exportar la información contenida en un DataSet a un archivo externo.
        /// </summary>
        /// <param name="data">Set de datos a exportar.</param>
        /// <param name="outputFileName">Nombre del archivo de salida.</param>
        /// <param name="formatType">Formato del archivo de exportación.</param>
        public static void Export(DataSet data, string outputFileName, OutputFormat formatType)
        {
            var factory = DataExportProvider.GetFactory(formatType);
            if (factory != null)
            {
                var exporter = factory.CreateExporter();
                if (exporter != null)
                {
                    exporter.Export(data, outputFileName);
                }
            }
        }

        /// <summary>
        /// Permite exportar la información contenida en una estructura tipo tabla a un flujo de datos de salida (Stream).
        /// </summary>
        /// <param name="data">Set de datos a exportar.</param>
        /// <param name="outputStream">Flujo de datos de salida.</param>
        /// <param name="formatType">Formato del archivo de exportación.</param>
        public static void Export(DataTable data, Stream outputStream, OutputFormat formatType)
        {
            var factory = DataExportProvider.GetFactory(formatType);
            if (factory != null)
            {
                var exporter = factory.CreateExporter();
                if (exporter != null)
                {
                    exporter.Export(data, outputStream);
                }
            }
        }

        /// <summary>
        /// Permite exportar la información contenida en un DataSet a un flujo de datos de salida (Stream).
        /// </summary>
        /// <param name="data">Set de datos a exportar.</param>
        /// <param name="outputStream">Flujo de datos de salida.</param>
        /// <param name="formatType">Formato del archivo de exportación.</param>
        public static void Export(DataSet data, Stream outputStream, OutputFormat formatType)
        {
            var factory = DataExportProvider.GetFactory(formatType);
            if (factory != null)
            {
                var exporter = factory.CreateExporter();
                if (exporter != null)
                {
                    exporter.Export(data, outputStream);
                }
            }
        }
    }
}
