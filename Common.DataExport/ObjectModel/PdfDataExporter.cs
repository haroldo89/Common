using iTextSharp.text;
using iTextSharp.text.pdf;
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
    /// Representa el servicio de exportación de datos a un archivo de salida PDF.
    /// (is a Concrete Product in Factory Method Context).
    /// </summary>
    class PdfDataExporter : IDataExporter
    {
        /// <summary>
        /// Tipo de fuente por defecto para el encabezado de la tabla.
        /// </summary>
        private readonly Font HEADER_ROW_FONT = FontFactory.GetFont("ARIAL", 5, Font.BOLD, BaseColor.BLACK);

        /// <summary>
        /// Tipo de fuente por defecto para el contenido de la tabla.
        /// </summary>
        private readonly Font CONTENT_ROW_FONT = FontFactory.GetFont("ARIAL", 5);

        #region Miembros de IDataExporter
        public void Export(DataTable data, string fileName)
        {
            if (data != null && data.Rows.Count > 0)
            {
                var outputStream = new FileStream(fileName, FileMode.Create);
                using (Document document = new Document())
                {
                    //Establecer tamaño de página del documento
                    document.SetPageSize(PageSize.LETTER.Rotate());
                    PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                    document.Open();
                    //TODO: DataExport - Establecer tamaño de las celdas de acuerdo a la longitud del texto de contenido.
                    PdfPTable table = new PdfPTable(data.Columns.Count);
                    //Agregar encabezado de columnas
                    foreach (DataColumn column in data.Columns)
                    {
                        table.AddCell(new Phrase(column.ColumnName, this.HEADER_ROW_FONT));
                    }
                    //Agregar filas de la tabla
                    foreach (DataRow row in data.Rows)
                    {
                        foreach (DataColumn column in data.Columns)
                        {
                            var fieldInfo = row[column].ToString();
                            var cell = new PdfPCell(new Phrase(new Chunk(fieldInfo, this.CONTENT_ROW_FONT)));
                            table.AddCell(cell);
                        }
                    }
                    //Agregar tabla al documento
                    document.Add(table);
                }
            }
        }

        public void Export(DataTable data, Stream outputStream)
        {
            if (data != null && data.Rows.Count > 0)
            {
                Document document = new Document();
                //Establecer tamaño de página del documento
                document.SetPageSize(PageSize.LETTER.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                document.Open();
                //TODO: DataExport - Establecer tamaño de las celdas de acuerdo a la longitud del texto de contenido.
                PdfPTable table = new PdfPTable(data.Columns.Count);
                //Agregar encabezado de columnas
                foreach (DataColumn column in data.Columns)
                {
                    table.AddCell(new Phrase(column.ColumnName, this.HEADER_ROW_FONT));
                }
                //Agregar filas de la tabla
                foreach (DataRow row in data.Rows)
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        var fieldInfo = row[column].ToString();
                        var cell = new PdfPCell(new Phrase(new Chunk(fieldInfo, this.CONTENT_ROW_FONT)));
                        table.AddCell(cell);
                    }
                }
                //Agregar tabla al documento
                document.Add(table);
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

        /// <summary>
        /// Agrega un salto de línea al final del contenido de un documento.
        /// </summary>
        /// <param name="document">Representación del contenido del documento.</param>
        private void AddBreakLine(Document document)
        {
            if (document != null)
            {
                document.Add(new Paragraph(Environment.NewLine));
            }
        }
    }
}
