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
using Iberdrola.Commons.Security;
using System.Reflection;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Validations;

namespace Iberdrola.SMG.UI
{
    public partial class FrmModalAbrirSolicitudProveedor : FrmBase
    {

        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    hdnContrato.Value = Request.QueryString["Contrato"];
                    hdnCodVisita.Value = Request.QueryString["codVisita"];
                    //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    string telefonos = Request.QueryString["telefono"].ToString();
                    string[] sTelefonos = telefonos.Split('/');
                    if (sTelefonos.Length > 1 && sTelefonos[1].Length>0 && sTelefonos[1]!="0")
                    {
                        hdnTelefonoContacto.Value = "+" + sTelefonos[1].Trim();
                    }
                    else if (sTelefonos[0].Length > 0 && sTelefonos[0]!="0")
                    {
                        hdnTelefonoContacto.Value = "+" + sTelefonos[0].Trim();
                    }
                    //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente

                    CargaComboSubtipo();
                    //20200217 BGN BEG R#21724 Permitir al proveedor añadir el teléfono del cliente
                    CargaTelefono();
                    //20200217 BGN END R#21724 Permitir al proveedor añadir el teléfono del cliente
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }

        }

        //20200217 BGN BEG R#21724 Permitir al proveedor añadir el teléfono del cliente
        private void CargaTelefono()
        {
            if (!String.IsNullOrWhiteSpace(hdnTelefonoContacto.Value))
            {
                txtTelefono.Enabled = false;
                txtTelefono.Text = hdnTelefonoContacto.Value;
            }
            else
            {
                txtTelefono.Enabled = true;
                txtTelefono.Text = String.Empty;
            }
        }
        //20200217 BGN END R#21724 Permitir al proveedor añadir el teléfono del cliente

        private void CargaComboSubtipo()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;


            ddlSubtipo.Items.Clear();

            Mantenimiento mantenimiento = new Mantenimiento();
            MantenimientoDTO mantenimientoDTO = mantenimiento.DatosMantenimiento(hdnContrato.Value, usuarioDTO.Pais);
            SolicitudesDB db = new SolicitudesDB();
            DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT * from TIPO_EFV where COD_EFV='" + mantenimientoDTO.CODEFV.ToString() + "' AND (SMGPROTECCIONGAS=1 or SMGPROTECCIONGASINDEPENDIENTE=1)");
            //Boolean esProteccionGas = Boolean.Parse(datos.Tables[0].Rows[0].ItemArray[0].ToString());

            if (datos.Tables[0].Rows.Count == 0)
            {
                TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();

                ddlSubtipo.DataSource = objTipoSolicitudesDB.GetSubtipoSolicitudesPuedeAbrirProveedor(usuarioDTO.IdIdioma);
                ddlSubtipo.DataTextField = "descripcion";
                ddlSubtipo.DataValueField = "codigo";
                ddlSubtipo.DataBind();


                ListItem defaultItem = new ListItem();
                defaultItem.Value = "-1";
                defaultItem.Text = "TODOS";//ObtenerValorPropiedadTextoJavascript("TEXTO_TODOS;//"TODOS";// '"Seleccione un subtipo de solicitud"
                ddlSubtipo.Items.Insert(0, defaultItem);
            
                //2020/01/08 R#13803 Añadir validación para evitar Altas de solicitud de Aceptacion Presupuesto si tiene una ya abierta
                SolicitudesDB objSolicDB = new SolicitudesDB();
                DataSet dsSolicitudes = objSolicDB.GetSolicitudesPorContrato(hdnContrato.Value, usuarioDTO.IdIdioma);
                if (dsSolicitudes != null && dsSolicitudes.Tables.Count > 0 && dsSolicitudes.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsSolicitudes.Tables[0].Rows)
                    {
                        string subTipoSol = dr.ItemArray[4].ToString();
                        string estadoSol = dr.ItemArray[8].ToString();

                        if (subTipoSol == "007" && estadoSol == "001")
                        {
                            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_ACEPTACION_DE_PRESUPUESTO, -1);
                            break;
                        }
                    }
                }
            
                IDataReader drFechaVisitaFechaCierreSolicitud = null;

                VisitasDB objVisitasDB = new VisitasDB();
                drFechaVisitaFechaCierreSolicitud = objVisitasDB.ObtenerFechaVisitaFechaCierreSolicitudAveria(hdnContrato.Value, hdnCodVisita.Value, "");

                DateTime fechaVisita = DateTime.Parse("01/01/1900");
                string estadoSolicitud = "";
                DateTime fechaCierreSolicitud = DateTime.Parse("01/01/1900");
                string estadoVisita = "";
                int categoriaVisita = 1;

                while (drFechaVisitaFechaCierreSolicitud.Read())
                {
                    Boolean mas21 = false;
                    Boolean mas21Larga = false;
                    Boolean mas30 = false;
                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA") != null)
                    {
                        estadoVisita = DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA").ToString();
                    }


                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA") != null)
                    {
                        fechaVisita = DateTime.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA").ToString());
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Estado_solicitud") != null)
                    {
                        estadoSolicitud = DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Estado_solicitud").ToString();
                    }


                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Valor") != null)
                    {
                        fechaCierreSolicitud = DateTime.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "Valor").ToString());
                    }

                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_CATEGORIA_VISITA") != null)
                    {
                        categoriaVisita = int.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_CATEGORIA_VISITA").ToString());
                    }
                    // COMPORBAMOS FECHA VISITA CIERRE PARA PERMITIR O NO AVERIA.
                    if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA") != null)
                    {
                        fechaVisita = DateTime.Parse(DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "FEC_VISITA").ToString());
                    }
                    if (fechaVisita.Year.ToString() != "1900")
                    {
                        if (fechaVisita.AddDays(21) > DateTime.Now)
                        {
                            if (estadoVisita == "02" || estadoVisita == "09")
                            {
                                if (categoriaVisita != 1)
                                {
                                    RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_AVERIA_MANTENIMIENTO, -1);
                                    //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Avería Mantenimiento de Gas"));
                                    mas21Larga = true;
                                }
                                mas21 = true;
                            }
                        }
                    }
                    // COMPROBAMOS SOLICITUD ABIERTA
                    if (estadoSolicitud != "")
                    {
                        if (fechaCierreSolicitud.Year.ToString() != "1900")
                        {
                            if (fechaCierreSolicitud.AddDays(30) > DateTime.Now)
                            {
                                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Avería Mantenimiento de Gas"));
                                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_AVERIA_MANTENIMIENTO, -1);
                                mas30 = true;
                            }
                        }
                    }
                    // SACAMOS MENSAJE.
                    if (mas30)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_ABRIR_AVERIAS_30_DIAS);//"No se pueden abrir averias al tener una avería abierta hace menos de 30 días. \\n EL PROVEEDOR DEBERA DE PONERSE EN CONTACTO CON SU RESPONSABLE.");
                    }
                    else if (mas21 || mas21Larga)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_ABRIR_AVERIAS_21_DIAS);//"No se pueden abrir averias al tener una visita cerrada hace menos de 21 días. \\n EL PROVEEDOR DEBERA DE PONERSE EN CONTACTO CON SU RESPONSABLE.");
                    }
                }
            }
        }

        private void RemoveElement(DropDownList dCombo, string sValor, int iPosicion)
        {
            ////12/06/2015 Administrador puede hacer todo.
            //if (!Usuarios.EsAdministrador(this.CodPerfil))
            //{
                if (sValor != "")
                {
                    dCombo.Items.Remove(dCombo.Items.FindByText(sValor));
                    dCombo.Items.Remove(dCombo.Items.FindByText(sValor + "\r\n"));
                }
                else
                {
                    dCombo.Items.RemoveAt(iPosicion);
                }
            //}
        }

        private void CargaComboTiposAveria()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            ddlAveria.Items.Clear();

            TiposAveriaDB objTiposAveriaDB = new TiposAveriaDB();

            ddlAveria.DataSource = objTiposAveriaDB.GetTiposAveria(usuarioDTO.IdIdioma);
            ddlAveria.DataTextField = "descripcion_averia";
            ddlAveria.DataValueField = "cod_averia";
            ddlAveria.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = Resources.TextosJavaScript.SELECCIONE_TIPO_AVERIA;//;"Seleccione un tipo de averia";
            ddlAveria.Items.Insert(0, defaultItem);
        }

        protected void ddlSubtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAveria.Visible = false;
            lblDescrAveria.Visible = false;
            if (ddlSubtipo.SelectedValue == "001")
            {
                ddlAveria.Visible = true;
                lblDescrAveria.Visible = true;
                CargaComboTiposAveria();
            }
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
                // DAMOS DE ALTA LA SOLICITUD.
                AltaSolicitud();

                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        private void AltaSolicitud()
        {
            try
            {
                SolicitudesDB db = new SolicitudesDB();

                string contrato = hdnContrato.Value;
                string cod_tipo_solicitud = "001";
                string cod_subtipo_solicitud = ddlSubtipo.SelectedValue;
                // PONEMOS POR DEFECTO EL ESTADO INICIAL DE LA AVERIA.
                string cod_estado = "001";
                // TI
                if (ddlSubtipo.SelectedValue == "011") {cod_estado = "035";}
                // ACEPTACION PRESUPUESTO.
                if (ddlSubtipo.SelectedValue == "007") { cod_estado = "001"; }

                string telef_contacto = txtTelefono.Text;
                // OBTENEMOS EL NOMBRE DE LA PERSONA DE CONTACTO.
                string pers_contacto = "";
                DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT top 1 NOM_TITULAR FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" + contrato + "'");
                if (datos.Tables[0].Rows.Count > 0)
                {
                    pers_contacto = datos.Tables[0].Rows[0][0].ToString();
                }
                
                string cod_averia = ddlAveria.SelectedValue;
                string observaciones = ""; //txt_ObservacionesAnteriores.Text & (char)(13) & txt_Observaciones.Text


                if (ddlSubtipo.SelectedValue == "-1")
                {
                    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_SUBTIPO);//"Debe seleccionar un subtipo de solicitud.");
                    //this.ExecuteScript("VerEditar()");
                    return;
                }
                ProveedoresDB objProveedoresDB = new ProveedoresDB();
                string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo(cod_tipo_solicitud, cod_subtipo_solicitud, contrato).Split(';');
                string proveedor = Proveedores[0].ToString().Substring(0, 3); //Mid(hidden_proveedor.Value, 1, 3);


                string usuario = String.Empty;

                telef_contacto = telef_contacto.Replace(" ", "");

                if (!String.IsNullOrEmpty(telef_contacto))
                {
                    //20220218 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    PhoneValidator telefVal = new PhoneValidator();
                    if (!telefVal.Validate(telef_contacto))
                    {
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                        return;
                    }

                    //if (telef_contacto.Length != 12)
                    //{
                    //    txtTelefono.Enabled = true;
                    //    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_TENER_12_DIGITOS);
                    //    return;
                    //}
                    //else
                    //{
                    //    if (telef_contacto.Substring(0, 3) != "+34" && telef_contacto.Substring(0, 4) != "+351")
                    //    {
                    //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                    //        return;
                    //    }
                    //    if (telef_contacto.Substring(3, 1) == "7" || telef_contacto.Substring(3, 1) == "6" || telef_contacto.Substring(3, 1) == "9" || telef_contacto.Substring(3, 1) == "8" || telef_contacto.Substring(3, 1) == "2" || telef_contacto.Substring(3, 1) == "3")
                    //    {
                    //    }
                    //    else
                    //    {
                    //        txtTelefono.Enabled = true;
                    //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_COMENZAR_6789);//"El telefono debe de comenzar por el digito 6, 7, 9 o 8");
                    //        return;
                    //    }
                    //}
                    //20220218 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                }
                else
                {
                    txtTelefono.Enabled = true;
                    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_OBLIGATORIO);
                    return;
                }

                try
                {
                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    usuario = user.Login;
                    string Horas = DateTime.Now.Hour.ToString();
                    if (Horas.Length == 1) Horas = "0" + Horas;
                    string Minutos = DateTime.Now.Minute.ToString();
                    if (Minutos.Length == 1) Minutos = "0" + Minutos;

                    observaciones = "[" + DateTime.Now.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + txtEditarObservaciones.Text + (char)(13);
                }
                catch (Exception ex)
                {

                }

                if (!String.IsNullOrEmpty(contrato) & cod_tipo_solicitud != "-1" & cod_subtipo_solicitud != "-1" & cod_estado != "-1" & !String.IsNullOrEmpty(telef_contacto) & !String.IsNullOrEmpty(pers_contacto) & !String.IsNullOrEmpty(proveedor))
                {
                    if ((cod_subtipo_solicitud == "1" | cod_subtipo_solicitud == "001") & cod_averia == "-1")
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_CODIGO_AVERIA_OBLIGATORIO);// "El código de averia es obligatorio.";
                        return;
                    }

                    SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                    //If cod_averia = -1 Then cod_averia = System.DBNull.Value.ToString


                    int id_solicitud;
                    if ((ddlAveria.Visible))
                    {
                        id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, false,false);
                    }
                    else
                    {
                        id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, "0", observaciones, proveedor, false,false);
                    }
                    
                    //// UPDATE THE NUM SOLICITUDES IF IT IS SMG AMPLIADO OR SMG AMPLIADO INDEPENDIENTE
                    //if (hdnEsSMGAmpliado.Value == "1" || hdnEsSMGAmpliadoIndependiente.Value == "1" || hdnEsSMGAmpliado.Value == "True" || hdnEsSMGAmpliadoIndependiente.Value == "True")
                    //{
                    // WE HAVE A TIGGER THAT DECREASE "1" THE VALUE IF THE ISSUE IS CANCELLED...
                    if (cod_subtipo_solicitud == "001")
                    {
                        ActualizarNumeroAveriasPorcontrato(contrato);
                    }
                  
                    //18/12/2017 BGN Capa Negocio actualizar historico para incluir llamada WS Alta Interaccion
                    //HistoricoDB objHistoricoDB = new HistoricoDB();
                    //objHistoricoDB.AddHistoricoSolicitud(id_solicitud.ToString(), "001", usuario, cod_estado, observaciones, proveedor);
                    Solicitud soli = new Solicitud();
                    soli.ActualizarHistoricoSolicitud(contrato, id_solicitud.ToString(), "001", usuario, cod_estado, observaciones, proveedor, cod_subtipo_solicitud, cod_averia, "0", "A");
                   
                    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + " " + id_solicitud);//"Se ha dado de alta la solicitud " + id_solicitud);

                }
                else
                {
                    MostrarMensaje( Resources.TextosJavaScript.TEXTO_NO_ALTA_FALTAN_CAMPOS_OBLIGATORIOS);//"No se ha dado de alta su solicitud. Todos los campos excepto 'Observaciones' son obligatorios.";
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje( Resources.TextosJavaScript.TEXTO_NO_SE_HA_PODIDO_ALTA_SOLICITUD + ex.Message);// "No se ha podido dar de alta la solicitud. Codigo: " + ex.Message;
            }
            finally
            {

            }
            //----------------------------------------------------------
            //this.ExecuteScript("VerEditar()");
            //Buscar();
        }

        private void ActualizarNumeroAveriasPorcontrato(string contrato)
        {
            SolicitudesDB Db = new SolicitudesDB();
            Db.ActualizarNumeroAveriasPorcontrato(contrato);
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
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
}
}
