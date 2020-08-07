using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport
{
    /// <summary>
    /// Fachada que expone servicios de lectura de datos desde archivos externos.
    /// </summary>
    public static class DataImportHelper
    {
        /// <summary>
        /// Permite leer la información de un archivo de origen y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputFileName">Nombre del archivo de origen.</param>
        /// <param name="inputFileFormat">Formato del archivo de entrada.</param>
        /// <returns>Contenido del archivo en forma de tabla.</returns>
        public static DataTable ReadTable(string inputFileName, InputFileFormat inputFileFormat)
        {
            DataTable dtFileInfo = null;
            var factory = DataReaderProvider.GetFactory(inputFileFormat);
            if (factory != null)
            {
                var reader = factory.CreateReader();
                if (reader != null)
                {
                    dtFileInfo = reader.ReadTable(inputFileName);
                }
            }
            return dtFileInfo;
        }

        /// <summary>
        /// Permite leer la información de un archivo de origen y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputStream">Flujo de datos de entrada.</param>
        /// <param name="inputFileFormat">Formato del archivo de entrada.</param>
        /// <returns>Contenido del archivo en forma de tabla.</returns>
        public static DataTable ReadTable(Stream inputStream, InputFileFormat inputFileFormat)
        {
            DataTable dtFileInfo = null;
            var factory = DataReaderProvider.GetFactory(inputFileFormat);
            if (factory != null)
            {
                var reader = factory.CreateReader();
                if (reader != null)
                {
                    dtFileInfo = reader.ReadTable(inputStream);
                }
            }
            return dtFileInfo;
        }
    }
}
