using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel
{
    /// <summary>
    /// Expone operaciones que prestan servicios de exportación de datos hacia medios de almacenamiento externos.
    /// (is Product in Factory Method Context).
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Exporta la información contenida en un DataTable a un archivo externo.
        /// </summary>
        /// <param name="data">Instancia de DataTable a exportar.</param>
        /// <param name="fileName">Nombre del archivo de salida.</param>
        void Export(DataTable data, string fileName);

        /// <summary>
        /// Exporta la información contenida en un DataTable a un flujo de datos de salida (Stream).
        /// </summary>
        /// <param name="data">Instancia de DataTable a exportar.</param>
        /// <param name="outputStream">Flujo de datos de salida.</param>
        void Export(DataTable data, Stream outputStream);

        /// <summary>
        /// Exporta la información contenida en un DataSet a un archivo externo.
        /// </summary>
        /// <param name="data">Instancia de DataSet a exportar.</param>
        /// <param name="fileName">Nombre del archivo de salida.</param>
        void Export(DataSet data, string fileName);

        /// <summary>
        /// Exporta la información contenida en un DataSet a un flujo de datos de salida (Stream).
        /// </summary>
        /// <param name="data">Instancia de DataSet a exportar.</param>
        /// <param name="outputStream">Flujo de datos de salida.</param>
        void Export(DataSet data, Stream outputStream);
    }
}
