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
    public partial class FrmGestionDocumental : FrmBaseListado
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

        public  MasterPageResponsive _Pagina;
        public  MasterPageResponsive Pagina
        {
            get
            {
                return _Pagina;
            }
            set
            {
                _Pagina = (MasterPageResponsive)this.Master; 
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Pagina = (MasterPageResponsive)this.Master;
            if (!IsPostBack)
            {
                //Pagina = (MasterPageResponsive)this.Master;
                Pagina.Titulo = Resources.TitulosVentanas.GestionDocumental;
                inicializarValores();
            }
        }

        private void inicializarValores()
        {
            rpn_main_pnpConsulta_txtReferencia_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I.Text = "";
            rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I.Text = "";
            grdListado.DataSource = null;
            grdListado.DataBind();

        }
        protected void rpn_main_pnpConsulta_btnConsultar_I_Click(object sender, EventArgs e)
        {
            string contrato = rpn_main_pnpConsulta_txtReferencia_I.Text;

            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(contrato);

            if (datosMantenimiento != null)
            {
                Boolean pasado = false;
                while (datosMantenimiento.Read())
                {
                    DateTime? FEC_BAJA_SERVICIO= (DateTime?)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "FEC_BAJA_SERVICIO");
                    
                    pasado = true;
                    string nombreCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR")).TrimEnd() + " " 
                        + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1")).TrimEnd() + " " 
                        + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2")).TrimEnd();
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNombreCli_I.Text = nombreCliente;
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtTipoEFV_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DESEFV");
                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtNif_I.Text = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");

                    rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaAltaServicio_I.Text = ((DateTime)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "FEC_ALTA_SERVICIO")).ToShortDateString();
                    if (FEC_BAJA_SERVICIO.HasValue)
                    {
                        rpn_main_CbpLoadSolicitud_pnpDatosCliente_txtFechaBajaServicio_I.Text = FEC_BAJA_SERVICIO.Value.ToShortDateString();
                    }                    
                    
                }
                if (!pasado)
                {
                    //MasterPageResponsive mp = (MasterPageResponsive)this.Master;
                    Pagina.MostrarMensaje(Resources.TextosJavaScript.CONTRATO_NO_ENCONTRADO);
                    inicializarValores();
                }
                else
                {
                    List<DocumentoDTO> lDocumentos = Documento.ObtenerDocumentosPorCodContrato(contrato);
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
            
        }

        protected void OnGrdListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    //e.Row.Attributes.Add("OnMouseOver", "Resaltar_On1(this);");
                //    //e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                //    for (int i = 0; i < e.Row.Cells.Count; i++)
                //    {
                //        String columnName = null;
                //        String columnValue = null;
                //        String strWidth = "80";
                //        //if (typeof(System.Web.UI.WebControls.BoundField) == grdListado.Columns[i].GetType())
                //        //{


                //        columnName = ((System.Data.DataRowView)(e.Row.DataItem)).Row.Table.Columns[i].ToString(); //((System.Web.UI.WebControls.BoundField)(grdListado.Columns[i])).DataField;
                //        columnValue = e.Row.Cells[i].Text.Trim();
                //        //ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla("FacturarAverias", columnName);
                //        //strWidth = (columnDefinition.Width).ToString();

                //        e.Row.Cells[i].Attributes["style"] += "cursor:default;value: " + e.Row.Cells[i].Text;
                //        e.Row.Cells[i].ToolTip = formatString(e.Row.Cells[i].Text);

                //        e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                //        //}
                //    }
                //}

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
                        //else if (documento.FechaEnvioEdatalia != null)
                        //{
                        //    ConsultarDocumentoEnEdatalia(e.CommandArgument.ToString());
                        //}
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
                        //20210923 BGN BEG R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
                        bool habilitarEnvio = false;
                        ConfiguracionDTO confHabilitarEnvio = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_ENVIO_DOC_MAIL);
                        if (confHabilitarEnvio != null && !string.IsNullOrEmpty(confHabilitarEnvio.Valor) && Boolean.Parse(confHabilitarEnvio.Valor))
                            habilitarEnvio = Boolean.Parse(confHabilitarEnvio.Valor);
                        if (habilitarEnvio)
                        {
                            if (GuardarFicheroEnvioMail(base64Result, nombreDocumento))
                            {
                                CerrarPestaña("Enviado el documento a su correo electrónico.");
                            }
                            else
                            {
                                CerrarPestaña("Error al enviar el correo con el documento.");
                            }
                        }
                        else
                        {
                            AbrirBase64EnVuelo(base64Result, nombreDocumento);
                        }
                        //20210923 BGN END R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
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

        //20210923 BGN BEG R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        private bool GuardarFicheroEnvioMail(string base64Result, string nombreFichero)
        {
            bool ok = true;
            try
            { 
                UsuarioDTO usuarioDTO = null;
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                if (usuarioDTO != null && !String.IsNullOrEmpty(usuarioDTO.Email))
                {
                    ConfiguracionDTO confRutaTemporal = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_DESTINO_DOCUMENTOS_DESCARGA_DELTA);
                    if (confRutaTemporal != null && !string.IsNullOrEmpty(confRutaTemporal.Valor))
                    {
                        string rutaDestino = confRutaTemporal.Valor;

                        using (ManejadorFicheros.GetImpersonator())
                        {
                            rutaDestino = Path.Combine(rutaDestino, nombreFichero);

                            byte[] todecode_byte = Convert.FromBase64String(base64Result);
                            File.WriteAllBytes(rutaDestino, todecode_byte);

                            bool envio = UtilidadesMail.EnviarMailConAdjunto(" FICHERO DELTA Descargado", "Adjunto Fichero DELTA ", usuarioDTO.Email, rutaDestino);

                            if (!envio)
                            {
                                throw new Exception("Error al enviar mail con fichero de delta");
                            }
                        }
                    }
                    //ManejadorFicheros.BorrarFichero(rutaDestino);
                }
            }
            catch (Exception ex)
            {
                ok = false;
                this.ManageException(ex);
            }
            return ok;
        }
        //20210923 BGN END R#33847 [Ticket combustión] Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        private void AbrirBase64EnVuelo(string base64Result, string nombreFichero)
        {
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Length", base64Result.Length.ToString());
            //Response.AddHeader("Content-Disposition", "inline; filename=" + nombreFichero);
            //Response.AddHeader("Cache-Control", "private, max-age=0, must-revalidate");
            //Response.AddHeader("Pragma", "public");
            //Response.BinaryWrite(Convert.FromBase64String(base64Result));
            //Response.OutputStream.Flush();
            //Response.OutputStream.Close();

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
       }

        private void CerrarPestaña(string mensaje)
        {
            Response.Write("<script type='text/javascript'> " +
                                    "window.opener = 'Self';" +
                                    "window.open('','_self','');" +
                                    "window.close(); " +
                                    "alert('"+ mensaje + "'); " +
                                    "</script>");
        }

    }
}
