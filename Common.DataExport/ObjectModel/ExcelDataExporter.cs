using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.DataExport.ObjectModel
{
    /// <summary>
    /// Representa el servicio de exportación de datos a un archivo de salida de Excel
    /// (is a Concrete Product in Factory Method Context).
    /// </summary>
    class ExcelDataExporter : IDataExporter
    {
        #region Miembros de IDataExporter
        public void Export(DataTable data, string fileName)
        {
            if (data != null && data.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(data);
                this.Export(ds, fileName);
            }
        }

        public void Export(DataTable data, System.IO.Stream outputStream)
        {
            if (data != null && data.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(data);
                this.Export(ds, outputStream);
            }
        }

        public void Export(DataSet data, string fileName)
        {
            if (data != null && data.Tables.Count > 0)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    //Crear libro
                    WorkbookPart infoSegment = document.AddWorkbookPart();
                    infoSegment.Workbook = new Workbook();
                    infoSegment.Workbook.Sheets = new Sheets();
                    uint sheetId = 1;
                    foreach (DataTable dt in data.Tables)
                    {
                        //Crear segmento para agregar hojas de trabajo
                        WorksheetPart sheetSegment = infoSegment.AddNewPart<WorksheetPart>();
                        SheetData sheetData = new SheetData();
                        sheetSegment.Worksheet = new Worksheet(sheetData);
                        Sheets bookSheets = infoSegment.Workbook.GetFirstChild<Sheets>();
                        var relationshipId = infoSegment.GetIdOfPart(sheetSegment);
                        if (bookSheets.Elements<Sheet>().Any())
                        {
                            sheetId = bookSheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }
                        //Agregar hoja de trabajo al libro
                        Sheet sheet = new Sheet
                        {
                            Id = relationshipId,
                            SheetId = sheetId,
                            Name = dt.TableName
                        };
                        bookSheets.Append(sheet);
                        //Agregar encabezado de la tabla
                        Row headerRow = this.AddHeaderwRow(dt.Columns);
                        sheetData.AppendChild(headerRow);
                        //Agregar filas de la tabla
                        foreach (DataRow dr in dt.Rows)
                        {
                            //Agregar fila a la hoja
                            Row sheetRow = this.AddNewRow(dr, dt.Columns);
                            sheetData.AppendChild(sheetRow);
                        }
                    }
                }
            }
        }

        public void Export(DataSet data, System.IO.Stream outputStream)
        {
            if (data != null && data.Tables.Count > 0)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(outputStream, SpreadsheetDocumentType.Workbook))
                {
                    //Crear libro
                    WorkbookPart infoSegment = document.AddWorkbookPart();
                    infoSegment.Workbook = new Workbook();
                    infoSegment.Workbook.Sheets = new Sheets();
                    uint sheetId = 1;
                    foreach (DataTable dt in data.Tables)
                    {
                        //Crear segmento para agregar hojas de trabajo
                        WorksheetPart sheetSegment = infoSegment.AddNewPart<WorksheetPart>();
                        SheetData sheetData = new SheetData();
                        sheetSegment.Worksheet = new Worksheet(sheetData);
                        Sheets bookSheets = infoSegment.Workbook.GetFirstChild<Sheets>();
                        var relationshipId = infoSegment.GetIdOfPart(sheetSegment);
                        if (bookSheets.Elements<Sheet>().Any())
                        {
                            sheetId = bookSheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }
                        //Agregar hoja de trabajo al libro
                        Sheet sheet = new Sheet
                        {
                            Id = relationshipId,
                            SheetId = sheetId,
                            Name = dt.TableName
                        };
                        bookSheets.Append(sheet);
                        //Agregar encabezado de la tabla
                        Row headerRow = this.AddHeaderwRow(dt.Columns);
                        sheetData.AppendChild(headerRow);
                        //Agregar filas de la tabla
                        foreach (DataRow dr in dt.Rows)
                        {
                            //Agregar fila a la hoja
                            Row sheetRow = this.AddNewRow(dr, dt.Columns);
                            sheetData.AppendChild(sheetRow);
                        }
                    }
                }
                //Restablecer indice del flujo de salida
                outputStream.Position = 0;
            }
        }
        #endregion Miembros de IDataExporter

        /// <summary>
        /// Construye una fila de encabezado de columnas para agregar a una tabla.
        /// </summary>
        /// <param name="rowColumns">Listado de columnas de la tabla.</param>
        /// <returns>Fila de encabezado para agregar a la tabla.</returns>
        private Row AddHeaderwRow(DataColumnCollection rowColumns)
        {
            Row headerRow = new Row();
            foreach (DataColumn column in rowColumns)
            {
                Cell cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(column.ColumnName)
                };
                headerRow.AppendChild(cell);
            }
            return headerRow;
        }

        /// <summary>
        /// Construye una fila de contenido para agregar a una tabla.
        /// </summary>
        /// <param name="rowInfo">Listado de valores de la fila.</param>
        /// <param name="rowColumns">Listado de columnas de la tabla.</param>
        /// <returns>Fila de contenido para agregar a la tabla.</returns>
        private Row AddNewRow(DataRow rowInfo, DataColumnCollection rowColumns)
        {
            Row sheetRow = new Row();
            foreach (DataColumn column in rowColumns)
            {
                Cell cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(rowInfo[column].ToString())
                };
                sheetRow.AppendChild(cell);
            }
            return sheetRow;
        }
    }
}
