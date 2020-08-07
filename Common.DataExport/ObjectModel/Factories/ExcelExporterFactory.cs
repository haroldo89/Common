using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel.Factories
{
    /// <summary>
    /// Inicializa una nueva instancia del servicio de exportación de datos a Excel
    /// (is a Concrete Creator in Factory Method Context).
    /// </summary>
    internal class ExcelExporterFactory : DataExporterFactory
    {
        public override IDataExporter CreateExporter()
        {
            return new ExcelDataExporter();
        }
    }
}
