using AlbatrosSoft.Common.DataImport.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport.Factories
{
    /// <summary>
    /// Inicializa una nueva instancia del servicio de lectura de datos de archivos de Excel.
    /// (is a Concrete Creator in Factory Method Context).
    /// </summary>
    internal class ExcelReaderFactory : DataReaderFactory
    {
        /// <summary>
        /// Crea una nueva instancia de la clase <see cref="ExcelDataReader" /> que representa el servicio de lectura de datos de archivos de Excel.
        /// </summary>
        /// <returns></returns>
        public override IFileDataReader CreateReader()
        {
            return new ExcelDataReader();
        }
    }
}
