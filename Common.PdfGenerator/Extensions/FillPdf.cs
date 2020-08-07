//===================================================================================================
// Desarrollado Por		    :   Harold Andres Caicedo Torres
// Fecha de Creación		:   2016/10/11.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Clase DiligenciadorPDF, usada para diligenciar un formulario pdf
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.PdfGenerator.Extensions
{
    /// <summary>
    /// Clase DiligenciadorPDF, usada para diligenciar un formulario pdf
    /// </summary>
    public class FillPdf
    {
        string _LocationPdf;
        /// <summary>
        /// Ubicacion del archivo pdf
        /// </summary>
        public string LocationPdf
        {
            get
            {
                return _LocationPdf;
            }
            set
            {
                _LocationPdf = value.Trim();
            }
        }

        /// <summary>
        /// Nueva locacion del archivo pdf resultado.
        /// </summary>
        public string NewLocationPdf { get; set; }
        PdfReader pdfReader;
        PdfStamper pdfStamper;
        AcroFields PDFFormFields;
        bool IsClosed;

        /// <summary>
        /// Crea una instancia de PDFFiller
        /// </summary>
        /// <param name="UbicacionPDF">Ubicación de la plantilla</param>
        /// <param name="NuevaUbicacionPDF">Nueva ubicación de la plantilla llenada</param>
        public FillPdf(string UbicacionPDF, string NuevaUbicacionPDF)
        {
            this.LocationPdf = UbicacionPDF;
            this.NewLocationPdf = NuevaUbicacionPDF;
            IsClosed = false;
            if (UbicacionPDF != null && UbicacionPDF != string.Empty)
            {
                try
                {
                    pdfReader = new PdfReader(this.LocationPdf);
                }
                catch (Exception) { }
            }
            else
            {
                throw new NullReferenceException("La ruta no debe ser una cadena vacía");
            }
                
        }

        /// <summary>
        /// Llena un campo del pdf
        /// </summary>
        /// <param name="FieldName">Nombre del campo</param>
        /// <param name="FieldValue">Valor del camo</param>    
        public void FillingField(string FieldName, string FieldValue)
        {
            if (pdfStamper == null || IsClosed)
            {
                throw new NullReferenceException("El objeto pdf es null o está cerrado");
            }
            else
            {
                PDFFormFields.SetField(FieldName.Trim(), FieldValue.Trim());
            }
        }

        /// <summary>
        /// Cierra el PDF Diligenciado
        /// </summary>
        public void ClosePdf()
        {
            pdfReader.RemoveUnusedObjects();
            pdfReader.RemoveFields();
            pdfStamper.Close();
            pdfStamper.Dispose();
            pdfReader.Close();
            pdfReader.Dispose();
            IsClosed = true;
        }

        /// <summary>
        /// Abre el pdf para ser Llenado
        /// </summary>
        public void OpenPdftoFill()
        {
            if (pdfStamper == null || IsClosed)
            {
                try
                {
                    pdfStamper = new PdfStamper(pdfReader, new FileStream(this.NewLocationPdf, FileMode.Create));
                    pdfStamper.Writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    pdfStamper.Writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.SetFullCompression();
                    PDFFormFields = pdfStamper.AcroFields;
                    PDFFormFields.GenerateAppearances = true;
                    IsClosed = false;
                }
                catch (Exception) { }
            }
        }

        public string ListNameFields()
        {
            string ret = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)
                {
                    sb.AppendLine(de.Key.ToString() + ",");
                }
                ret = sb.ToString();
            }
            catch (Exception)
            {
                ret = "No se ha encontrado archivo";
            }
            return ret;
        }

        public List<string> ListNameofFieldsToList()
        {
            return new List<string>(ListNameFields().Split(','));
        }

        public string ReturnFieldValue(string nombreCampo)
        {
            string field = string.Empty;
            if (pdfReader.AcroFields.Fields.Count > 0)
            {
                field = PDFFormFields.GetField(nombreCampo);

            }
            return field;
        }

        public void RenameField(string fieldName, string newFieldValue)
        {
            PDFFormFields.RenameField(fieldName, newFieldValue);
        }
    }
}
