using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataImport
{
    /// <summary>
    /// Establece los formatos de archivo soportados por los servicios de lectura de datos.
    /// </summary>
    public sealed class InputFileFormat
    {
        private static IEnumerable<InputFileFormat> _SupportedFileFormats;

        /// <summary>
        /// Nombre del formato de archivo.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Extensión del formato de archivo.
        /// </summary>
        public string FileExtension { get; private set; }

        /// <summary>
        /// Libro de Microsoft Excel (.xlsx).
        /// </summary>
        public static InputFileFormat Excel = InputFileFormat.Parse("Excel", ".xlsx");

        /// <summary>
        /// Archivo de valores separados por coma (.csv).
        /// </summary>
        public static InputFileFormat Csv = InputFileFormat.Parse("CSV", ".csv");

        static InputFileFormat()
        {
            _SupportedFileFormats = new List<InputFileFormat>()
            {
                InputFileFormat.Excel,
                InputFileFormat.Csv
            };
        }

        private InputFileFormat(string name, string fileExtension)
        {
            this.Name = name;
            this.FileExtension = fileExtension;
        }

        /// <summary>
        /// Devuelve los tipos de archivo soportados para la lectura de datos en forma de colección.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<InputFileFormat> AsEnumerable()
        {
            return _SupportedFileFormats;
        }

        /// <summary>
        /// Determina si el tipo de archivo dado es igual a la instancia actual.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is InputFileFormat)
            {
                return (obj as InputFileFormat).Name.Equals(this.Name);
            }
            return false;
        }

        /// <summary>
        /// Devuelve el valor hash de la instancia actual.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Devuelve la representación en forma de cadena de texto de un tipo de archivo soportado para lectura de datos.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.FileExtension);
        }

        /// <summary>
        /// Construye un tipo de archivo soportado para la lectura de datos.
        /// </summary>
        /// <param name="name">Nombre del tipo de archivo.</param>
        /// <param name="fileExtension">Extensión del tipo de archivo.</param>
        /// <returns></returns>
        internal static InputFileFormat Parse(string name, string fileExtension)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Nombre de formato de entrada no puede ser un valor nulo.", "name");
            }
            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentException("Extensión de archivo de entrada no puede ser un valor nulo.", "fileExtension");
            }
            return new InputFileFormat(name, fileExtension);
        }
    }
}
