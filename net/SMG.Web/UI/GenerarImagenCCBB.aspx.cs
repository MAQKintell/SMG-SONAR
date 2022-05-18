using System;
using System.Web.UI;
using iTextSharp.text.pdf;
using Iberdrola.Commons.Logging;
using System.Drawing;
using System.IO;
using System.Web.UI;

namespace Iberdrola.SMG.UI
{
    /// <summary>
    /// Página para abrir un DataTable en un excel.
    /// </summary>
    public partial class GenerarImagenCCBB : System.Web.UI.Page
    {
        /// <summary>
        /// Generación de imagen para el BarCode
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string sProdCode = Request.QueryString["sProdCode"];
                if (sProdCode != null)
                {
                    //LogHelper.Debug("Generación del barcode, sProdCode = " + sProdCode);
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();

                    Response.ContentType = "image/jpeg";

                    GenerarBarCode128Image(sProdCode);

                    Response.Flush();
                    Response.End();
                }
                else
                {
                    string sImage = Request.QueryString["sImage"];
                    if (sImage != null)
                    {
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();

                        Response.ContentType = "image/png";

                        using (Bitmap image = new Bitmap(Server.MapPath("HTML/Images/" + sImage)))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                ms.WriteTo(Response.OutputStream);
                            }
                        }
                    }

                }
             }
        }

        /// <summary>
        /// GenerarBarCode128Image: Función que genera la imagen del bar code correspondiente al cups
        /// </summary>
        /// <param name="prodCode">CUPS a transformar en jpg</param>
        private void GenerarBarCode128Image(string sProdCode)
        {

            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.StartStopText = true;
            code128.Code = sProdCode;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            bm.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            
            bm.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
