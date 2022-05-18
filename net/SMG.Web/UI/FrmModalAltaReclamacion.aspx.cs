using System;
using System.Web.UI;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Exceptions;
using System.Web.UI.WebControls;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Logging;
using System.IO;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalAltaReclamacion : FrmBase
    { 
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnDevolver.Enabled = false;
                UsuarioDTO user1 = Usuarios.ObtenerUsuarioLogeado();
                int idPerfil = int.Parse(user1.Id_Perfil.ToString());

                if (!Page.IsPostBack)
                {
                    try
                    {
                        // Load the providers.
                        CargaComboProveedores();
                        // Select the provider.
                        string proveedor = Request.QueryString["Proveedor"];
                        cmbProveedor.SelectedValue = proveedor.ToUpper().Substring(0, 3);
                        //************************************************************************************************
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("Error loading Providers", LogHelper.Category.BussinessLogic);
                        LogHelper.Error(ex.Message, LogHelper.Category.BussinessLogic);
                        LogHelper.Error(ex.StackTrace, LogHelper.Category.BussinessLogic);
                    }
                    lblContrato.Text = Request.QueryString["Contrato"];
                    hdnIdSolicitud.Value = Request.QueryString["idSolicitud"];
                    hdnEstado.Value = Request.QueryString["Estado"];
                    if (hdnIdSolicitud.Value != null && hdnIdSolicitud.Value != "undefined" && hdnIdSolicitud.Value != "")
                    {
                        txtObservacionesAnteriores.Enabled = true;
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                        txtObservacionesAnteriores.Text = objSolicitudesDB.ObtenerObservacionesAnterioresPorIdSolicitud(int.Parse(hdnIdSolicitud.Value)).Replace("#", "\r");
                        txtObservacionesAnteriores.Text = txtObservacionesAnteriores.Text.Replace("\r", "\r\n");
                        UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                        if (!Usuarios.EsProveedor(int.Parse(user.Id_Perfil.ToString())))
                        {
                            if (hdnEstado.Value != "Pdte.Respuesta" && hdnEstado.Value != "undefined" && hdnEstado.Value != "046")
                            {
                                btnDevolver.Enabled = true;
                            }
                            divReclamacion.Visible = true;
                        }
                        else
                        {
                            if (hdnEstado.Value == "Pdte.Respuesta" || hdnEstado.Value == "046")
                            {
                                btnAceptar.Enabled = true;
                            }
                            else
                            {
                                btnAceptar.Enabled = false;
                            }
                            divReclamacion.Visible = false;
                        }
                        if (hdnEstado.Value == "Cerrada" || hdnEstado.Value == "047")
                        {
                            btnDevolver.Enabled = false;
                            btnAceptar.Enabled = false;
                        }
                        //txtObservacionesAnteriores.Enabled = false;                    
                    }

                    
                    if (Usuarios.EsProveedor(idPerfil))
                    {
                        cmbProveedor.Visible = false;
                        Label1.Visible = false;
                    }
                    //20200113 R#9867 Añadir validación para evitar que el teléfono pueda asignar proveedor en solicitudes de reclamación.
                    else if (Usuarios.EsTelefono(idPerfil) || Usuarios.EsReclamacion(idPerfil))
                    {
                        cmbProveedor.Enabled = false;
                    }
                }

                //20200520 BGN ADD BEG R#21754: Colgar facturas en información de reclamación
                if (hdnIdSolicitud.Value != null && hdnIdSolicitud.Value != "undefined" && hdnIdSolicitud.Value != "")
                {
                    CargarDocumentos();
                }
                //20200520 BGN ADD END R#21754: Colgar facturas en información de reclamación

                // Kintell: 14/07/2020 R#25437
                btnCerrarReclamacion.Visible = false;
                if (hdnEstado.Value != "047" && (Usuarios.EsAdministrador(idPerfil) || Usuarios.EsReclamacion(idPerfil)))
                { 
                    btnCerrarReclamacion.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }
        private void CargaComboProveedores()
        {
            //20200511 BGN MOD BEG [R#23938]: Parametrización proveedor reclamación.
            UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
            Proveedor prov = new Proveedor();
            cmbProveedor.Items.Clear();
            cmbProveedor.DataSource = prov.GetProveedorAltaReclamacion(user.Pais.ToUpper());
            cmbProveedor.DataValueField = "COD_PROVEEDOR";
            cmbProveedor.DataTextField = "nombre";            
            cmbProveedor.DataBind();
            //20200511 BGN MOD END [R#23938]: Parametrización proveedor reclamación.
        }

        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        protected void OnBtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                string usuario = user.Login;
                string Observaciones = "";
                Observaciones = "[" + DateTime.Now.ToString() + "] " + usuario + ": " + txtObservaciones.Text + "#" + txtObservacionesAnteriores.Text;
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                if (Usuarios.EsProveedor(int.Parse(user.Id_Perfil.ToString())))
                {
                    // RETURN THE ISSUE TO THE OTHER PERFIL.
                    objSolicitudesDB.EvolutionStateSolicitudReclamacion(hdnIdSolicitud.Value, Observaciones, usuario, true, "", cmbProveedor.SelectedValue);
                }
                else
                {
                    if ((hdnIdSolicitud.Value != "undefined" && hdnIdSolicitud.Value != "") && (hdnEstado.Value != "Pdte.Respuesta" && hdnEstado.Value != "046"))
                    {
                        // CLOSE THE ISSUE.
                        objSolicitudesDB.EvolutionStateSolicitudReclamacion(hdnIdSolicitud.Value, Observaciones, usuario, false, "047", cmbProveedor.SelectedValue);
                    }
                    else
                    {
                        if (hdnEstado.Value == "Pdte.Respuesta" || hdnEstado.Value == "046")
                        {
                            // UPDATE OBSERVACIONES.
                            objSolicitudesDB.EvolutionStateSolicitudReclamacion(hdnIdSolicitud.Value, Observaciones, usuario, false, "046", cmbProveedor.SelectedValue);
                        }
                        else
                        {
                            // CREATE THE ISSUE.
                            objSolicitudesDB.AddSolicitudReclamacion(lblContrato.Text, user.Nombre + ": " + Observaciones, usuario, cmbProveedor.SelectedValue);
                        }
                    }
                }
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión

            // cargar los valores en el formulario

            // eliminar de sesión los valores del formulario
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario

            // cargar los valores en sessión
        }
        public override void DeleteSessionData()
        {
            // eliminar de sesión los valores del formulario si existían
        }
        #endregion
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                string usuario = user.Login;
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();

                string Observaciones = "";
                Observaciones = "[" + DateTime.Now.ToString() + "] " + usuario + ": " + txtObservaciones.Text + "#" + txtObservacionesAnteriores.Text;

                // RETURN THE ISSUE.
                objSolicitudesDB.EvolutionStateSolicitudReclamacion(hdnIdSolicitud.Value, Observaciones, usuario, false, "046",cmbProveedor.SelectedValue);
               
               
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnCerrarReclamacion_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                string usuario = user.Login;
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();

                string Observaciones = "";
                Observaciones = "[" + DateTime.Now.ToString() + "] " + usuario + ": " + txtObservaciones.Text + "#" + txtObservacionesAnteriores.Text;

                // RETURN THE ISSUE.
                objSolicitudesDB.EvolutionStateSolicitudReclamacion(hdnIdSolicitud.Value, Observaciones, usuario, false, "047", cmbProveedor.SelectedValue);


                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        //20200520 BGN ADD BEG R#21754: Colgar facturas en información de reclamación
        protected void btnAdjuntarFichero_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnIdSolicitud.Value != "undefined" && hdnIdSolicitud.Value != "")
                {
                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    string usuario = user.Login;
                    string ruta = String.Empty; ;
                    ConfiguracionDTO rutaFicherosReclamacion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_RECLAMACION);
                    if (rutaFicherosReclamacion != null && !string.IsNullOrEmpty(rutaFicherosReclamacion.Valor) && !string.IsNullOrEmpty(FileUpload1.FileName))
                    {
                        using (ManejadorFicheros.GetImpersonator())
                        {
                            ruta = rutaFicherosReclamacion.Valor;//@"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                            ruta = ManejadorFicheros.CombinarRutas(ruta, lblContrato.Text);
                            ruta = ManejadorFicheros.CombinarRutas(ruta, hdnIdSolicitud.Value);
                            ruta = ManejadorFicheros.CombinarRutas(ruta, FileUpload1.FileName);
                            bool ficheroSubido = ManejadorFicheros.SubirFichero(FileUpload1, ruta);
                            if (ficheroSubido)
                            {
                                //Guardar en la tabla DOCUMENTOS
                                ConfiguracionDTO confIdTipoDocReclamacion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ID_TIPO_DOCUMENTO_RECLAMACION);                                
                                int idTipoDocumento = int.Parse(confIdTipoDocReclamacion.Valor);//6
                                int idSolicitud = int.Parse(hdnIdSolicitud.Value);
                                //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                //Documentos.InsertaDocumentoCargado(lblContrato.Text, null, idSolicitud, FileUpload1.FileName, idTipoDocumento, null, null, usuario);
                                DocumentoDTO documentoDto = new DocumentoDTO();
                                documentoDto.CodContrato = lblContrato.Text;
                                documentoDto.IdSolicitud = idSolicitud;
                                documentoDto.NombreDocumento = FileUpload1.FileName;
                                documentoDto.IdTipoDocumento = idTipoDocumento;
                                documentoDto.EnviarADelta = true;
                                documentoDto = Documento.Insertar(documentoDto, usuario);
                                //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 
                                CargarDocumentos();
                            }
                            else
                            {
                                ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_ADJUNTAR_FICHERO_ERROR_NO_DEFINIDO); //"No se ha podido adjuntar el fichero. Error no definido."
                            }
                        }
                    }
                    else
                    {
                        ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_ADJUNTAR_FICHERO_ERROR_RUTA_DESTINO); //"No se ha podido adjuntar el fichero. Error en la ruta destino."
                    }
                }
                else
                {
                    ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_ADJUNTAR_FICHERO_ALTA_SOLICITUD);//"No se ha podido adjuntar el fichero. Debe darse de alta antes la solicitud."
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void DownloadButton_Click(Object sender, EventArgs e)
        {
            string ruta = String.Empty;
            try
            { 
                LinkButton linkDoc = (LinkButton)sender;                
                ConfiguracionDTO rutaFicherosReclamacion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_RECLAMACION);
                if (rutaFicherosReclamacion != null && !string.IsNullOrEmpty(rutaFicherosReclamacion.Valor))
                {
                    using (ManejadorFicheros.GetImpersonator())
                    {
                        ruta = rutaFicherosReclamacion.Valor;//@"\\172.20.46.45\ADMON_PYS\PRODUCTOS Y SERVICIOS\08 SMG\";
                        ruta = ManejadorFicheros.CombinarRutas(ruta, lblContrato.Text);
                        ruta = ManejadorFicheros.CombinarRutas(ruta, hdnIdSolicitud.Value);
                        ruta = ManejadorFicheros.CombinarRutas(ruta, linkDoc.Text);
                        if (File.Exists(ruta))
                        {
                            Response.Clear();
                            Response.ContentType = "application/octet-stream";
                            Response.AppendHeader("content-disposition", "attachment; filename=" + ruta);
                            Response.TransmitFile(ruta);
                            Response.Flush();
                            Response.End();
                        }
                        else
                        {
                            ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_DESCARGAR_FICHERO);// "No se ha podido encontrar el fichero. Póngase en contracto con sistemas."
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                //UtilidadesMail.EnviarMensajeError(" INCIDENCIA EN OPERA SMG.", "Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
            }
        }

        private void CargarDocumentos()
        {
            phDocumentos.Controls.Clear();

            List<DocumentoDTO> lDocumentos = Documento.ObtenerDocumentosPorIdSolicitud(int.Parse(hdnIdSolicitud.Value));
            if (lDocumentos.Count > 0)
            {
                for (int i = 0; i < lDocumentos.Count; i++)
                {
                    LinkButton linkDoc = new LinkButton();
                    linkDoc.ID = "linkDoc" + lDocumentos[i].IdDocumento;
                    linkDoc.Text = lDocumentos[i].NombreDocumento;
                    linkDoc.CssClass = "linkFormulario";
                    linkDoc.Visible = true;
                    linkDoc.EnableViewState = false;
                    linkDoc.Enabled = true;
                    linkDoc.Click += new EventHandler(DownloadButton_Click);
                    linkDoc.CommandArgument = Convert.ToString(lDocumentos[i].IdDocumento);
                    
                    phDocumentos.Controls.Add(linkDoc);
                    phDocumentos.Controls.Add(new LiteralControl("&nbsp;"));
                }
            }
        }
        //20200520 BGN ADD END R#21754: Colgar facturas en información de reclamación
    }
}
