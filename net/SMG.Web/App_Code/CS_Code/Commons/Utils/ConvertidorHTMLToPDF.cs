//using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Descripción breve de GenerarHTMLToPDF
/// </summary>

namespace Iberdrola.Commons.Utils
{
    public class ConvertidorHTMLToPDF
    {
        public ConvertidorHTMLToPDF()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        //public PdfDocument GenerarDocumentoPDF(StringBuilder sbHTMLTextoHTML)
        //{
        //    try
        //    { 
        //        // Configuración PDF de salida.
        //        HtmlToPdf converter = new HtmlToPdf();

        //        converter.Options.PdfPageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true); ;
        //        converter.Options.PdfPageOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);
        //        converter.Options.DisplayFooter = true;
        //        converter.Footer.DisplayOnFirstPage = true;
        //        converter.Footer.DisplayOnOddPages = true;
        //        converter.Footer.DisplayOnEvenPages = true;
        //        converter.Footer.Height = 50;

        //        PdfTextSection text = new PdfTextSection(0, 0, "Constantes.CARTA_CAI_PIE", new System.Drawing.Font("Courier", 4.5f));
        //        text.HorizontalAlign = PdfTextHorizontalAlign.Center;
        //        converter.Footer.Add(text);

        //        return converter.ConvertHtmlString(sbHTMLTextoHTML.ToString(), "");
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        throw;
        //    }
        //}
    }
}