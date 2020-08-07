using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport
{
    /// <summary>
    /// Establece los formato de salida de un archivo de exportación.
    /// </summary>
    public sealed class OutputFormat
    {
        private string Name { get; set; }

        private OutputFormat(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Libro de Microsoft Excel (.xlsx).
        /// </summary>
        public static OutputFormat Excel = OutputFormat.Parse("Excel");

        /// <summary>
        /// Archivo de valores separados por coma (.csv).
        /// </summary>
        public static OutputFormat Csv = OutputFormat.Parse("Csv");

        /// <summary>
        /// Archivo XML.
        /// </summary>
        public static OutputFormat Xml = OutputFormat.Parse("Xml");

        /// <summary>
        /// Página web HTML.
        /// </summary>
        public static OutputFormat Html = OutputFormat.Parse("Html");

        /// <summary>
        /// Archivo PDF.
        /// </summary>
        public static OutputFormat Pdf = OutputFormat.Parse("Pdf");

        public override bool Equals(object obj)
        {
            if (obj is OutputFormat)
            {
                return (obj as OutputFormat).Name.Equals(this.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal static OutputFormat Parse(string outputFormatType)
        {
            if (string.IsNullOrEmpty(outputFormatType))
            {
                throw new ArgumentException("Tipo de formato de salida no puede ser un valor nulo", "outputFormatType");
            }
            return new OutputFormat(outputFormatType);
        }
    }
}
