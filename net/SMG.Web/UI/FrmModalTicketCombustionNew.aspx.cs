using System;
using System.Data;
using System.Web.UI;
using Iberdrola.Commons.Configuration;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using System.IO;
using System.Xml;
using Iberdrola.Commons.Utils;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalTicketCombustionNew : FrmBase
    {
        private string _filtro;

        private IDataReader CargarListado(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            UsuarioDTO usuarioDTO = null;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //IDataReader dr = Contrato.ObtenerVisitas(codContrato, (Int16)usuarioDTO.IdIdioma);
            IDataReader dr = TicketCombustion.ObtenerTodosPorCodContratoGridView(codContrato, idSolicitud, codVisita, (Int16)usuarioDTO.IdIdioma);
            return dr;
        }

        /// <summary>
        /// Evento de inicialización de la página.
        /// Carga los datos de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool resultadoOK = true;

            try
            {
                List<TicketCombustionDTO> lTicketCombustionDTO = null;
                TicketCombustionDTO ticketCombustionDTO = null;
                IDataReader idrTicketCombustion = null;

                //Obtenemos los valores que vienen por la URL
                String codContrato = Request.QueryString["COD_CONTRATO"];
                int codVisita = Convert.ToInt16(Request.QueryString["COD_VISITA"]);
                decimal idSolicitud = Convert.ToDecimal(Request.QueryString["ID_SOLICITUD"]);

                if (codVisita > 0 || idSolicitud > 0)
                {
                    lTicketCombustionDTO = TicketCombustion.ObtenerPorCodContratoYCodvisitaOIdSolicitud(codContrato, idSolicitud, codVisita);

                    //Si solo hay un ticket de combustion los cargamos.
                    if (lTicketCombustionDTO != null)
                    {
                        ticketCombustionDTO = lTicketCombustionDTO[0];

                        if (!Page.IsPostBack)
                        {
                            //Obtenemos los ticket de combustion para mostrarlos en el grid view del formulario
                            idrTicketCombustion = CargarListado(codContrato, idSolicitud, codVisita);

                            if (idrTicketCombustion != null)
                            {
                                //Cargamos el grid con los tickets de combustion
                                dgTicketsCombustion.DataSource = idrTicketCombustion;
                                dgTicketsCombustion.DataBind();

                                //Cargamos en pantalla los datos del ticket de combustion
                                if (lTicketCombustionDTO.Count == 1)
                                {
                                    decimal idTicketCombustion = Convert.ToDecimal(ticketCombustionDTO.IdTicketCombustion);
                                    CargarDatos(idTicketCombustion);
                                }
                            }
                        }

                        //Cargamos en pantalla los adjunos asociados al ticket de combustion
                        if (ticketCombustionDTO.IdFicheroConductoHumos != null || ticketCombustionDTO.IdFicheroTicketCombustion != null)
                        {
                            CargarDocumentos(ticketCombustionDTO);
                        }
                    }
                }
                else
                {
                    resultadoOK = false;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

            if (!resultadoOK)
            {
                this.CerrarVentanaModal();
            }
        }



        private void CargarDatos(decimal idTicketCombustion)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            TicketCombustionDTO ticketCombustionDTO = TicketCombustion.Obtener(idTicketCombustion);

            //Carga del tipo de la caldera
            txtTipoEquipo.DataSource = TablasReferencia.ObtenerTiposTipoCaldera((Int16)usuarioDTO.IdIdioma);
            txtTipoEquipo.DataValueField = "Id";
            txtTipoEquipo.DataTextField = "Descripcion";
            txtTipoEquipo.SelectedValue = ticketCombustionDTO.TipoEquipo.ToString();
            txtTipoEquipo.DataBind();
            FormUtils.AddDefaultItem(txtTipoEquipo);

            txtTemperaturaPDC.Text = ticketCombustionDTO.TemperaturaPDC.ToString();
            txtCOCorregido.Text = ticketCombustionDTO.COCorregido.ToString();
            txtTiro.Text = (ticketCombustionDTO.Tiro != null) ? ticketCombustionDTO.Tiro.ToString() : "";
            txtO2.Text = ticketCombustionDTO.O2.ToString();
            txtCO.Text = ticketCombustionDTO.CO.ToString();
            txtCO2.Text = ticketCombustionDTO.CO2.ToString();
            txtCOAmbiente.Text = ticketCombustionDTO.COAmbiente.ToString();
            txtLambda.Text = ticketCombustionDTO.Lambda.ToString();
            txtRendimiento.Text = ticketCombustionDTO.Rendimiento.ToString();
            txtTemperaturaMaxACS.Text = ticketCombustionDTO.TemperaturaMaxACS.ToString();
            txtCaudalACS.Text = ticketCombustionDTO.CaudalACS.ToString();
            txtPotenciaUtil.Text = ticketCombustionDTO.PotenciaUtil.ToString();
            txtTemperaturaEntradaACS.Text = ticketCombustionDTO.TemperaturaEntradaACS.ToString();
            txtTemperaturaSalidaACS.Text = ticketCombustionDTO.TemperaturaSalidaACS.ToString();
            txtComentarios.Text = ticketCombustionDTO.Comentarios;

            //Cargamos en pantalla los adjunos asociados al ticket de combustion
            if (ticketCombustionDTO.IdFicheroConductoHumos != null || ticketCombustionDTO.IdFicheroTicketCombustion != null)
            {
                CargarDocumentos(ticketCombustionDTO);
            }
        }

        /// <summary>
        /// Trata de cerrar la ventana modal
        /// </summary>
        protected void CerrarVentanaModal()
        {
            try
            {
                MasterPageModalResponsive mpm = (MasterPageModalResponsive)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void DownloadButton_Click(Object sender, EventArgs e)
        {
            string base64Result = string.Empty;
            string rutaTemporal = string.Empty;

            try
            {
                LinkButton linkDoc = (LinkButton)sender;

                //Obtenemos el fichero de ticket de combustion
                UtilidadesWebServices utilidadesWebServices = new UtilidadesWebServices();
                string ccbb = "49702301202010";
                base64Result = utilidadesWebServices.llamadaWSConsultaDocumento(ccbb);

                XmlDocument docXML = new XmlDocument();
                docXML.LoadXml(base64Result);

                XmlNamespaceManager namespaces = new XmlNamespaceManager(docXML.NameTable);
                namespaces.AddNamespace("dlwmin", "http://ws.gesdoc.fwapp.iberdrola.com/");

                base64Result = docXML.SelectSingleNode("//dh", namespaces).InnerText;
                             
                if (!string.IsNullOrEmpty(base64Result))
                {
                    //ConfiguracionDTO rutaFicheros = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_RECLAMACION);
                    //if (rutaFicheros != null && !string.IsNullOrEmpty(rutaFicheros.Valor))
                    //{
                    //    rutaTemporal = @"c:\Temp\";
                    //    AbrirBase64DesdeRutaTemporal(base64Result, rutaTemporal, linkDoc.Text);
                    //}

                    AbrirBase64EnVuelo(base64Result, linkDoc.Text);
                }
            }
            catch (Exception ex)
            {
                //UtilidadesMail.EnviarMensajeError(" INCIDENCIA EN OPERA SMG.", "Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
            }
        }

        private void AbrirBase64DesdeRutaTemporal(string base64Result, string rutaTemporal, string nombreFichero)
        {
            try
            {
                if (Directory.Exists(rutaTemporal))
                {
                    rutaTemporal = ManejadorFicheros.CombinarRutas(rutaTemporal, nombreFichero);

                    //Guardamos el fichero en una ruta para poder abrirlo posteriormente.
                    byte[] todecode_byte = Convert.FromBase64String(base64Result);
                    File.WriteAllBytes(rutaTemporal, todecode_byte);

                    using (ManejadorFicheros.GetImpersonator())
                    {
                        if (File.Exists(rutaTemporal))
                        {
                            Response.Clear();
                            Response.ContentType = "application/octet-stream";
                            Response.AppendHeader("content-disposition", "attachment; filename=" + rutaTemporal);
                            Response.TransmitFile(rutaTemporal);
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
                throw;
            }
        }

        private void AbrirBase64EnVuelo(string base64Result, string nombreFichero)
        {
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", base64Result.Length.ToString());
            Response.AddHeader("Content-Disposition", "inline; filename=" + nombreFichero);
            Response.AddHeader("Cache-Control", "private, max-age=0, must-revalidate");
            Response.AddHeader("Pragma", "public");
            Response.BinaryWrite(Convert.FromBase64String(base64Result));
            Response.End();
        }

        private void CargarDocumentos(TicketCombustionDTO ticketCombustionDTO)
        {
            phDocumentos.Controls.Clear();
            List<DocumentoDTO> lDocumentos = null;

            //Obtenemos los documentos correspondientes al ticket de combustion
            if (ticketCombustionDTO.IdTicketCombustion > 0)
                lDocumentos = Documento.ObtenerDocumentosPorTicketCombustion(ticketCombustionDTO.CodigoContrato
                                                                                , ticketCombustionDTO.IdSolicitud
                                                                                , ticketCombustionDTO.CodigoVisita);
            //else
            //    lDocumentos = Documento.ObtenerDocumentosPorTicketCombustion(ticketCombustionDTO.CodigoContrato
            //                                                                                , null
            //                                                                                , ticketCombustionDTO.CodigoVisita);

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
                    linkDoc.OnClientClick = "window.document.forms[0].target='_new'; setTimeout(function(){window.document.forms[0].target='';}, 500);";
                    linkDoc.Click += new EventHandler(DownloadButton_Click);
                    linkDoc.CommandArgument = Convert.ToString(lDocumentos[i].IdDocumento);

                    string texto = string.Empty;
                    if (ticketCombustionDTO.IdFicheroTicketCombustion == lDocumentos[i].IdDocumento)
                        texto = "- Ticket combustión: ";
                    else
                        texto = "- Conducto humos: ";

                    LiteralControl a = new LiteralControl();
                    a.Text = "<font Class='labelFormulario'>" + texto + "</font>" ;

                    //phDocumentos.Controls.Add(new LiteralControl(texto + "&nbsp;"));
                    phDocumentos.Controls.Add(a);
                    phDocumentos.Controls.Add(linkDoc);
                    phDocumentos.Controls.Add(new LiteralControl("<br />"));
                }
            }
        }

        /// <summary>
        /// Evento del botón cancelar de la pantalla.
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBtnCancelar_Click(object sender, EventArgs e)
        {
            this.CerrarVentanaModal();
        }
        protected void dgTicketsCombustion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                    e.Row.Attributes["OnClick"] =
                        Page.ClientScript.GetPostBackClientHyperlink(this.dgTicketsCombustion, "Select$" + e.Row.RowIndex.ToString());

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        String columnName = null;
                        if (typeof(System.Web.UI.WebControls.BoundField) == this.dgTicketsCombustion.Columns[i].GetType())
                        {
                            columnName = ((System.Web.UI.WebControls.BoundField)(dgTicketsCombustion.Columns[i])).DataField;

                            //e.Row.Cells[i].Visible = false;
                            ColumnDefinition columnDefinition = DefinicionColumnas.GetColumnaTabla(dgTicketsCombustion.ID, columnName);
                            string strWidth = (columnDefinition.Width).ToString();

                            //e.Row.Cells[i].Attributes["style"] += "cursor:hand;";
                            //e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                            e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                            e.Row.Cells[i].Text = "<div style='width:" + strWidth + "px; white-space: nowrap; overflow:hidden;'>" + e.Row.Cells[i].Text.Trim() + "</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
            finally
            {
                //hdnNumeroVisitas.Value = ContVisitas.ToString();
            }
        }

        protected void OnRowSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().FullName.ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                {
                    LinkButton lbtn = (LinkButton)sender;

                    decimal idTicketCombustion = Decimal.Parse(lbtn.Text);

                    if (idTicketCombustion > 0)
                    {
                        CargarDatos(idTicketCombustion);
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void dgTicketsCombustion_DataBinding(object sender, EventArgs e)
        {
            if (this.dgTicketsCombustion != null)
            {
                int codVisita = Convert.ToInt16(Request.QueryString["COD_VISITA"]);
                decimal idSolicitud = Convert.ToDecimal(Request.QueryString["ID_SOLICITUD"]);

                if (idSolicitud > 0 || codVisita == 0)
                {
                    dgTicketsCombustion.Columns[1].Visible = true;
                    dgTicketsCombustion.Columns[2].Visible = false;
                }
                else
                {
                    dgTicketsCombustion.Columns[1].Visible = false;
                    dgTicketsCombustion.Columns[2].Visible = true;
                }
            }
        }

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



    }
}
