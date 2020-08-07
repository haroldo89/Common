using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel.Factories
{
    /// <summary>
    /// Inicializa una nueva instancia del servicio de exportación de datos a archivos de formato XML
    /// (is a Concrete Creator in Factory Method Context).
    /// </summary>
    internal class XmlExporterFactory : DataExporterFactory
    {
        public override IDataExporter CreateExporter()
        {
            return new XmlDataExporter();
        }
    }
}
