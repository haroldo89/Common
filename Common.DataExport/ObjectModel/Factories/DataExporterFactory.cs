using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel.Factories
{
    /// <summary>
    /// Establece el comportamiento necesario para crear un nuevo servicio de exportación de datos
    /// (is Creator in Factory Method Context).
    /// </summary>
    public abstract class DataExporterFactory
    {
        /// <summary>
        /// Devuelve una nueva instancia de un servicio de exportación de datos.
        /// </summary>
        /// <returns></returns>
        public abstract IDataExporter CreateExporter();
    }
}
