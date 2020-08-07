using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport.Contracts
{
    /// <summary>
    /// Establece el comportamiento necesario para crear un servicio de lectura de datos  provenientes de un archivo externo.
    /// (is Creator in Factory Method Context).
    /// </summary>
    public abstract class DataReaderFactory
    {
        /// <summary>
        /// Devuelve una nueva instancia de un servicio de lectura de datos desde archivo externo.
        /// </summary>
        /// <returns></returns>
        public abstract IFileDataReader CreateReader();
    }
}
