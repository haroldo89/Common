using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport.Contracts
{
    /// <summary>
    /// Expone operaciones que prestan servicios de lectura de datos desde medios de almacenamiento externos.
    /// (is Product in Factory Method Context).
    /// </summary>
    public interface IFileDataReader
    {
        /// <summary>
        /// Lee la información de un archivo de origen y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputFileName">Nombre del archivo de origen.</param>
        /// <returns>Contenido del archivo en forma de tabla.</returns>
        DataTable ReadTable(string inputFileName);

        /// <summary>
        /// Lee la información de un archivo de origen y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputStream">Flujo de datos de entrada.</param>
        /// <returns>Contenido del archivo en forma de tabla</returns>
        DataTable ReadTable(Stream inputStream);
    }
}
