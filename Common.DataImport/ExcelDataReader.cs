using AlbatrosSoft.Common.DataImport.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;

namespace AlbatrosSoft.Common.DataImport
{
    /// <summary>
    /// Representa el servicio de lectura de datos de archivos de Excel.
    /// (is a Concrete Product in Factory Method Context).
    /// </summary>
    public class ExcelDataReader : IFileDataReader
    {
        #region Miembros de IFileDataReader
        /// <summary>
        /// Lee la información de un archivo de origen de Excel y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputFileName">Nombre del archivo de origen.</param>
        /// <returns>Contenido del archivo en forma de tabla.</returns>
        public DataTable ReadTable(string inputFileName)
        {
            DataTable dt = new DataTable();
            if (File.Exists(inputFileName))
            {
                try
                {
                    using (FileStream fileStream = File.Open(inputFileName, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataSet ds = excelReader.AsDataSet();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }
                }
                catch
                {
                    dt = null;
                }
            }
            return dt;
        }

        /// <summary>
        /// Lee la información de un archivo de origen de Excel y devuelve su contenido en forma de tabla.
        /// </summary>
        /// <param name="inputStream">Flujo de datos de entrada.</param>
        /// <returns>Contenido del archivo en forma de tabla</returns>
        public DataTable ReadTable(Stream inputStream)
        {
            DataTable dt = new DataTable();
            if (inputStream != null)
            {
                try
                {
                    using (inputStream)
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(inputStream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataSet ds = excelReader.AsDataSet();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }
                }
                catch
                {
                    dt = null;
                }
            }
            return dt;
        }
        #endregion Miembros de IFileDataReader
    }
}
