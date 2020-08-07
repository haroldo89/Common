using AlbatrosSoft.Common.DataExport.ObjectModel.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel
{
    /// <summary>
    /// Proveedor de Servicio de Exportación de Datos.
    /// </summary>
    public static class DataExportProvider
    {
        private static Dictionary<OutputFormat, Func<DataExporterFactory>> _TypesMapping = new Dictionary<OutputFormat, Func<DataExporterFactory>>();

        static DataExportProvider()
        {
            LoadTypesMapping();
        }

        /// <summary>
        /// Establece la relación entre los formatos de salida y los proveedores de exportación de datos
        /// </summary>
        private static void LoadTypesMapping()
        {
            _TypesMapping.Add(OutputFormat.Excel, () => new ExcelExporterFactory());
            _TypesMapping.Add(OutputFormat.Csv, () => new CsvExporterFactory());
            _TypesMapping.Add(OutputFormat.Xml, () => new XmlExporterFactory());
            _TypesMapping.Add(OutputFormat.Pdf, () => new PdfExporterFactory());
        }

        /// <summary>
        /// Devuelve la instancia del proveedor encargado de inicializar el servicio de exportación de datos
        /// que corresponde al formato de salida indicado.
        /// </summary>
        /// <param name="outputFormatType">>Formato de salida del archivo de exportación.</param>
        /// <returns>Proveedor encargado de inicializar el servicio de exportación de datos para el formato de salida especificado.</returns>
        public static DataExporterFactory GetFactory(OutputFormat outputFormatType)
        {
            if (outputFormatType == null)
            {
                throw new ArgumentException("Tipo de formato de salida no puede ser un valor nulo", "outputFormatType");
            }
            Func<DataExporterFactory> factoryCreator;
            if (_TypesMapping.TryGetValue(outputFormatType, out factoryCreator))
            {
                return factoryCreator();
            }
            else
            {
                throw new NotImplementedException("Tipo de formato de salida no soportado");
            }
        }
    }
}
