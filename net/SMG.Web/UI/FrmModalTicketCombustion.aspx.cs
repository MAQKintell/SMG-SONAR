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
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.WS;
using System.Linq;
using System.Web;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalTicketCombustion : FrmBase
    {
        private string _filtro;
        private UsuarioDTO usuarioDTO = null;
        decimal idTicketCombustion = 0;
        bool edicionForm = false;
        string tipoVisitaEnWEB = string.Empty;

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
                //IDataReader idrTicketCombustion = null;
                
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                //Obtenemos los valores que vienen por la URL
                String codContrato = Request.QueryString["COD_CONTRATO"];
                int codVisita = Convert.ToInt16(Request.QueryString["COD_VISITA"]);
                decimal idSolicitud = Convert.ToDecimal(Request.QueryString["ID_SOLICITUD"]);
                edicionForm = (!string.IsNullOrEmpty(Request.QueryString["EDICION"])) ? Convert.ToBoolean(Request.QueryString["EDICION"]) : false;
                tipoVisitaEnWEB = Request.QueryString["TIPO_VISITA"];

                if (codVisita > 0 || idSolicitud > 0)
                {
                    //Carga del tipo de la caldera
                    if (!Page.IsPostBack)
                    {
                        CargarCombos(idSolicitud, codVisita);
                    }

                    lTicketCombustionDTO = TicketCombustion.ObtenerPorCodContratoYCodvisitaOIdSolicitud(codContrato, idSolicitud, codVisita);

                    //Si solo hay un ticket de combustion los cargamos.
                    if (lTicketCombustionDTO != null && lTicketCombustionDTO.Count > 0)
                    {
                        ticketCombustionDTO = lTicketCombustionDTO[0];

                        idTicketCombustion = Convert.ToDecimal(ticketCombustionDTO.IdTicketCombustion);

                        /*
                        if (!Page.IsPostBack)
                        {
                            idTicketCombustion = Convert.ToDecimal(ticketCombustionDTO.IdTicketCombustion);

                            //Obtenemos los ticket de combustion para mostrarlos en el grid view del formulario
                            //idrTicketCombustion = CargarListado(codContrato, idSolicitud, codVisita);

                            //if (idrTicketCombustion != null)
                            //{
                                //Cargamos el grid con los tickets de combustion
                                //dgTicketsCombustion.DataSource = idrTicketCombustion;
                                //dgTicketsCombustion.DataBind();

                            //Cargamos en pantalla los datos del ticket de combustion
                            if (lTicketCombustionDTO.Count == 1)
                            {
                                idTicketCombustion = Convert.ToDecimal(ticketCombustionDTO.IdTicketCombustion);
                                CargarDatos(idTicketCombustion);
                            }
                        }
                        */
                        if (!Page.IsPostBack)
                        {
                            CargarDatos(idTicketCombustion);

                            //Vamos a buscar si existe el id de solicitud de averia para mostrarlo en pantalla 
                            if (codVisita > 0 && PValidacionesTicketCombustion.ActivarCamposEnFormTicketWEB(codContrato, idSolicitud, codVisita, tipoVisitaEnWEB))
                            {
                                CaracteristicaHistoricoDB caracteristicaHistoricoDB = new CaracteristicaHistoricoDB();
                                string idSol = caracteristicaHistoricoDB.GetCaracteristicaNumeroSolicitudByCodContratoCodVisitaAndTipCar(codContrato, codVisita, "188");
                                if (!string.IsNullOrEmpty(idSol))
                                {
                                    TxtIdSolicitudAveria.Text = idSol;
                                    ticketCombustionDTO.IdSolicitudAveria = Decimal.Parse(idSol);
                                }
                            }
                        }

                        //Cargamos en pantalla los adjunos asociados al ticket de combustion
                        if (ticketCombustionDTO.IdFicheroConductoHumos != null || ticketCombustionDTO.IdFicheroTicketCombustion != null)
                            CargarDocumentos(ticketCombustionDTO);

                    }
                    else
                    {
                        lblMostrarResultado.Text = "No existe ticket de combustion para este contrato y " + (codVisita > 0 ? "visita" : "solicitud") + ".";
                        divMostrarResultado.Visible = true;
                    }

                    //Miramos si se deben de activar o desactivar los campos
                    ActivarDesactivarEdicionTicketCombustion(codContrato, idSolicitud, codVisita);
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

        private DataTable ObtenerDatosVisitas(string codContrato, int codVisita)
        {
            using (VisitasWSDB visitasDB = new VisitasWSDB())
            {
                return visitasDB.DatosVisitaParaCierreGet(codContrato, codVisita);
            }
        }

        private bool ActivarDesactivarEdicionTicketCombustion(string codContrato, decimal idSolicitud, int codVisita)
        {
            txtTipoEquipo.Enabled = false;
            txtTemperaturaPDC.Enabled = false;
            txtCOCorregido.Enabled = false;
            txtTiro.Enabled = false;
            txtO2.Enabled = false;
            txtCO.Enabled = false;
            txtCO2.Enabled = false;
            txtCOAmbiente.Enabled = false;
            txtLambda.Enabled = false;
            txtRendimiento.Enabled = false;
            txtTemperaturaMaxACS.Enabled = false;
            txtCaudalACS.Enabled = false;
            txtPotenciaUtil.Enabled = false;
            txtTemperaturaEntradaACS.Enabled = false;
            txtTemperaturaSalidaACS.Enabled = false;
            TxtIdSolicitudAveria.Enabled = false;
            txtComentarios.Enabled = false;
            btnGuardar.Enabled = false;

            //Hacemos visible la seccion de los documentos.
            divFicherosTicketCombustion.Visible = true;
            divFicherosVisualizar.Visible = true;
            divFicherosAdjuntar.Visible = false;

            //20210825 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
            //Mostramos los campos asociados a las nuevas comprobaciones del ticket
            if (PValidacionesTicketCombustion.ActivarCamposEnFormTicketWEB(codContrato, idSolicitud, codVisita, tipoVisitaEnWEB))
            {
                TxtIdSolicitudAveria.Visible = true;
                lblIdSolicitudAveria.Visible = true;
            }

            //20210825 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

            ConfiguracionDTO confActivarFormularioEdicion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_ACTIVAR_FORMULARIO_EDICION);
            bool isAdmin = usuarioDTO.Id_Perfil == 4; // 4 ADICO --> Administrador

            if (edicionForm || (bool.Parse(confActivarFormularioEdicion.Valor) && isAdmin))
            {
                txtTipoEquipo.Enabled = true;
                txtTemperaturaPDC.Enabled = true;
                txtCOCorregido.Enabled = true;
                txtTiro.Enabled = true;
                txtO2.Enabled = true;
                txtCO.Enabled = true;
                txtCO2.Enabled = true;
                txtCOAmbiente.Enabled = true;
                txtLambda.Enabled = true;
                txtRendimiento.Enabled = true;
                txtTemperaturaMaxACS.Enabled = true;
                txtCaudalACS.Enabled = true;
                txtPotenciaUtil.Enabled = true;
                txtTemperaturaEntradaACS.Enabled = true;
                txtTemperaturaSalidaACS.Enabled = true;
                TxtIdSolicitudAveria.Enabled = true;
                txtComentarios.Enabled = true;

                //Hacemos visible la seccion de los documentos.
                divFicherosVisualizar.Visible = true;
                divFicherosAdjuntar.Visible = true;
                btnGuardar.Enabled = true;
            }

            return true;
        }

        private IDataReader CargarListado(string codContrato, decimal? idSolicitud, int? codVisita)
        {
            IDataReader dr = TicketCombustion.ObtenerTodosPorCodContratoGridView(codContrato, idSolicitud, codVisita, (Int16)usuarioDTO.IdIdioma);
            return dr;
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        private void CargarDatos(decimal idTicketCombustion)
        {
            TicketCombustionDTO ticketCombustionDTO = null;
            ticketCombustionDTO = TicketCombustion.Obtener(idTicketCombustion);

            if (ticketCombustionDTO != null)
            { 
                //Carga del tipo de la caldera
                txtTipoEquipo.SelectedValue = ticketCombustionDTO.TipoEquipo.ToString();
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
                TxtIdSolicitudAveria.Text = ticketCombustionDTO.IdSolicitudAveria.ToString();

                txtComentarios.Text = ticketCombustionDTO.Comentarios;

                //Cargamos en pantalla los adjunos asociados al ticket de combustion
                if (ticketCombustionDTO.IdFicheroConductoHumos != null || ticketCombustionDTO.IdFicheroTicketCombustion != null)
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
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void GuardarTicketCombustion()
        {
            if (!this.IsValid)
                return;

            TicketCombustionDTO ticketCombustionDTO = new TicketCombustionDTO();

            string mensaje = string.Empty;

            lblMostrarResultado.Text = string.Empty;

            if (idTicketCombustion > 0)
                ticketCombustionDTO = TicketCombustion.Obtener(idTicketCombustion);

            //Obtenemos los valores que vienen por la URL
            String codContrato = Request.QueryString["COD_CONTRATO"];
            int codVisita = Convert.ToInt16(Request.QueryString["COD_VISITA"]);
            decimal idSolicitud = Convert.ToDecimal(Request.QueryString["ID_SOLICITUD"]);

            ticketCombustionDTO.CodigoContrato = codContrato;
            ticketCombustionDTO.CodigoVisita = codVisita;
            ticketCombustionDTO.IdSolicitud = idSolicitud;

            //Corregimos los ids por si se han guardado con 0
            ticketCombustionDTO.IdSolicitud = (ticketCombustionDTO.IdSolicitud == 0 ? null : ticketCombustionDTO.IdSolicitud);
            ticketCombustionDTO.CodigoVisita = (ticketCombustionDTO.CodigoVisita == 0 ? null : ticketCombustionDTO.CodigoVisita);

            ConfiguracionDTO confValidacionesActiva = null;

            //Obtenemos datos que nos faltan
            ticketCombustionDTO = ObtenerDatosMantenimiento(ticketCombustionDTO);

            if (!string.IsNullOrEmpty(txtTipoEquipo.SelectedItem.Value) && Convert.ToInt16(txtTipoEquipo.SelectedItem.Value) > 0)
                ticketCombustionDTO.TipoEquipo = Convert.ToInt16(txtTipoEquipo.SelectedItem.Value);

            if (!string.IsNullOrEmpty(txtTemperaturaPDC.Text))
                ticketCombustionDTO.TemperaturaPDC = Convert.ToDecimal(txtTemperaturaPDC.Text);

            if (!string.IsNullOrEmpty(txtCOCorregido.Text))
                ticketCombustionDTO.COCorregido = Convert.ToDecimal(txtCOCorregido.Text);

            if (!string.IsNullOrEmpty(txtTiro.Text))
                ticketCombustionDTO.Tiro = Convert.ToDecimal(txtTiro.Text);

            if (!string.IsNullOrEmpty(txtO2.Text))
                ticketCombustionDTO.O2 = Convert.ToDecimal(txtO2.Text);

            if (!string.IsNullOrEmpty(txtCO.Text))
                ticketCombustionDTO.CO = Convert.ToDecimal(txtCO.Text);

            if (!string.IsNullOrEmpty(txtCO2.Text))
                ticketCombustionDTO.CO2 = Convert.ToDecimal(txtCO2.Text);

            if (!string.IsNullOrEmpty(txtCOAmbiente.Text))
                ticketCombustionDTO.COAmbiente = Convert.ToDecimal(txtCOAmbiente.Text);

            if (!string.IsNullOrEmpty(txtLambda.Text))
                ticketCombustionDTO.Lambda = Convert.ToDecimal(txtLambda.Text);

            if (!string.IsNullOrEmpty(txtRendimiento.Text))
                ticketCombustionDTO.Rendimiento = Convert.ToDecimal(txtRendimiento.Text);

            if (!string.IsNullOrEmpty(txtTemperaturaMaxACS.Text))
                ticketCombustionDTO.TemperaturaMaxACS = Convert.ToDecimal(txtTemperaturaMaxACS.Text);

            if (!string.IsNullOrEmpty(txtCaudalACS.Text))
                ticketCombustionDTO.CaudalACS = Convert.ToDecimal(txtCaudalACS.Text);

            if (!string.IsNullOrEmpty(txtPotenciaUtil.Text))
                ticketCombustionDTO.PotenciaUtil = Convert.ToDecimal(txtPotenciaUtil.Text);

            if (!string.IsNullOrEmpty(txtTemperaturaEntradaACS.Text))
                ticketCombustionDTO.TemperaturaEntradaACS = Convert.ToDecimal(txtTemperaturaEntradaACS.Text);

            if (!string.IsNullOrEmpty(txtTemperaturaSalidaACS.Text))
                ticketCombustionDTO.TemperaturaSalidaACS = Convert.ToDecimal(txtTemperaturaSalidaACS.Text);

            //20210624 BUA ADD R#31134: Excepciones procesamiento peticiones Ticket Combustion
            if (!string.IsNullOrEmpty(TxtIdSolicitudAveria.Text))
                ticketCombustionDTO.IdSolicitudAveria = Convert.ToDecimal(TxtIdSolicitudAveria.Text);

            if (!string.IsNullOrEmpty(tipoVisitaEnWEB))
                ticketCombustionDTO.TipoVisita = tipoVisitaEnWEB;
            //20210624 BUA END R#31134: Excepciones procesamiento peticiones Ticket Combustion

            ticketCombustionDTO.Comentarios = txtComentarios.Text;

            //Guardamos en el objeto ticketcombustion los ficheros cargados por pantalla.
            ticketCombustionDTO = RecuperamosFicherosFromWeb(ticketCombustionDTO);

            //Validamos los datos introducidos del ticket de combustion
            bool isGuardarTicketCombustion = false;
            CierreVisitaResponse responseTicket = new CierreVisitaResponse();

            //Si se cumplen las siguientes condiciones no se obliga a introducir los documentos.
            // -- Que sea un perfil Administrador
            // -- Que venga del boton de visualizacion/modificacion del ticket combustion
            // -- Que no haya incluido los documentos
            string excepcionValidacion = RequiereExcepcionDocumentacion();

            if (!string.IsNullOrEmpty(ticketCombustionDTO.CodigoVisita.ToString()) && ticketCombustionDTO.CodigoVisita > 0)
                confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_VISITAS);
            else
                confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_SOLICITUDES);

            isGuardarTicketCombustion = PValidacionesTicketCombustion.ValidarDatosEntradaTicketCombustion(ticketCombustionDTO
                                                                                                            , responseTicket
                                                                                                            , confValidacionesActiva.Valor //"CierreVisita"
                                                                                                            , usuarioDTO.Login
                                                                                                            , null
                                                                                                            , excepcionValidacion);

            //Guardamos el ficheros
            if (!responseTicket.TieneError && isGuardarTicketCombustion)
            {                
                //Guardamos fichero de Ticket combustion
                DocumentoDTO documentoDTOTicketCombustion = null;
                documentoDTOTicketCombustion = this.GuardarFichero(ticketCombustionDTO, responseTicket, "");
                if (documentoDTOTicketCombustion != null)
                    ticketCombustionDTO.IdFicheroTicketCombustion = documentoDTOTicketCombustion.IdDocumento;

                //Guardamos fichero de Conducto de humos
                DocumentoDTO documentoDTOConductoHumos = null;
                documentoDTOConductoHumos = this.GuardarFichero(ticketCombustionDTO, responseTicket, "ConductoHumos");
                if (documentoDTOConductoHumos != null)
                    ticketCombustionDTO.IdFicheroConductoHumos = documentoDTOConductoHumos.IdDocumento;

                //Guardamos el ticket de combustion.
                TicketCombustion.GuardarTicketCombustion(ticketCombustionDTO, usuarioDTO.Login);

                //Enviamos el correo con el informe de revision de mantenimiento.
                //PValidacionesTicketCombustion.EnvioCorreoInformeRevisionMantenimiento(ticketCombustionDTO);

                string resultado = "TODO CORRECTO";
                if (responseTicket.TieneAviso)
                    resultado = responseTicket.ListaAvisos[0].Descripcion.ToString();

                //Guardamos en la tabla TICKET_COMBUSTION_RESUMEN_PROCESADOS el ticket de combustion enviado
                decimal? id = PValidacionesTicketCombustion.GuardarTicketCombustionResumenResultado(ticketCombustionDTO, "WEB", null, usuarioDTO.Login, resultado);                

                //20211004 BUA ADD R#23245: Guardar el Ticket de combustión
                //Una vez cerrada la solicitud creada por averia, cerramos la visita o solicitud que se cerro como visita erronea
                //PValidacionesTicketCombustion.CerrarVisitaSolicitudErronea(ticketCombustionDTO, (CierreSolicitudResponse)responseTicket, usuarioDTO.Login);
                //END 20211004 BUA ADD R#23245: Guardar el Ticket de combustión

            }

            //Mostramos el mensaje que se ha generado
            if (responseTicket.TieneError)           
                mensaje = "No se han superado las validaciones por el siguiente motivo '" + responseTicket.ListaErrores[0].Descripcion + "'";

            if (responseTicket.TieneAviso)
                mensaje = "Se ha guardado el ticket de combustion y se ha producido el aviso '" + responseTicket.ListaAvisos[0].Descripcion + "'." +
                    (edicionForm ? " Cierre la ventana y vuelva a pulsar sobre el boton Aceptar para asi poder cerrar la " + (codVisita > 0 ? "visita" : "solicitud") : string.Empty);
                //mensaje = "Se ha guardado el ticket de combustion y se ha producido el aviso '" + responseTicket.ListaAvisos[0].Descripcion + "'.";

            if (!responseTicket.TieneError && !responseTicket.TieneAviso)
                mensaje = "Se ha guardado Correctamente el ticket de combustion." +
                    (edicionForm ? " Cierre la ventana y vuelva a pulsar sobre el boton Aceptar para asi poder cerrar la " + (codVisita > 0 ? "visita" : "solicitud") : string.Empty);

            //Cargamos en pantalla los adjuntos asociados al ticket de combustion
            if (ticketCombustionDTO.IdFicheroConductoHumos != null || ticketCombustionDTO.IdFicheroTicketCombustion != null)
                CargarDocumentos(ticketCombustionDTO);

            lblMostrarResultado.Text = mensaje;
            divMostrarResultado.Visible = true;
            
        }

        private string RequiereExcepcionDocumentacion()
        {
            //Si se cumplen las siguientes condiciones no se obliga a introducir los documentos.
            // -- Que sea un perfil Administrador
            // -- Que venga del boton de visualizacion/modificacion del ticket combustion
            // -- Que no haya incluido los documentos
            bool isAdmin = usuarioDTO.Id_Perfil == 4; // 4 ADICO --> Administrador
            string tipoExcepcion = string.Empty;
            
            if (isAdmin && !edicionForm) //edicionform = false significa que viene del boton de visualizacion/modificacion del ticket combustion
            {
                if (!FileTicketCombustion.HasFile && !FileConductoHumos.HasFile)
                    tipoExcepcion = "NoValidarDocumento_Todos_PerfilAdministradorFormularioEdicion";
                else
                {
                    if (!FileTicketCombustion.HasFile && FileConductoHumos.HasFile)
                        tipoExcepcion = "NoValidarDocumento_TicketCombustion_PerfilAdministradorFormularioEdicion";

                    if (!FileConductoHumos.HasFile && FileTicketCombustion.HasFile)
                        tipoExcepcion = "NoValidarDocumento_ConductoHumos_PerfilAdministradorFormularioEdicion";
                }
            }

            return tipoExcepcion;
        }

        private void CargarCombos(decimal idSolicitud, int codVisita)
        {
            //Carga del tipo de la caldera
            if (codVisita > 0)
            {
                ConfiguracionDTO confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_VISITAS);
                //txtTipoEquipo.DataSource = TicketCombustion.ObtenerTiposCalderaTicketCombustion((Int16)usuarioDTO.IdIdioma, "CierreVisita");
                txtTipoEquipo.DataSource = TicketCombustion.ObtenerTiposCalderaTicketCombustion((Int16)usuarioDTO.IdIdioma, confValidacionesActiva.Valor);
            }
            else if (idSolicitud > 0)
            {
                ConfiguracionDTO confValidacionesActiva = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.TICKET_COMBUSTION_VALIDACION_ACTIVA_SOLICITUDES);
                //txtTipoEquipo.DataSource = TicketCombustion.ObtenerTiposCalderaTicketCombustion((Int16)usuarioDTO.IdIdioma, "CierreVisita");
                txtTipoEquipo.DataSource = TicketCombustion.ObtenerTiposCalderaTicketCombustion((Int16)usuarioDTO.IdIdioma, confValidacionesActiva.Valor);
            }

            //TablasReferencia.ObtenerTiposTipoCaldera((Int16)usuarioDTO.IdIdioma);
            txtTipoEquipo.DataValueField = "Id";
            txtTipoEquipo.DataTextField = "Descripcion";
            //txtTipoEquipo.SelectedValue = ticketCombustionDTO.TipoEquipo.ToString();
            txtTipoEquipo.DataBind();
            FormUtils.AddDefaultItem(txtTipoEquipo);
        }

        private TicketCombustionDTO RecuperamosFicherosFromWeb(TicketCombustionDTO ticketCombustionDTO)
        {
            //Obtenemos los datos del fichero de ticket de combustion.
            if (FileTicketCombustion.HasFile)
            {
                System.IO.Stream fsTicketCombustion = FileTicketCombustion.PostedFile.InputStream;
                System.IO.BinaryReader brTicketCombustion = new System.IO.BinaryReader(fsTicketCombustion);
                Byte[] bytesTicketCombustion = brTicketCombustion.ReadBytes((Int32)fsTicketCombustion.Length);

                ticketCombustionDTO.NombreFichero = FileTicketCombustion.FileName;
                ticketCombustionDTO.ContenidoFichero = bytesTicketCombustion;
                ticketCombustionDTO.IdFicheroTicketCombustion = null;
            }

            //Obtenemos los datos del fichero de conducto de humos.
            if (FileConductoHumos.HasFile)
            {
                System.IO.Stream fsConductoHumos = FileConductoHumos.PostedFile.InputStream;
                System.IO.BinaryReader brConductoHumos = new System.IO.BinaryReader(fsConductoHumos);
                Byte[] bytesConductoHumos = brConductoHumos.ReadBytes((Int32)fsConductoHumos.Length);

                ticketCombustionDTO.NombreFicheroConductoHumos = FileConductoHumos.FileName;
                ticketCombustionDTO.FicheroConductoHumos = bytesConductoHumos;
                ticketCombustionDTO.IdFicheroConductoHumos = null;
            }

            return ticketCombustionDTO;
        }

        private DocumentoDTO GuardarFichero(TicketCombustionDTO ticketCombustionDTO
                                                , CierreVisitaResponse response
                                                , string pNombreFichero)
        {
            try
            {
                String rutaDestino = (string)Documentos.ObtenerRutaFicheros();

                //Comprobamos que existan los campos relacionados con el fichero a procesar.
                string prefijo = (pNombreFichero == "ConductoHumos" ? "Fichero" : "ContenidoFichero");

                Object objNombreCampo = ticketCombustionDTO.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(ticketCombustionDTO, null);
                Object objValorCampo = ticketCombustionDTO.GetType().GetProperty(prefijo + pNombreFichero).GetValue(ticketCombustionDTO, null);

                if (objNombreCampo == null || string.IsNullOrEmpty(objNombreCampo.ToString())
                    || objValorCampo == null || string.IsNullOrEmpty(objValorCampo.ToString()))
                {
                    return null;
                }

                //Recuperamos el nombre del fichero
                string nombreFichero = ticketCombustionDTO.GetType().GetProperty("NombreFichero" + pNombreFichero).GetValue(ticketCombustionDTO, null).ToString();

                //Descomponemos el nombre de fichero
                string[] sNombreFichero = nombreFichero.Split('_');
                string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");

                TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);

                int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);

                string nombreficheroPRO = ticketCombustionDTO.CodigoContrato.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ticketCombustionDTO.Proveedor.Substring(0, 3) + "_" + TipoDocumentoSinExtension;

                // Añadimos la extension.
                if (nombreFichero.IndexOf(".pdf") >= 0)
                    nombreficheroPRO = nombreficheroPRO + ".pdf";
                else
                    nombreficheroPRO = nombreficheroPRO + ".tiff";


                //Construimos la ruta + nombrefichero final
                rutaDestino = rutaDestino + nombreficheroPRO.Trim();

                //Recuperamos el contenido del fichero
                byte[] tempContenidoFichero = (byte[])ticketCombustionDTO.GetType().GetProperty(prefijo + pNombreFichero).GetValue(ticketCombustionDTO, null);
                byte[] contenidoFichero = new byte[tempContenidoFichero.Length];
                contenidoFichero = tempContenidoFichero;

                // Generamos el fichero a partir del codigo base64 en la ruta temporal.
                long tamanhio = 0;
                using (ManejadorFicheros.GetImpersonator())
                {
                    // GENERAMOS EL FICHERO.
                    FileStream streamWriter = new FileStream(rutaDestino, FileMode.Create);
                    streamWriter.Write(contenidoFichero, 0, contenidoFichero.Length);
                    streamWriter.Close();

                    FileInfo info = new System.IO.FileInfo(rutaDestino);
                    tamanhio = info.Length;
                }

                if (tamanhio <= 0)
                {
                    response.AddError(CierreVisitaWSError.ErrorLongitudFichero);
                    return null;
                }

                //Insertamos los datos del documento en la tabla documento
                DocumentoDTO documentoDto = new DocumentoDTO();
                documentoDto.CodContrato = ticketCombustionDTO.CodigoContrato;
                if (!string.IsNullOrEmpty(ticketCombustionDTO.IdSolicitud.ToString()))
                    documentoDto.IdSolicitud = Convert.ToInt32(ticketCombustionDTO.IdSolicitud.ToString());

                if (!string.IsNullOrEmpty(ticketCombustionDTO.CodigoVisita.ToString()))
                    documentoDto.CodVisita = ticketCombustionDTO.CodigoVisita;
                documentoDto.NombreDocumento = nombreficheroPRO;
                documentoDto.IdTipoDocumento = idTipoDocumento;
                //20210118 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                documentoDto.EnviarADelta = true;
                //20210118 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

                documentoDto = Documento.Insertar(documentoDto, usuarioDTO.Login);

                return documentoDto;
            }
            catch (Exception ex)
            {
                response.AddError(CierreVisitaWSError.ErrorInsertFicheroAdjunto);

                return null;
            }
        }



        private TicketCombustionDTO ObtenerDatosMantenimiento(TicketCombustionDTO requesTicketCombustiontDTO)
        {
            //Obtenemos el CUPS del mantenimiento
            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(requesTicketCombustiontDTO.CodigoContrato);

            if (datosMantenimiento != null)
            {
                while (datosMantenimiento.Read())
                {

                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.Proveedor))
                        requesTicketCombustiontDTO.Proveedor = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "proveedor_averia");

                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.TelefonoContacto1))
                        requesTicketCombustiontDTO.TelefonoContacto1 = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "TELEFONO_CLIENTE1");

                    if (string.IsNullOrEmpty(requesTicketCombustiontDTO.PersonaContacto))
                        requesTicketCombustiontDTO.PersonaContacto = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR");
                }
            }

            return requesTicketCombustiontDTO;
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
                            //Response.End();
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

        //private void AbrirBase64EnVuelo(string base64Result, string nombreFichero)
        //{
        //    Response.ContentType = "application/octet-stream";
        //    Response.AddHeader("Content-Length", base64Result.Length.ToString());
        //    Response.AddHeader("Content-Disposition", "inline; filename=" + nombreFichero);
        //    Response.AddHeader("Cache-Control", "private, max-age=0, must-revalidate");
        //    Response.AddHeader("Pragma", "public");
        //    Response.BinaryWrite(Convert.FromBase64String(base64Result));
        //    Response.End();
        //}

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


        private void CargarDocumentos(TicketCombustionDTO ticketCombustionDTO)
        {
            phDocumentos.Controls.Clear();
            List<DocumentoDTO> lDocumentos = null;

            HttpContext context = HttpContext.Current;
            string navVersion = context.Request.Browser.Browser;

            //Obtenemos los documentos correspondientes al ticket de combustion
            if (ticketCombustionDTO.IdTicketCombustion > 0)
                lDocumentos = Documento.ObtenerDocumentosPorTicketCombustion(ticketCombustionDTO.CodigoContrato
                                                                                , ticketCombustionDTO.IdSolicitud
                                                                                , ticketCombustionDTO.CodigoVisita);

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

                    if (navVersion.ToUpper() == "IE")
                        linkDoc.OnClientClick = "window.document.forms[0].target='_new'; setTimeout(function(){window.document.forms[0].target='';}, 500);";

                    //linkDoc.OnClientClick = "GuardarDatos();"; //Activa el div divSplashScreen
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

        protected void OnLoadFileUpload_Click(Object sender, EventArgs e)
        {
            string[] nombreFichero = FileTicketCombustion.FileName.Split('\\');

            //var files = await picker.PickMultipleFilesAsync();
        }

        protected void DownloadButton_Click(Object sender, EventArgs e)
        {
            string base64Result = string.Empty;
            string rutaTemporal = string.Empty;

            try
            {
                LinkButton linkDoc = (LinkButton)sender;
                UtilidadesWebServices utilidadesWebServices = new UtilidadesWebServices();

                lblMostrarResultado.Text = string.Empty;

                string ccbb = String.Empty;

                string[] documento = linkDoc.Text.Split('.');
                string[] nomDoc = documento[0].Split('_');
                if (nomDoc.Length > 2)
                    foreach (string nom in nomDoc)
                        ccbb += nom;
                else
                    ccbb = nomDoc[0];

                //ccbb = "49702301202010"; //Si lo encuentra
                //ccbb = "7570678101768124"; //No lo encuentra

                //Obtenemos el fichero de ticket de combustion
                base64Result = utilidadesWebServices.llamadaWSConsultaDocumento(ccbb);

                if (string.IsNullOrEmpty(base64Result))                
                {
                    lblMostrarResultado.Text = "El documento '" + linkDoc.Text + "' no se encuentra en GESDOCOR.";
                    divMostrarResultado.Visible = true;

                    HttpContext context = HttpContext.Current;
                    string navVersion = context.Request.Browser.Browser;

                    if (navVersion.ToUpper() == "IE")
                        Response.Write("<script type='text/javascript'> " +
                                    "window.opener = 'Self';" +
                                    "window.open('','_parent','');" +
                                    "window.close(); " +
                                    "alert('El documento " + linkDoc.Text + " no se encuentra en GESDOCOR." + "'); " +
                                    "</script>");
                }
                else
                {
                    XmlDocument docXML = new XmlDocument();
                    docXML.LoadXml(base64Result);

                    XmlNamespaceManager namespaces = new XmlNamespaceManager(docXML.NameTable);
                    namespaces.AddNamespace("dlwmin", "http://ws.gesdoc.fwapp.iberdrola.com/");

                    base64Result = docXML.SelectSingleNode("//dh", namespaces).InnerText;

                    //string nombreFichero = docXML.SelectSingleNode("//nombre", namespaces).InnerText;

                    AbrirBase64EnVuelo(base64Result, linkDoc.Text);

                    //rutaTemporal = @"\\CLBILANAS01\Gesdocor\Comercial\GD_SMG\INTEGRACION\DOCUMENTOS\DELTA"; // @"c:\Temp\";
                    //ConfiguracionDTO rutaFicheros = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_RECLAMACION);
                    //rutaTemporal = rutaFicheros.Valor
                    //if (!string.IsNullOrEmpty(rutaTemporal))
                    //    AbrirBase64DesdeRutaTemporal(base64Result, rutaTemporal, linkDoc.Text);
                }
            }
            catch (Exception ex)
            {
                //this.ManageException(ex);
            }
            finally
            {
                //Cerramos la ventana de la capa de espera
                //string Script = "<script language='javascript'>OcultarCapaEspera();</script>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);                
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

        protected void OnBtnGuardar_Click(object sender, EventArgs e)
        {

            this.GuardarTicketCombustion();

            //Cerramos la ventana de la capa de espera
            string Script = "<script language='javascript'>OcultarCapaEspera();</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);

            //this.CerrarVentanaModal();
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
                                        
                    idTicketCombustion = Decimal.Parse(lbtn.Text);

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
