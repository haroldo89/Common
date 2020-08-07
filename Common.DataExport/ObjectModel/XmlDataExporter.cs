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
    /// Representa el servicio de exportación de datos a un archivo de salida XML.
    /// (is a Concrete Product in Factory Method Context).
    /// </summary>
    class XmlDataExporter : IDataExporter
    {
        #region Miembros de IDataExporter
        public void Export(DataTable data, string fileName)
        {
            if (data != null && data.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(data.TableName))
                {
                    data.TableName = "DataRow";
                }
                data.WriteXml(fileName);
            }
        }

        public void Export(DataTable data, Stream outputStream)
        {
            if (data != null && data.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(data.TableName))
                {
                    data.TableName = "DataRow";
                }
                data.WriteXml(outputStream);
                //Restablecer indice del flujo de salida
                outputStream.Position = 0;
            }
        }

        public void Export(DataSet data, string fileName)
        {
            if (data != null && data.Tables.Count > 0)
            {
                if (data.Tables.Cast<DataTable>().Any(t => string.IsNullOrEmpty(t.TableName)))
                {
                    data.Tables.Cast<DataTable>().Where(t => string.IsNullOrEmpty(t.TableName)).ToList().ForEach(t => t.TableName = "DataRow");
                }
                data.WriteXml(fileName);
            }
        }

        public void Export(DataSet data, Stream outputStream)
        {
            if (data != null && data.Tables.Count > 0)
            {
                if (data.Tables.Cast<DataTable>().Any(t => string.IsNullOrEmpty(t.TableName)))
                {
                    data.Tables.Cast<DataTable>().Where(t => string.IsNullOrEmpty(t.TableName)).ToList().ForEach(t => t.TableName = "DataRow");
                }
                data.WriteXml(outputStream);
                //Restablecer indice del flujo de salida
                outputStream.Position = 0;
            }
        }
        #endregion Miembros de IDataExporter
    }
}
