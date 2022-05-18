using System;
using System.Collections.Generic;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;  
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections;
using System.Data;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Reporting;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.DataAccess;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModificarEstadosSolicitud : FrmBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strTipoActuacion = "";
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["IdSolicitud"] != null)
                    {
                        strTipoActuacion = "SOLICITUD";

                        string strIdSolicitud = null;
                        strIdSolicitud = Request.QueryString["IdSolicitud"].ToString();

                        if (!string.IsNullOrEmpty(strIdSolicitud))
                        {
                            CargarDatosSolicitud(strIdSolicitud);

                        }
                        
                    }
                    else if (Request.QueryString["IdVisita"] != null)
                    {
                        strTipoActuacion = "VISITA";

                        string strIdVisita = null;
                        strIdVisita = Request.QueryString["IdVisita"].ToString();

                        if (!string.IsNullOrEmpty(strIdVisita))
                        {
                            CargarDatosVisita(strIdVisita, Request.QueryString["CodContrato"]);
                            this.btnEliminar.Enabled = false; 
                        }
                    }

                    this.lblTituloVentana.Text = this.lblTituloVentana.Text.Replace("<<tipo>>", strTipoActuacion);
                    this.lblSubtitulo.Text = this.lblSubtitulo.Text.Replace("<<tipo>>", strTipoActuacion);
                   
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        private void CargarDatosSolicitud(string strIdSolicitud)
        {
            
            SolicitudesDB solDB = new SolicitudesDB();
            IDataReader dr = null;
            string sNumFactura = null;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = null;
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            this.hdnIdSolicitud.Value = strIdSolicitud;            

            try
            {
                dr = solDB.GetSingleSolicitudes(strIdSolicitud,usuarioDTO.IdIdioma);

                if (dr.Read())
                {
                    string desEstadoSolicitud = DataBaseUtils.GetDataReaderColumnValue(dr, "Des_Estado_solicitud").ToString();
                    lblEstadoActual.Text = desEstadoSolicitud;

                    hdnCodEstadoActual.Value = DataBaseUtils.GetDataReaderColumnValue(dr, "ESTADO_SOLICITUD").ToString();
                    string strSubtipoSolicitud = DataBaseUtils.GetDataReaderColumnValue(dr, "SUBTIPO_SOLICITUD").ToString();
                    
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "NUM_FACTURA") == null)
                    {
                        sNumFactura = "";
                    }
                    else
                    {
                        sNumFactura = DataBaseUtils.GetDataReaderColumnValue(dr, "NUM_FACTURA").ToString();
                    }
                    EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();
                    

                    ddlEstadosSolicitud.Items.Clear();
                    ddlEstadosSolicitud.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipo("001", strSubtipoSolicitud,usuarioDTO.IdIdioma);
                    ddlEstadosSolicitud.DataTextField = "descripcion";
                    ddlEstadosSolicitud.DataValueField = "codigo";
                    ddlEstadosSolicitud.DataBind();


                    if ((ddlEstadosSolicitud.Items.Count == 0))
                    {
                        ListItem defaultItem = new ListItem();
                        defaultItem.Value = "-1";
                        defaultItem.Text = Resources.TextosJavaScript.TEXTO_NINGUNO;// "Ninguno";
                        ddlEstadosSolicitud.Items.Insert(0, defaultItem);
                    }
                    else
                    {
                        FormUtils.AddDefaultItem(ddlEstadosSolicitud);
                    }
                }
                if (dr != null)
                {
                    dr.Close();
                }
                this.txtNumFactura.Text = sNumFactura;
                this.txtNumFactura.Enabled = false; 
                // Si esta liquidada hay que preguntar si se quiere modificar el estado.
                dr = solDB.GetDatosLiquidacionPorSolicitud(strIdSolicitud);
                hdnLiquidado.Value = "0";
                if (dr.Read())
                {
                    hdnLiquidado.Value = "1";
                }
            }
            catch
            {
                throw;
            }
            finally 
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }


            HistoricoDB hisDB = new HistoricoDB();
            DataSet ds = hisDB.GetHistoricoSolicitud(strIdSolicitud,usuarioDTO.IdIdioma);

            if (ds != null && ds.Tables.Count > 0)
            {
                this.dgSolicitudesHistorico.DataSource = ds.Tables[0];
                this.dgSolicitudesHistorico.DataBind();
            }
        }

        private void CargarDatosVisita(string strIdVisita, string strCodContrato)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            this.hdnIdVisita.Value = strIdVisita;
            this.hdnCodContrato.Value = strCodContrato;
            this.txtNumFactura.Enabled = true; 

            Visitas visitas = new Visitas();
            VisitaDTO visitasDTO = visitas.DatosVisitas(strCodContrato, strIdVisita);

            //Recuperamos los estados posibles de una solicitud
            List<TipoEstadoVisitaDTO> listaEstadosVisita = TablasReferencia.ObtenerTiposEstadoVisita((Int16)usuarioDTO.IdIdioma);

            // Cargamos la lista de estados
            this.ddlEstadosSolicitud.DataSource = listaEstadosVisita;
            this.ddlEstadosSolicitud.DataValueField = "Codigo";
            this.ddlEstadosSolicitud.DataTextField = "Descripcion";
            this.ddlEstadosSolicitud.DataBind();
            FormUtils.AddDefaultItem(ddlEstadosSolicitud);

            hdnCodEstadoVisitaActual.Value = visitasDTO.CodigoEstadoVisita.ToString();

            //Rellenamos el numFactura
            if (visitasDTO.NumFactura != null)
            {
                txtNumFactura.Text = visitasDTO.NumFactura.ToString();
                if (!string.IsNullOrEmpty(this.txtNumFactura.Text))
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_VISITA_MODIFICAR_FACTURADA);//"La visita que va a modificar esta Facturada. Si no le quita el numero de factura, no podra modificarla, aunque cambie el estado.");
                }
            }
            else
            {
                txtNumFactura.Text = "";
            }

            if (visitasDTO != null)
            {
                // Cargamos el estado actual de la solicitud.
                foreach(TipoEstadoVisitaDTO estadoDto in listaEstadosVisita)
                {
                    if (estadoDto.Codigo.Equals(visitasDTO.CodigoEstadoVisita))
                    {
                        this.lblEstadoActual.Text = estadoDto.Descripcion;
                        break;
                    }
                }
            }

            
            try
            {
                //drHis = visitas.ObtenerHistoricoVisita(strCodContrato, strIdVisita);

                //if (drHis.Read())
                //{
                //    this.dgSolicitudesHistorico.DataSource = drHis;
                //    this.dgSolicitudesHistorico.DataBind();
                //}


                HistoricoDB hisDB = new HistoricoDB();
                DataTable dt = visitas.ObtenerHistoricoVisitaPorIdVisita(strCodContrato, strIdVisita,(Int16)usuarioDTO.IdIdioma);

                if (dt.Rows.Count   > 0)
                {
                    this.dgSolicitudesHistorico.DataSource = dt;
                    this.dgSolicitudesHistorico.DataBind();
                }

                SolicitudesDB solDB = new SolicitudesDB();
                IDataReader dr = null;

                // Si esta liquidada hay que preguntar si se quiere modificar el estado.
                dr = solDB.GetDatosLiquidacionPorVisita(strCodContrato, strIdVisita);
                hdnLiquidado.Value = "0";
                if (dr.Read())
                {
                    hdnLiquidado.Value = "1";
                }
                
            }
            catch
            {
                throw;
            }
            finally
            {
               
            }

        }

        /// <summary>
        /// Eliminamos la solicitud, el historico y sus caracteristicas e historico asociado.
        /// </summary>
        /// <param name="strIdSolicitud"></param>
        private void EliminarSolicitud(string strIdSolicitud)
        {
            try
            {
                //Actualizamos el numFactura con lo que haya metido el usuario
                string sNumFactura = this.txtNumFactura.Text;

                SolicitudesDB solDB = new SolicitudesDB();
                HistoricoDB histDB = new HistoricoDB();
                CaracteristicasDB carDB = new CaracteristicasDB();


                //Eliminamos las caracteristicas y el historico de las caracteristicas para esa solicitud.
                carDB.EliminarCaracteristica_e_Historico_Solicitud(strIdSolicitud);

                // En el historico hay que actualizar los movimientos del estado anterior con el estado nuevo.
                histDB.EliminarHistoricoSolicitud(strIdSolicitud);

                // Actualizamos el estado de la solicitud y el numFactura que haya metido.
                solDB.EliminarSolicitud(strIdSolicitud);


                MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ELIMINADA + strIdSolicitud);//"La solicitud " + strIdSolicitud + " ha sido eliminada.");

            }
            catch (Exception ex)
            {
            }
        }



        private void GuardarCambioEstadoSolicitud(string strIdSolicitud)
        {
            try
            {
                // Obtenemos las observaciones y le añadimos las nuevas observaciones.
                string strObservacionesAnteriores = ObtenerObservacionesSolicitud(strIdSolicitud);

                string Horas = DateTime.Now.Hour.ToString();
                if (Horas.Length == 1) Horas = "0" + Horas;
                string Minutos = DateTime.Now.Minute.ToString();
                if (Minutos.Length == 1) Minutos = "0" + Minutos;
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                String usuario = usuarioDTO.Login;
                string strObservaciones = "[" + DateTime.Now.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + txtObservaciones.Text + (char)(13) + strObservacionesAnteriores;

                ////Actualizamos el numFactura con lo que haya metido el usuario
                //string sNumFactura = this.txtNumFactura.Text; 

                SolicitudesDB solDB = new SolicitudesDB();
                HistoricoDB histDB = new HistoricoDB();
                // Actualizamos las observaciones con el texto introducido
                // En el historico hay que actualizar los movimientos del estado anterior con el estado nuevo.
                histDB.UpdateEstadoHistoricoSolicitud(strIdSolicitud, hdnCodEstadoActual.Value, ddlEstadosSolicitud.SelectedValue, strObservaciones, Usuarios.ObtenerUsuarioLogeado().Login);

                solDB.UpdateObservacionesSolicitud(strIdSolicitud, strObservaciones);
                
                //HistoricoDB hisDB = new HistoricoDB();
                //hisDB.AddHistoricoSolicitud(strIdSolicitud, "002", Usuarios.ObtenerUsuarioLogeado().Login, ddlEstadosSolicitud.SelectedValue, strObservaciones, "ADICO");

                // Actualizamos el estado de la solicitud y el numFactura que haya metido.
                //Le pasamos como ultimo parametro un 0, para que en procedimiento no actualice el valor del campo Tipo Lugar Averia
                solDB.UpdateEstadoSolicitud(strIdSolicitud, ddlEstadosSolicitud.SelectedValue, 0);
                // Guardamos historico del cambio.
                solDB.InsertarSolicitudModificada(strIdSolicitud, (int)Enumerados.TipoRazonModificacionsolicitud.Modificar, usuario, hdnCodEstadoActual.Value);
                
            }
            catch(Exception ex)
            {
            }
        }

        private string ObtenerObservacionesSolicitud(string strIdSolicitud)
        {
            SolicitudesDB solDB = new SolicitudesDB();
            IDataReader dr = null;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = null;
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            try
            {
                dr = solDB.GetSingleSolicitudes(strIdSolicitud,usuarioDTO.IdIdioma);

                if (dr.Read())
                {
                    return DataBaseUtils.GetDataReaderColumnValue(dr, "Observaciones").ToString();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        private void GuardarCambioEstadoVisita(string strIdVisita, string strCodContrato)
        {
            // Recuperamos los datos de la visita
            Visitas visitas = new Visitas();
            VisitaDTO visitaDTO = visitas.DatosVisitas(strCodContrato, strIdVisita);

            // Actualizamos los datos de la visita que queremos que se actualicen en BD.
            visitaDTO.CodigoEstadoVisita = this.ddlEstadosSolicitud.SelectedValue;
            visitaDTO.Observaciones = this.txtObservaciones.Text + " - " + Resources.TextosJavaScript.TEXTO_MOVIMIENTO_CORRECCION + " -";
            //Guardamos el número de factura que haya metido.
            visitaDTO.NumFactura = this.txtNumFactura.Text;   
            

            // Actualizamos el estado de la visita en BD.
            VisitasDB visDB = new VisitasDB();
            visDB.ActualizarDatosVisita(visitaDTO, Usuarios.ObtenerUsuarioLogeado().Login);
            
            // En el historico hay que actualizar los movimientos del estado anterior con el estado nuevo.

            //VisitaHistoricoDB visHistDB = new VisitaHistoricoDB();
            //VisitaHistoricoDTO visHistDTO = new VisitaHistoricoDTO(strCodContrato, 
            //    decimal.Parse(strIdVisita), 
            //    DateTime.Now, 
            //    visitaDTO.CodigoEstadoVisita);

            //visHistDB.SalvarHistorico(visHistDTO, Usuarios.ObtenerUsuarioLogeado().Login);

            HistoricoDB histDB = new HistoricoDB();
            histDB.UpdateEstadoHistoricoVisita(strCodContrato, int.Parse(strIdVisita), hdnCodEstadoVisitaActual.Value, this.ddlEstadosSolicitud.SelectedValue);
        }

        protected void OnBtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    // Si estamos en solicitudes actualizamos la 
                    if (!string.IsNullOrEmpty(this.hdnIdSolicitud.Value))
                    {
                        GuardarCambioEstadoSolicitud(this.hdnIdSolicitud.Value);
                    }
                    else if (!string.IsNullOrEmpty(this.hdnIdVisita.Value))
                    {
                        GuardarCambioEstadoVisita(this.hdnIdVisita.Value, this.hdnCodContrato.Value);
                    }

                    // Tras el cambio de estado volvemos a la página anterior.
                    this.OnBtnVolver_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void dgSolicitudesHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                    e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        protected void OnTxtObservaciones_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtObservaciones.Text))
                {
                    e.IsValid = false;
                    txtObservacionesCustomValidator.ToolTip = "Las observaciones del cambio de estado son obligatorias.";
                }                
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void OnDdlEstadosSolicitud_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (!FormUtils.HasValue(ddlEstadosSolicitud))
                {
                    e.IsValid = false;
                    ddlEstadosSolicitudCustomValidator.ToolTip = "Seleccione el estado nuevo de la solicitud.";
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }



        /// <summary>
        /// OnTxtNumFactura_ServerValidate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnTxtNumFactura_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtNumFactura.Text))
                {
                    e.IsValid = true;
                    txtCustomValidatorNumFactura.ToolTip = "El Numero de Factura esta vacío.";
                }                
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }


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

        protected void OnBtnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSessionData();
                NavigationController.Backward();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBtnEliminarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.ValidatePage())
                //{

                    

                    // Si estamos en solicitudes actualizamos la 
                    if (!string.IsNullOrEmpty(this.hdnIdSolicitud.Value))
                    {
                        EliminarSolicitud(this.hdnIdSolicitud.Value);
                    }
                    else if (!string.IsNullOrEmpty(this.hdnIdVisita.Value))
                    {
                      //Mensaje avisando que se puede Eliminar una solicitud

                    }

                    // Tras el cambio de estado volvemos a la página anterior.
                    this.OnBtnVolver_Click(null, null);
                //}
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }

        }


        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }
}
}
