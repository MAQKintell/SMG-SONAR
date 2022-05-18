using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using System.Collections;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Threading;
using Iberdrola.Commons.Reporting;
using System.Globalization;
using System.Web.Configuration;
using Iberdrola.Commons.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
using Iberdrola.Commons.DataAccess;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;


namespace Iberdrola.SMG.UI
{
    public partial class FrmModalGestionDocumental : FrmBase
    {
        
        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            
        }
        public override void SaveSessionData()
        {
           
        }
        public override void DeleteSessionData()
        {
           
        }
        #endregion

        public  MasterPageModalResponsive _Pagina;
        public  MasterPageModalResponsive Pagina
        {
            get
            {
                return _Pagina;
            }
            set
            {
                _Pagina = (MasterPageModalResponsive)this.Master; 
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Pagina = (MasterPageModalResponsive)this.Master;
            if (!IsPostBack)
            {
                Pagina.Titulo = Resources.TitulosVentanas.GestionDocumental;
                inicializarValores();
                rpn_main_pnpConsulta_txtReferencia_I.Text = Request.QueryString["COD_CONTRATO"];
                rpn_main_pnpConsulta_txtIdSolicitud_I.Text = Request.QueryString["ID_SOLICITUD"];
                int idSolicitud = int.Parse(Request.QueryString["ID_SOLICITUD"]);
                List<DocumentoDTO> lDocumentos = Documento.ObtenerDocumentosGDPorIdSolicitud(idSolicitud);
                if (lDocumentos.Count > 0)
                {
                    
                    grdListado.DataSource = lDocumentos;
                    grdListado.DataBind();
                }
                else
                {
                    Pagina.MostrarMensaje(Resources.TextosJavaScript.NO_EXISTEN_DOCUMENTOS);
                }
            }
        }

        private void inicializarValores()
        {
            rpn_main_pnpConsulta_txtReferencia_I.Text = "";
            rpn_main_pnpConsulta_txtIdSolicitud_I.Text = "";
            grdListado.DataSource = null;
            grdListado.DataBind();

        }
       
        protected void OnGrdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnGrdListado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "VerDocumento":
                        decimal idDocumento = decimal.Parse(e.CommandArgument.ToString());
                        DocumentoDTO documento = Documento.Obtener(idDocumento);
                        if (documento.FechaEnvioDelta != null)
                        {
                            ConsultarDocumentoEnDelta(documento.NombreDocumento);
                        }
                        else
                        {
                            CerrarPestaña("El documento no se encuentra en GESDOCOR.");                                                        
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        private void ConsultarDocumentoEnDelta(string nombreDocumento)
        {
            try
            {
                string ccbb = String.Empty;
                string[] documento = nombreDocumento.Split('.');
                string[] nomDoc = documento[0].Split('_');
                if (nomDoc.Length > 2)
                {
                    foreach (string nom in nomDoc)
                    {
                        ccbb += nom;
                    }
                }
                else
                {
                    ccbb = nomDoc[0];
                }

                string base64Result = string.Empty;
                //Obtenemos el fichero de ticket de combustion
                UtilidadesWebServices utilidadesWebServices = new UtilidadesWebServices();
                //ccbb = "49702301202010";
                base64Result = utilidadesWebServices.llamadaWSConsultaDocumento(ccbb);

                if (!String.IsNullOrEmpty(base64Result))
                {
                    XmlDocument docXML = new XmlDocument();
                    docXML.LoadXml(base64Result);

                    XmlNamespaceManager namespaces = new XmlNamespaceManager(docXML.NameTable);
                    namespaces.AddNamespace("dlwmin", "http://ws.gesdoc.fwapp.iberdrola.com/");

                    base64Result = docXML.SelectSingleNode("//dh", namespaces).InnerText;

                    if (!string.IsNullOrEmpty(base64Result))
                    {
                        AbrirBase64EnVuelo(base64Result, nombreDocumento);
                    }
                    else
                    {
                        CerrarPestaña("El documento no se ha podido recuperar de GESDOCOR.");
                    }
                }
                else
                {
                    CerrarPestaña("El documento no se ha podido recuperar de GESDOCOR.");
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        private void AbrirBase64EnVuelo(string base64Result, string nombreFichero)
        {
            //20211006 BGN BEG R#34004 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Modal Modificar Solicitud
            ConfiguracionDTO confRutaTemporal = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS_DESCARGA_DELTA);
            if (confRutaTemporal != null && !string.IsNullOrEmpty(confRutaTemporal.Valor))
            {
                string rutaDestino = confRutaTemporal.Valor;

                using (ManejadorFicheros.GetImpersonator())
                {
                    rutaDestino = Path.Combine(rutaDestino, nombreFichero);

                    byte[] todecode_byte = Convert.FromBase64String(base64Result);
                    File.WriteAllBytes(rutaDestino, todecode_byte);

                    if (File.Exists(rutaDestino))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("content-disposition", "attachment; filename=" + nombreFichero);
                        Response.TransmitFile(rutaDestino);
                        Response.Flush();
                    }
                    else
                    {
                        ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_DESCARGAR_FICHERO);// "No se ha podido encontrar el fichero. Póngase en contracto con sistemas."
                    }
                }

                ManejadorFicheros.BorrarFichero(rutaDestino);
            }
            //20211006 BGN END R#34004 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Modal Modificar Solicitud
        }

        private void CerrarPestaña(string mensaje)
        {
            Response.Write("<script type='text/javascript'> " +
                                    "window.opener = 'Self';" +
                                    "window.open('','_parent','');" +
                                    "window.close(); " +
                                    "alert('"+ mensaje + "'); " +
                                    "</script>");
        }

    }
}
