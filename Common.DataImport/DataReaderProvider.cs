using AlbatrosSoft.Common.DataImport.Contracts;
using AlbatrosSoft.Common.DataImport.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport
{
    /// <summary>
    /// Proveedor de Servicios de Lectura de Datos provenientes de Archivos Externos.
    /// </summary>
    public static class DataReaderProvider
    {
        /// <summary>
        /// Agrupa los tipos de archivo de entrada soportados para la lectura de datos.
        /// </summary>
        private static IDictionary<InputFileFormat, Func<DataReaderFactory>> _ReaderFactories;

        static DataReaderProvider()
        {
            LoadFactories();
        }

        /// <summary>
        /// Establece los tipos de archivo de entrada soportados para la lectura de datos.
        /// </summary>
        private static void LoadFactories()
        {
            _ReaderFactories = new Dictionary<InputFileFormat, Func<DataReaderFactory>>()
            {
                {InputFileFormat.Excel, () => new ExcelReaderFactory()},
            };
        }

        /// <summary>
        /// Devuelve la instancia del proveedor encargado de inicializar el servicio de lectura de datos
        /// que corresponde al formato de entrada indicado.
        /// </summary>
        /// <param name="inputFileFormat">Formato de entrada del archivo de origen.</param>
        /// <returns>Proveedor encargado de inicializar el servicio de lectura de datos para el formato de entrada especificado.</returns>
        public static DataReaderFactory GetFactory(InputFileFormat inputFileFormat)
        {
            DataReaderFactory factory = null;
            if (inputFileFormat == null)
            {
                throw new ArgumentException("Tipo de formato de archivo entrada no puede ser un valor nulo.", "inputFileFormat");
            }
            Func<DataReaderFactory> factoryCreator;
            if (_ReaderFactories.TryGetValue(inputFileFormat, out factoryCreator))
            {
                factory = factoryCreator();
            }
            else
            {
                throw new NotImplementedException("Tipo de formato de entrada no soportado.");
            }
            return factory;
        }
    }
}
