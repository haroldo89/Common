using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel.Factories
{
    /// <summary>
    // Inicializa una nueva instancia del servicio de exportación de datos a archivos de formato PDF
    /// (is a Concrete Creator in Factory Method Context).
    /// </summary>
    internal class PdfExporterFactory : DataExporterFactory
    {
        public override IDataExporter CreateExporter()
        {
            return new PdfDataExporter();
        }
    }
}
