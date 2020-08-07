using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel
{
    /// <summary>
    /// Representa el servicio de exportación de datos a un archivo de salida CSV.
    /// (is a Concrete Product in Factory Method Context).
    /// </summary>
    class CsvDataExporter : IDataExporter
    {
        private readonly string FIELD_SEPARATOR = ";";

        #region Miembros de IDataExporter
        public void Export(DataTable data, string fileName)
        {
            if (data != null && data.Rows.Count > 0)
            {
                StringBuilder sbData = new StringBuilder();
                var header = this.GetColumnsHeader(data.Columns);
                sbData.Append(header);
                var rowsInfo = this.GetRowsInfo(data);
                sbData.Append(rowsInfo);
                File.WriteAllText(fileName, sbData.ToString());
            }
        }

        public void Export(DataTable data, Stream outputStream)
        {
            if (data != null && data.Rows.Count > 0)
            {
                StreamWriter swOutput = new StreamWriter(outputStream);
                StringBuilder sbData = new StringBuilder();
                var header = this.GetColumnsHeader(data.Columns);
                sbData.Append(header);
                var rowsInfo = this.GetRowsInfo(data);
                sbData.Append(rowsInfo);
                swOutput.Write(sbData.ToString());
                //Restablecer indice del flujo de salida
                outputStream.Position = 0;
            }
        }

        public void Export(DataSet data, string fileName)
        {
            throw new NotImplementedException("Operación no ha sido implementada.");
        }

        public void Export(DataSet data, Stream outputStream)
        {
            throw new NotImplementedException("Operación no ha sido implementada.");
        }
        #endregion Miembros de IDataExporter

        private string GetColumnsHeader(DataColumnCollection columns)
        {
            StringBuilder sbData = new StringBuilder();
            foreach (DataColumn column in columns)
            {
                sbData.AppendFormat("{0}{1}", column.ColumnName, this.FIELD_SEPARATOR);
            }
            sbData.AppendLine();
            return sbData.ToString();
        }

        private string GetRowsInfo(DataTable data)
        {
            StringBuilder sbData = new StringBuilder();
            foreach (DataRow item in data.Rows)
            {
                StringBuilder sbRow = new StringBuilder();
                foreach (object column in item.ItemArray)
                {
                    sbRow.AppendFormat("{0}{1}", column.ToString().Trim(), this.FIELD_SEPARATOR);
                }
                sbRow.Remove(sbRow.ToString().Length - 2, 1);
                sbData.AppendLine(sbRow.ToString());
            }
            return sbData.ToString();
        }
    }
}
