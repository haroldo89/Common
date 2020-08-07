// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2016/10/11.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Clase GeneradorPDF, la cual concatena un numero de plantillas para generar el 
//                  Anexo FUN
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace Common.PdfGenerator.Extensions
{
    /// <summary>
    /// Clase GeneradorPDF, la cual concatena un numero de plantillas para generar el 
    /// anexo FUN
    /// </summary>
    public class GeneratorPdf
    {
        //FileStream DestinationDocStream { get; set; }
        //Document DocumentPdf { get; set; }
        //PdfCopy Copy { get; set; }
        //string Location { get; set; }
        //PdfReader Reader { get; set; }

        FileStream DestinationDocStream;
        Document DocumentPdf;
        PdfCopy Copy;
        string Location;
        PdfReader Reader;

        /// <summary>
        /// Generador PDF
        /// </summary>
        /// <param name="location"></param>
        /// <param name="newLocation"></param>
        public GeneratorPdf(string location, string newLocation)
        {
            DestinationDocStream = new FileStream(newLocation, FileMode.Create);
            DocumentPdf = new Document();
            Copy = new PdfCopy(DocumentPdf, DestinationDocStream);
            Copy.SetMergeFields();
            Copy.CompressionLevel = PdfStream.BEST_COMPRESSION;
            DocumentPdf.Open();
            this.Location = location;
        }

        public void ConcatenatePages(int numberCopies, int pageNumber)
        {
            using (Reader = new PdfReader(new RandomAccessFileOrArray(Location), null))
            {
                for (int i = 0; i < numberCopies; i++)
                {
                    using (PdfReader readcopy = new PdfReader(Reader))
                    {
                        using (PdfStamper stmp = new PdfStamper(readcopy, new FileStream(Location + "1.tmp", FileMode.OpenOrCreate)))
                        {
                            stmp.AcroFields.GenerateAppearances = true;
                            foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in Reader.AcroFields.Fields)
                            {
                                string nombreOr = de.Key.ToString();
                                string nombredef = string.Format("{0}_{1}", nombreOr, i.ToString());
                                stmp.AcroFields.RenameField(nombreOr, nombredef);
                            }
                            stmp.Close();
                        }
                        Copy.AddDocument(readcopy);
                        Copy.Flush();
                        readcopy.Close();
                        readcopy.Dispose();

                    }
                }
                Copy.Close();
            }
        }
    }
}
