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
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Security;
using System.Linq;
using Iberdrola.Commons.Logging;
using Oracle.ManagedDataAccess.Client;
using Iberdrola.Commons.Validations;

namespace Iberdrola.SMG.UI
{
    public partial class frmModalModificarSolicitud : FrmBase
    {
        //public void Item_DataBinding(object sender, System.EventArgs e)
        //{
        //    PlaceHolder ph = (PlaceHolder)sender;

        //}

        private void CrearCaracteristicasPorNombreTipo(DataSet dsCaracteristicas, Boolean habilitado)//string id_solicitud,Boolean habilitado)
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                hdnTotalCaracteristicas.Value = "0";
                tblCaracteristicas.Controls.Clear();
                tblCaracteristicas.Rows.Clear();

                for (int i = 0; i <= dsCaracteristicas.Tables[0].Rows.Count - 1; i++)
                {
                    string nombreTipo = "";// dsCaracteristicas.Tables[0].Rows[i]["CLASE_NET"].ToString();
                    if (habilitado)
                    {
                        nombreTipo = dsCaracteristicas.Tables[0].Rows[i]["CLASE_NET"].ToString();
                    }
                    else
                    {
                        nombreTipo = dsCaracteristicas.Tables[0].Rows[i]["CLASE_NET_DESHABILITADO"].ToString();
                    }
                    string value = dsCaracteristicas.Tables[0].Rows[i]["valor"].ToString();
                    Boolean obligatorio = Boolean.Parse(dsCaracteristicas.Tables[0].Rows[i]["obligatorio"].ToString());
                    string tipCar = dsCaracteristicas.Tables[0].Rows[i]["tip_car"].ToString();

                    TableRow lineaCaracteristica1 = new TableRow();
                    lineaCaracteristica1.ID = "lineaCaracteristica" + tipCar;

                    // *************************** DESCRIPCION CARACTERISTICA ****************************************************************************************
                    // Creamos la celda de la tabla con la descripción de la característica.
                    TableCell ce = new TableCell();
                    ce.Text = dsCaracteristicas.Tables[0].Rows[i]["Descripcion"].ToString();
                    ce.CssClass = "labelFormulario";
                    ce.ID = "labelCaracteristica" + tipCar;
                    lineaCaracteristica1.Cells.Add(ce);

                    // *************************** OBJETO ************************************************************************************************************
                    // Creamos el objeto a meter en la celda de la tabla.
                    ce = new TableCell();
                    Type dynamicType = typeof(WebControl).Assembly.GetType(nombreTipo, true);
                    WebControl dynamicObject = (WebControl)Activator.CreateInstance(dynamicType);
                    dynamicObject.Visible = true;
                    dynamicObject.ID = "Caracteristica" + tipCar;
                    dynamicObject.Width = 300;
                    dynamicObject.EnableViewState = false;
                    dynamicObject.Enabled = habilitado;
                    dynamicObject.Attributes.Add("onChange", "Cargacombo('" + dynamicObject.ID + "', 'Caracteristica" + tipCar + "')");
                    //dynamicObject.Attributes.Add("Value", value);

                    // *************************** VALIDATOR *********************************************************************************************************
                    //// Metemos el validator en caso de ser obligatorio y estar habilitado el campo.
                    ////if (obligatorio && !habilitado)
                    ////{
                    //    RequiredFieldValidator validadorFicheroContratoInspeccion = new RequiredFieldValidator();
                    //    validadorFicheroContratoInspeccion.ErrorMessage = "*";
                    //    validadorFicheroContratoInspeccion.ToolTip = "Campo obligatorio.";
                    //    //validadorNumeroConDosDecimales.runat = "server";
                    //    validadorFicheroContratoInspeccion.CssClass = "errorFormulario";
                    //    validadorFicheroContratoInspeccion.Enabled = false;
                    //    validadorFicheroContratoInspeccion.ID = "FileContratoInspeccion" + i;
                    //    validadorFicheroContratoInspeccion.ControlToValidate = "Caracteristica" + i;
                    //    validadorFicheroContratoInspeccion.Enabled = habilitado;
                    //    ce.Controls.Add(validadorFicheroContratoInspeccion);
                    ////}

                    // *************************** CARGAR COMBO ******************************************************************************************************
                    // Comprobamos si tiene valores que meter en el combo.
                    ConsultasDB consultas = new ConsultasDB();
                    DataTable dtValoresTabla = consultas.ObtenerValoresTablaPorTipoCaracteristica(tipCar, Int16.Parse(usuarioDTO.IdIdioma.ToString()));
                    CargarValorCaracteristicaCombo(dtValoresTabla, dynamicObject);
                    //if (dtValoresTabla.Rows.Count >= 1)
                    //{
                    //    Type t = dynamicObject.GetType();
                    //    PropertyInfo[] props = t.GetProperties();
                    //    if (props[23].ToString() == "System.Object DataSource")
                    //    {
                    //        props[23].SetValue(dynamicObject, dtValoresTabla, null); // DataSource.
                    //        props[11].SetValue(dynamicObject, "DESC_TABLE_VALUE", null);// TextField.

                    //        if (!habilitado)
                    //        {
                    //            props[13].SetValue(dynamicObject, "DESC_TABLE_VALUE", null);// ValueField.
                    //            props[15].SetValue(dynamicObject, value, null);// SelectedValue.
                    //        }
                    //        else
                    //        {
                    //            props[13].SetValue(dynamicObject, "ID_TABLE_COD_TABLE_VALUE", null);// ValueField.
                    //        }
                    //        dynamicObject.DataBind();
                    //        props = null;
                    //        t = null;
                    //    }
                    //}
                    //dtValoresTabla = null;
                    
                    //************************************************************************************************************************************************
                    // Agregamos a la celda el objeto.
                    ce.Controls.Add(dynamicObject);
                    // Agregamos a la tabla la linea con el objeto.
                    lineaCaracteristica1.Cells.Add(ce);
                    // Agregamos a la tabla la linea con la característica.
                    tblCaracteristicas.Rows.Add(lineaCaracteristica1);
                    lineaCaracteristica1 = null;
                    ce = null;
                    dynamicObject = null;
                    dynamicType = null;
                }

                hdnTotalCaracteristicas.Value = (dsCaracteristicas.Tables[0].Rows.Count).ToString();
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_VUELVA_A_SELECCIONAR_LA_SOLICITUD);//"Vuelva a seleccionar la solicitud en la búsqueda");
            }
        }

        public void CargarValorCaracteristicaCombo(DataTable dtDatos, Control dynamicObject)
        {
            if (dtDatos.Rows.Count > 0)
            {
                dynamicObject.EnableViewState=false;
                Type t = dynamicObject.GetType();
                PropertyInfo[] props = t.GetProperties();
                DataRow dRow = dtDatos.NewRow();
                dRow["DESC_TABLE_VALUE"] = "";
                dRow["ID_TABLE_COD_TABLE_VALUE"] = "";
                dtDatos.Rows.Add(dRow);
                var query2 = dtDatos.AsEnumerable().OrderBy(c=> c.Field<string>("DESC_TABLE_VALUE")); 
                DataView dv2   = query2.AsDataView();

                // Si es KO solo permitimos pago anticipado.
                //if (hdnSolCent.Value == "KO")
                //    ññ
                if (props[23].ToString() == "System.Object DataSource")
                {
                    props[23].SetValue(dynamicObject, dv2, null); // DataSource.
                    props[11].SetValue(dynamicObject, "DESC_TABLE_VALUE", null);// TextField.
                    props[13].SetValue(dynamicObject, "ID_TABLE_COD_TABLE_VALUE", null);// ValueField.
                    dynamicObject.DataBind();
                }
            }
        }

        public void CambioComboCaracteristicaPorNombre_SelectedIndexChanged(string ID, string selectedValue)
        {
            try
            {
             //   return;
                //hdnCaracteristicaCalderasSeleccionado.Value
                string[] sNumero = ID.Split('-');
                string sNumCaracteristica = sNumero[0].ToString().Substring(14);
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                int IdIdioma = usuarioDTO.IdIdioma;
                ConsultasDB consultas = new ConsultasDB();
                string[] matrizSelectedValue = selectedValue.Split('-');
                string idTable = matrizSelectedValue[0].ToString();
                string codTable = matrizSelectedValue[1].ToString();

                //if (idTable != "1")
                //{
                    //    // Cargamos combo Origen.
                    //    //Control dynamicObjectOrigen = FindControlById(tblCaracteristicas, ID);



                    DataTable datosComboOrigen = consultas.ObtenerValoresActualesYAnterioresPTableValuePorIdTableYCodTable(idTable, codTable, IdIdioma, sNumCaracteristica, ddl_EstadoSol.SelectedValue, hdnSubtipo.Value);

                    for (int conteo = 0; conteo < datosComboOrigen.Rows.Count; conteo++)
                    {
                        if(!String.IsNullOrEmpty(datosComboOrigen.Rows[conteo]["ID_CARACTERISTICA"].ToString()))
                        {
                            string idCaracteristica1 = datosComboOrigen.Rows[conteo]["ID_CARACTERISTICA"].ToString();
                            string idObjeto1 = "Caracteristica" + idCaracteristica1;
                            Control dynamicObject = FindControlById(tblCaracteristicas, idObjeto1);

                            DataTable selectedTable = datosComboOrigen.AsEnumerable()
                                .Where(r => r.Field<string>("ID_CARACTERISTICA") == idCaracteristica1)
                                .CopyToDataTable();

                            CargarValorCaracteristicaCombo(selectedTable, dynamicObject);
                        }
                    }

                    // Seleccionamos valor combo origen.
                    //Type tOrigen = dynamicObjectOrigen.GetType();
                    //PropertyInfo[] propsOrigen = tOrigen.GetProperties();
                    //propsOrigen[15].SetValue(dynamicObjectOrigen, selectedValue, null);// SelectedValue.
                //}
                //else
                //{

                //    //// Cargamos valores combo nuevo con los datos relacionados con el valor del combo origen. 
                //    DataTable datosActuales = consultas.ObtenerValoresPTableValuePorParentIdTableYParentCodTable(idTable, codTable, IdIdioma);
                //    string idCaracteristica = datosActuales.Rows[1]["ID_CARACTERISTICA"].ToString();
                //    string idObjeto = "Caracteristica" + idCaracteristica;
                //    Control dynamicObject1 = FindControlById(tblCaracteristicas, idObjeto);


                //    CargarValorCaracteristicaCombo(datosActuales, dynamicObject1);
                //}
                
            }
            catch (Exception ex)
            {
                // the object is not a dropdownlist.
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //CrearCaracteristicasPorNombreTipo(Request.QueryString["idSolicitud"],false);
                //return;
                //Form.Enctype = "multipart/form-data";
                //Form.EnableViewState=true;
                ////ViewStateMode="Enabled"
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                var controlName = Request.Params.Get("__EVENTTARGET");
                var argument = Request.Params.Get("__EVENTARGUMENT");

                if (controlName != null)
                {
                    if (controlName.ToString().IndexOf("Caracteristica") >= 0)
                    {
                        string cod_estado = ddl_EstadoSol.SelectedValue;

                        //SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                        //DataSet datosActuales = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE FROM P_TABLE_VALUE WHERE DESC_TABLE_VALUE='" + argument + "'");
                        //if (datosActuales.Tables[0].Rows.Count > 0)
                        //{
                        //    string idTable = datosActuales.Tables[0].Rows[0].ItemArray[0].ToString();
                        //    string codTableValue = datosActuales.Tables[0].Rows[0].ItemArray[1].ToString();
                        //    hdnCaracteristicaCalderasSeleccionado.Value = idTable + "_" + codTableValue;
                        //}

                        // Comprobamos si es un alta de avería de gas confort o no, para volver a cargar las características
                        //if (hdnAltaIncidenciaGasConfort.Value == "0")
                        //{

                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                        if (hdnSubtipo.Value != "012" && hdnSubtipo.Value != "013" && hdnSubtipo.Value != "015")
                        {
                            CambioComboCaracteristica_SelectedIndexChanged(controlName, argument);
                        }
                        //}
                        //else
                        //{
                        //    CargaCaracteristicasPorTipoSolicitud("001", "012", "065");
                        //}
                    }
                }
                if (!IsPostBack)
                {
                    hdnMostrarDatosBancarios.Value = "true";
                    hdnMostrarDiaPago.Value = "true";
                    hdnVisibleUnidades1.Value = "1";
                    hdnVisibleUnidades2.Value = "1";
                    hdnCodigoOperacion.Value = "0";
                    hdnCampania.Value = "";
                    //hdnMostrarFornecimientoGas.Value = "true";
                    string idSolicitud = Request.QueryString["idSolicitud"];
                    string codContrato = Request.QueryString["Contrato"];
                    string esCaldera = Request.QueryString["Calderas"];
                    string esInspeccion = Request.QueryString["Inspeccion"];

                    hdnCodContrato.Value = codContrato;
                    hdnSubtipoAveriaSobreGC.Value = Request.QueryString["SubtipoAveriaSobreGC"];

                    string cod_estado = ddl_EstadoSol.SelectedValue;
                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    int idePerfil = 0;

                    // 18/02/2016 Cargamos el telefono de contacto del cliente para solicitudes de avería de GC.
                    //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente                    
                    if (!string.IsNullOrEmpty(Request.QueryString["Telefono"]))
                    {
                        string telefonos = Request.QueryString["Telefono"].ToString();
                        string[] sTelefonos = telefonos.Split('/');
                        if (sTelefonos.Length > 1 && sTelefonos[1] != "0")
                        {
                            txtTelefono.Text = "+" + sTelefonos[1].Trim();
                        }
                        else
                        {
                            txtTelefono.Text = "+" + sTelefonos[0].Trim();
                        }
                    }
                    //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente

                    if (string.IsNullOrEmpty(idSolicitud))
                    {

                        hdnAltaIncidenciaGasConfort.Value = "1";
                        //Cargamos las características para el alta de avería de gas confort.
                        CargaCaracteristicasPorTipoSolicitud("001", "012", "065");
                        CargaComboEstadosFuturos("001", "012", "065", "Pendiente");

                        //prueba
                        idSolicitud = "0";


                    }


                    if (user.Id_Perfil.HasValue) { idePerfil = int.Parse(user.Id_Perfil.ToString()); }

                    if (Usuarios.EsAdministrador(idePerfil))
                    {
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();

                        lblProveedor.Visible = true;
                        cmbProveedor.Visible = true;
                        //10/02/2016 No poder modificar proveedor a petición de Edurne, mail de hoy 10:30h +-.
                        //lblProveedor.Visible = false;
                        //cmbProveedor.Visible = false;
                        UsuarioDB usuarioDB = new UsuarioDB();
                        this.cmbProveedor.DataSource = usuarioDB.ObtenerTodosProveedoresEnSolicitudes();
                        cmbProveedor.DataTextField = "PROVEEDOR";
                        cmbProveedor.DataValueField = "PROVEEDOR";
                        this.cmbProveedor.DataBind();

                        if (!string.IsNullOrEmpty(idSolicitud) && !string.IsNullOrEmpty(codContrato))
                        {
                            //cmbProveedor.SelectedValue = objSolicitudesDB.ObtenerProveedorUltimoMovimientoSolicitud(int.Parse(idSolicitud));
                            ConsultasDB consultasDB= new ConsultasDB();
                            string Proveedor = consultasDB.ObtenerProveedorAveriaPorContrato(codContrato);
                            cmbProveedor.SelectedValue = Proveedor.Substring(0, 3);
                        }
                    }
                    hdnVieneDeAltaCalderas.Value = esCaldera;
                    hdnContrato.Value = codContrato;
                    //06/09/2013 Twitter subject.
                    imgTwitter.Visible = false;
                    //10/03/2015 Facebook subject.
                    imgFacebook.Visible = false;


                    if (user.Id_Perfil.HasValue) { idePerfil = int.Parse(user.Id_Perfil.ToString()); }
                    if (Usuarios.EsTelefono(idePerfil) || Usuarios.EsAdministrador(idePerfil))
                    {
                        imgTwitter.Visible = true;
                        imgFacebook.Visible = true;
                    }


                    if (esInspeccion == "1")
                    {
                        DatosCliente(idSolicitud, codContrato, false);
                        VisitasDB objVisitasDB = new VisitasDB();
                        // Kintell R#25236
                        // No dejar abrir si tiene ya una abierta.
                        Int32 otraInspeccionAbierta = objVisitasDB.comprobarSiTieneOtraInspeccionAbierta(codContrato);
                        if (otraInspeccionAbierta > 0)
                        {
                            MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_INSPECCION_YA_ABIERTA);

                            MasterPageModal mpm = (MasterPageModal)this.Master;
                            string script = "<script>window.parent.refrescar();</script>";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "REFRESCAR", script, false);
                            mpm.CerrarVentanaModal();
                            return;
                        }

                        // (20-04-2018) MAQ - Inspección Gas.
                        // Si tiene otra solicitud del tipon Inspección Gas en los últimos 5 meses.
                        Int64 numeroSolicitudes = objVisitasDB.ComprobarSisolicitudInspeccionGasAbiertaEnLosUltimosCincoAños(codContrato);
                        // Kintell/Bea R#24886
                        Boolean pagoInspeccion = objVisitasDB.comprobarSiTieneQuePagarInspeccion(codContrato);

                        // Kintel/Bea R#30097
                        string campania = string.Empty;
                        ConfiguracionDTO configHabilitar = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_BUSQUEDA_CAMPANIA_SIDAT);
                        if (configHabilitar != null && !string.IsNullOrEmpty(configHabilitar.Valor) && Convert.ToBoolean(configHabilitar.Valor) == true)
                        {
                            campania = OtenerCampania(codContrato);
                            if (!String.IsNullOrEmpty(campania))
                            {
                                hdnCampania.Value = campania;
                            }
                        }

                        if (numeroSolicitudes > 0)
                        {
                            hdnInspeccionExtra.Value = "1";
                            if (String.IsNullOrEmpty(campania))
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_AVISO_ABRIR_INSPECCION_GAS);
                            }
                            else
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_AVISO_ABRIR_INSPECCION_GAS_DESCUENTO);
                            }
                        }
                        else if (pagoInspeccion)
                        {
                            hdnInspeccionExtra.Value = "1";
                            if (String.IsNullOrEmpty(campania))
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS);
                            }
                            else
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS_DESCUENTO);
                            }
                        }
                        else
                        {
                            hdnInspeccionExtra.Value = "0";
                        }

                        ////Cuando se cree una solicitud para contratos MGI, primero se mira si es una solicitud a cobrar, luego se pregunta al CDC si esta en la campaña X, y siempre y cuando no tenga otra solicitud en esa campaña se sacara un mensaje y se marcará la solcitud como NO a cobrar.
                        ////Kintell 29/01/2021 R#29209
                        //SolicitudesDB db = new SolicitudesDB();
                        ////DataSet datos;
                        ////Boolean esMGI;
                        ////Boolean esMGIIndependiente;
                        ////if (lblServicioInfo.Text!="")
                        ////{

                        ////}
                        //DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT SMGPROTECCIONGAS,SMGPORTUGALMANTENIMIENTO,SMGPORTUGALMANTENIMIENTOINDEPENDIENTE,COD_COBERTURA from TIPO_EFV where descripcion_efv='" + lblServicioInfo.Text + "'");
                        //Boolean esMGI = Boolean.Parse(datos.Tables[0].Rows[0].ItemArray[1].ToString());
                        //Boolean esMGIIndependiente = Boolean.Parse(datos.Tables[0].Rows[0].ItemArray[2].ToString());

                        //if (esMGI || esMGIIndependiente)
                        //{
                        //    ConfiguracionDTO configHabilitar = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_BUSQUEDA_CAMPANIA_SIDAT);
                        //    if (configHabilitar != null && !string.IsNullOrEmpty(configHabilitar.Valor) && Convert.ToBoolean(configHabilitar.Valor) == true)
                        //    {
                        //        string campania = OtenerCampania(codContrato);
                        //        if (!String.IsNullOrEmpty(campania))
                        //        {
                        //            hdnCampania.Value = campania;
                        //            Boolean solicitudesPreviasConCampania = objVisitasDB.comprobarSiTieneSolicitudesAnterioresParaLaCampania(codContrato, campania);

                        //            if (numeroSolicitudes > 0 && !solicitudesPreviasConCampania)
                        //            {
                        //                hdnInspeccionExtra.Value = "0";
                        //                MostrarMensaje(Resources.TextosJavaScript.TEXTO_INSPECCION_NOCOBRAR_CAMPAÑA);
                        //            }
                        //            else
                        //            {
                        //                if (numeroSolicitudes > 0 && solicitudesPreviasConCampania)
                        //                {
                        //                    hdnInspeccionExtra.Value = "1";
                        //                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS);
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (numeroSolicitudes > 0)
                        //            {
                        //                hdnInspeccionExtra.Value = "1";
                        //                MostrarMensaje(Resources.TextosJavaScript.TEXTO_AVISO_ABRIR_INSPECCION_GAS);
                        //            }

                        //            if (pagoInspeccion)
                        //            {
                        //                hdnInspeccionExtra.Value = "1";
                        //                MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (numeroSolicitudes > 0)
                        //        {
                        //            hdnInspeccionExtra.Value = "1";
                        //            MostrarMensaje(Resources.TextosJavaScript.TEXTO_AVISO_ABRIR_INSPECCION_GAS);
                        //        }

                        //        if (pagoInspeccion)
                        //        {
                        //            hdnInspeccionExtra.Value = "1";
                        //            MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS);
                        //        }
                        //    }

                        //}
                        //else
                        //{
                        //    if (numeroSolicitudes > 0)
                        //    {
                        //        hdnInspeccionExtra.Value = "1";
                        //        MostrarMensaje(Resources.TextosJavaScript.TEXTO_AVISO_ABRIR_INSPECCION_GAS);
                        //    }

                        //    if (pagoInspeccion)
                        //    {
                        //        hdnInspeccionExtra.Value = "1";
                        //        MostrarMensaje(Resources.TextosJavaScript.TEXTO_COBRAR_INSPECCION_GAS);
                        //    }
                        //}

                        hdnSubtipo.Value = "016";
                        cod_estado = "069";
                        CargaCaracteristicasPorTipoSolicitud("001", "016", cod_estado);
                        //DatosCliente(idSolicitud, codContrato, true);

                        hdnAltaIncidenciaGasConfort.Value = "0";
                        hdnInspeccion.Value = "1";
                        
                        CargaComboEstadosFuturos("001", "016", "069", "Pendiente");
                        //ddl_EstadoSol.SelectedIndex = 1;

                        //prueba
                        idSolicitud = "0";
                    }
                    else
                    {
                        hdnInspeccion.Value = "0";
                        if (esCaldera == "1")
                        {
                            cod_estado = "034";
                            CargaCaracteristicasPorTipoSolicitud("001", "008", cod_estado);
                            DatosCliente(idSolicitud, codContrato, true);
                        }
                        else 
                        { 
                            DatosCliente(idSolicitud, codContrato, false); 
                        }
                    }
                    //if (hdnSubtipo.Value == "008")
                    //{
                    //    txtTelefono.Visible = false;
                    //}
                    //else
                    //{
                    //    txtTelefono.Visible = true;
                    //}

                    //idSolicitud

                    CargarHistoricoCaracteristicas(idSolicitud);

                    //#2094:Pago Anticipado
                    CargaComboLugarAveria();


                    MostrarAvisoGeneradaTrasLlamadaCortesia(idSolicitud);

                    // 19/08/2015 Nuevo subtipo averia gas confort, solo puede dar de alta el proveedor.
                    btnAltaAveriaCaldera.Visible = false;
                    btnCodificacionAverias.Visible = false;
                    if (hdnSubtipo.Value == "012" || hdnIdSolicitud.Value == "0")
                    {
                        //                        btnAltaAveriaCaldera.Visible = true;
                        btnCodificacionAverias.Visible = true;
                    }

                    // 20-04-2018 MAQ - Inspección gas, si es MGI puede ver las de PIG, pero no modificarlas.
                    if (!string.IsNullOrEmpty(idSolicitud))
                    {
                        //cmbProveedor.SelectedValue = objSolicitudesDB.ObtenerProveedorUltimoMovimientoSolicitud(int.Parse(idSolicitud));
                        

                        if(hdnSubtipo.Value == "016")
                        {
                            ConsultasDB consultasDB = new ConsultasDB();
                            string Proveedor = consultasDB.ObtenerProveedorInspeccionGasPorContrato(codContrato);
                            // Bea/Kintell (30/06/2020) .- Modificar el spSMGObtenerProveedorInspeccionGasPorContrato para que devuelva el proveedor bueno, hemos agregado lo de que no sea teléfono.
                            if (user.CodProveedor != Proveedor.Substring(0, 3) && !Usuarios.EsAdministrador(idePerfil) && !Usuarios.EsTelefono(idePerfil))
                            {
                                ddl_EstadoSol.Enabled = false; 
                                btnAceptar.Enabled = false;
                            }
                        }
                    }

                    //20200528 BGN BEG R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
                    try
                    {
                        bool permisoGestionDocumental = false;
                        string expediente = usuarioDTO.Login;
                        ConfiguracionDTO confExpedientesGestionDoc = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.EXPEDIENTES_GESTION_DOCUMENTAL);
                        if (confExpedientesGestionDoc != null && !string.IsNullOrEmpty(confExpedientesGestionDoc.Valor))
                        {
                            if (confExpedientesGestionDoc.Valor.Equals("TODOS"))
                            {
                                permisoGestionDocumental = true;
                            }
                            else
                            {
                                string[] expedientes = confExpedientesGestionDoc.Valor.Split(';');
                                if (expedientes.Contains(expediente))
                                    permisoGestionDocumental = true;
                            }                            
                        }
                        if (permisoGestionDocumental)
                            btnGestionDocumental.Visible = true;
                        else
                            btnGestionDocumental.Visible = false;

                    }
                    catch (Exception ex2)
                    {
                        btnGestionDocumental.Visible = false;
                    }
                    //20200528 BGN END R#22132 Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }

        private string OtenerCampania(string codContrato)
        {
            string campania = "";
            OracleConnection Conectar = new OracleConnection();
            OracleDataAdapter da = new OracleDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                ConfiguracionDTO confConexion = Configuracion.ObtenerConfiguracion(Enumerados.ConexionesYRutas.SIDAT_CADENA_CONEXION);
                ConfiguracionDTO querySidat = Configuracion.ObtenerConfiguracion(Enumerados.ConexionesYRutas.SIDAT_QUERY_CAMPANIA);

                if (confConexion != null && !String.IsNullOrEmpty(confConexion.Valor))
                {
                    string strQuery = "";

                    strQuery = querySidat.Valor;
                    strQuery = strQuery.Replace("@CONTRATO", codContrato);

                    Conectar.ConnectionString = confConexion.Valor;
                    //Abrimos la conexion y comprobamos que no hay error.
                    Conectar.Open();
                    OracleCommand comando = new OracleCommand(strQuery, Conectar);
                    comando.Connection = Conectar;

                    da = new OracleDataAdapter(strQuery, Conectar);
                    da.Fill(ds, "CampaniaSidat");

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        campania = ds.Tables[0].Rows[i].Field<string>("COD_PDT_PTLAR");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
            finally
            {
                ds.Clear();
                Conectar.Close();
            }

            return campania;
        }

        private void MostrarAvisoGeneradaTrasLlamadaCortesia(string idSolicitud)
        {
            IDataReader dr = null;
            bool bMostrarAviso = false;

            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = null;
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                SolicitudesDB solDB = new SolicitudesDB();
                dr = solDB.GetSingleSolicitudes(idSolicitud,usuarioDTO.IdIdioma);

                if (dr.Read())
                {
                    string strObservaciones = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "Observaciones");

                    if (!string.IsNullOrEmpty(strObservaciones))
                    {
                        bMostrarAviso = strObservaciones.IndexOf(Resources.TextosJavaScript.TEXTO_GENERADA_TRAS_LLAMADA_CORTESIA) > -1;
                    }
                }

                this.imgLlamadaCortesia.Visible = bMostrarAviso;
            }
            catch
            {
                this.imgLlamadaCortesia.Visible = false;
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

        private void CargarHistoricoCaracteristicas(string idSolicitud)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            // Si tenemos el id solicitud cargamos el histórico de las características
            if (!string.IsNullOrEmpty(idSolicitud))
            {
                // Miramos el perfil del usuario para ver si hay que mostrarle o no el 
                // histórico de las caracterísitcas.

                // TODO: Pendiente de ver si sólo lo tiene que ver el administrador
                // O lo pueden ver todos los perfiles
                dgHistorico.DataSource = CaracteristicaHistorico.GetCaracteristicaHistoricoSolicitud(idSolicitud,(Int16)usuarioDTO.IdIdioma);
                dgHistorico.DataBind();

                this.btnMostrarHistoricoCaracteristicas.Visible = true;
            }
            else
            {
                // Si no tenemos el histórico no cargamos nada.
                // Reseteamos lo que pudiera haber el el grid
                dgHistorico.DataSource = null;
                dgHistorico.DataBind();

                // Ocultamos el botón de mostrar las características
                this.btnMostrarHistoricoCaracteristicas.Visible = false;
            }
        }

        protected void DatosCliente(string IdSolicitud, string codContrato, Boolean vieneDeCalderas)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            Mantenimiento mantenimiento = new Mantenimiento();
            hdnIdSolicitud.Value = IdSolicitud;
            if (String.IsNullOrEmpty(codContrato))
            {
                return;
            }

            // There we 'write' the information of the client, (downside the page).
            MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();
            UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
            if (Usuarios.EsAdministrador((int)user.Id_Perfil))
            {
                mantenimientoDTO = mantenimiento.DatosMantenimientoSinPais(codContrato);
            }
            else
            {
                mantenimientoDTO = mantenimiento.DatosMantenimiento(codContrato, usuarioDTO.Pais);
            }
            
            //mantenimientoDTO = mantenimiento.DatosMantenimiento(codContrato, usuarioDTO.Pais);

            try
            {
                //Reader.Read();
                string Nombre = mantenimientoDTO.NOM_TITULAR.ToString() + " " + mantenimientoDTO.APELLIDO1.ToString() + " " + mantenimientoDTO.APELLIDO2.ToString();
                lblClienteInfo.Text = Nombre;
                string Suministro = mantenimientoDTO.TIP_VIA_PUBLICA.ToString() + " " + mantenimientoDTO.NOM_CALLE.ToString() + " " + mantenimientoDTO.COD_PORTAL.ToString() + ", " + mantenimientoDTO.TIP_PISO.ToString() + " " + mantenimientoDTO.TIP_MANO.ToString() + ", " + mantenimientoDTO.COD_POSTAL.ToString() + ", " + mantenimientoDTO.COD_POBLACION.ToString() + ", " + mantenimientoDTO.COD_PROVINCIA.ToString();
                lblSuministroInfo.Text = Suministro;

                string servicio;
                string T1;
                string T2;
                string T5;


                if (mantenimientoDTO.T1.ToString() == "S")
                {
                    T1 = "Servicio Mantenimiento Gas Calefacción";
                }
                else
                {
                    T1 = "";
                }

                if (mantenimientoDTO.T2.ToString() == "S")
                {
                    T2 = "Servicio Mantenimiento Gas";
                }
                else
                {
                    T2 = "";
                }

                if (mantenimientoDTO.T5.ToString() == "S")
                {
                    T5 = "con pago fraccionado";
                }
                else
                {
                    T5 = "";
                }

                servicio = T1 + T2 + " " + T5;

                lblServicioInfo.ForeColor = Color.Black;

                // Changed for calderas gas confort.
                if (String.IsNullOrEmpty(mantenimientoDTO.DESEFV))
                {
                    servicio = "";
                }
                else
                {
                    servicio = mantenimientoDTO.DESEFV.ToString();
                    SolicitudesDB db = new SolicitudesDB();
                    DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT gasconfort from TIPO_EFV where descripcion_efv='" + servicio + "'");
                    Boolean esGasConfort = Boolean.Parse(datos.Tables[0].Rows[0].ItemArray[0].ToString());

                    lblServicioInfo.ForeColor = Color.Black;
                }

                lblServicioInfo.Text = servicio;
                //lblServicioInfo.Font.Bold = true;

                hdnSolCent.Value = mantenimientoDTO.SOLCENT.ToString();

                //if (!String.IsNullOrEmpty(mantenimientoDTO.OBSERVACIONES)) { lblObservacionesInfo.Text = mantenimientoDTO.OBSERVACIONES.ToString(); }
            }
            catch (Exception err)
            {
                // 19/07/2016 Tema Idioma.
                //MostrarMensaje("CONTRATO NO ENCONTRADO (Al cargar datos del cliente)");
                MostrarMensaje(Resources.TextosJavaScript.CONTRATO_NO_ENCONTRADO);
                //InicializarControles();
            }


            // There we 'write' the issue´s information.
            SolicitudesDB objSolicitudesDB = new SolicitudesDB();
            SqlDataReader dr = null;
            //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            //UsuarioDTO usuarioDTO = null;
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            dr = objSolicitudesDB.GetSingleSolicitudes(IdSolicitud,usuarioDTO.IdIdioma);
            try
            {
                while (dr.Read())
                {
                    txtEstadoActual.Text = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Des_Estado_solicitud");
                    txtEstadoActual.Enabled = false;
                    txt_ObservacionesAteriores.Text = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Observaciones");
                    txt_ObservacionesAteriores.Enabled = false;
                    //(String)DataBaseUtils.GetDataReaderColumnValue(dr, "Urgencia");
                    //(String)DataBaseUtils.GetDataReaderColumnValue(dr, "Id_Solicitud");
                    if (DataBaseUtils.GetDataReaderColumnValue(dr, "telefono_contacto") != null)
                    {
                        txtTelefono.Text = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "telefono_contacto");
                    }
                    else
                    {
                        txtTelefono.Text = "";
                    }
                    // Load the futures states.
                    string subtipo = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "subtipo_solicitud");
                    string estado = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Estado_solicitud");

                    //if(subtipo=="008" && estado=="045")
                    //{
                    //    estado = "034";
                    //    txtEstadoActual.Text = "Pdte. Scoring";
                    //    ValidaComboMotivoCancelacion("008");
                    //    CargaCaracteristicasPorTipoSolicitud("001", "008", estado);

                    //}

                    CargaComboEstadosFuturos("001", subtipo, estado, txtEstadoActual.Text);
                    hdnSubtipo.Value = subtipo;
                    hdnEstadoInicialSolicitud.Value = estado;

                    // Load the 'HORARIO CONTACTO'.
                    DataSet datos = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT top 1 HORARIO_CONTACTO FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" + codContrato + "'");

                    if (datos.Tables[0].Rows.Count > 0)
                    {
                        cmbHorarioContacto.SelectedValue = datos.Tables[0].Rows[0].ItemArray[0].ToString();
                    }

                    // Calderas topic.
                    CargaComboMotivosCancelacion(subtipo);

                    string motCancel = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "Mot_cancel");
                    if (ddl_MotivoCancel.SelectedItem != null)
                    {
                        ddl_MotivoCancel.SelectedItem.Selected = false;
                    }
                    try
                    {
                        ddl_MotivoCancel.Items.FindByValue(motCancel).Selected = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    ValidaComboMotivoCancelacion(estado);

                    // Calderas topic.
                    CalderasDB objCalderasDB = new CalderasDB();
                    DataSet ds = objCalderasDB.GetCalderasPorContrato(codContrato);
                    hdnMarcaCaldera.Value = "";
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        this.txt_ModeloCaldera.Text = ds.Tables[0].Rows[0][4].ToString();
                        this.txt_MarcaCaldera.Text = ds.Tables[0].Rows[0][3].ToString();
                        txt_MarcaCaldera.Enabled = false;
                        hdnMarcaCaldera.Value = ds.Tables[0].Rows[0][2].ToString();
                        hdnDesTipoCaldera.Value = ds.Tables[0].Rows[0][8].ToString();
                        //txt_ModeloCaldera.Enabled = false;
                    }

                    // 16/12/2015
                    // Coger la caldera de los datos informados en la solicitud, no de la tabla caldera
                    CalderaDB objCalderaDBenC = new CalderaDB();
                    DataTable dt = objCalderaDBenC.GetTipologiaCalderaPorIdSolicitud(IdSolicitud);

                    if (dt.Rows.Count != 0)
                    {

                        hdnTipologiaCalderaGC.Value = dt.Rows[0][0].ToString();
                    }


                    if (!vieneDeCalderas)
                    {
                        CargaCaracteristicasSolicitud(IdSolicitud, subtipo);
                    }


                    // 16/12/2014 Twitter
                    Boolean esTwitter = Boolean.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ES_TWITTER").ToString());
                    if (esTwitter)
                    {
                        lblTwitter.Visible = true;
                        this.ExecuteScript("document.getElementById('ctl00_ContentPlaceHolderContenido_lblTwitter').style.visibility = 'visible';");
                    }
                    else
                    {
                        lblTwitter.Visible = false;
                        // this.ExecuteScript("document.getElementById('ctl00_ContentPlaceHolderContenido_lblTwitter').style.visibility = 'hidden';");
                    }
                    // 10/03/2015 Facebook
                    Boolean esFacebook = Boolean.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "ES_FACEBOOK").ToString());
                    if (esFacebook)
                    {
                        lblFacebook.Visible = true;
                        this.ExecuteScript("document.getElementById('ctl00_ContentPlaceHolderContenido_lblFacebook').style.visibility = 'visible';");
                    }
                    else
                    {
                        lblFacebook.Visible = false;
                        // this.ExecuteScript("document.getElementById('ctl00_ContentPlaceHolderContenido_lblTwitter').style.visibility = 'hidden';");
                    }

                }
                dr.Close();

            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_ERROR_AL_OBTENER_DATOS_SOLICITUD_ACTUAL);//"Error al obtener datos de la solicitud actual");
            }

        }

        private void CrearCaracteristicas(DataSet dsCaracteristicas, Boolean Habilitado)
        {
            //CrearCaracteristicasPorNombreTipo(dsCaracteristicas, Habilitado);
            //return;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            Boolean esHoneywell = false;
            int IdIdioma = usuarioDTO.IdIdioma;
            // (20/08/2015) Tema de averia de gas confort
            var argument = Request.Params.Get("__EVENTARGUMENT");
            //***********************************************************************
            hdnTotalCaracteristicas.Value = "0";
            tblCaracteristicas.Rows.Clear();

            string idCaracteristicaPadre = "";
            SolicitudesDB objSolicitudesDB = new SolicitudesDB();

            // 14 digitos 18/03/2015
            RegularExpressionValidator validadorNumeroCaracteres = new RegularExpressionValidator();
            validadorNumeroCaracteres.ValidationExpression = @"\d{14}";
            validadorNumeroCaracteres.ErrorMessage = "*";
            validadorNumeroCaracteres.ToolTip = "Código de Barras debe tener 14 dígitos";
            //validadorNumeroCaracteres.runat = "server";
            validadorNumeroCaracteres.CssClass = "errorFormulario";
            validadorNumeroCaracteres.Enabled = false;
            Boolean Cargar = true;


            // 16/12/2015 
            // alfanumerico 7 digitos que empiece por g
            RegularExpressionValidator validadorNumeroCaracteresNumeroSerieTermostatoInteligente = new RegularExpressionValidator();
            validadorNumeroCaracteresNumeroSerieTermostatoInteligente.ValidationExpression = @"\G[0-9a-zA-Z]{6}";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligente.ErrorMessage = "*";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligente.ToolTip = "El número de serie debe de ser de 7 dígitos y empezar por la letra G.";
            //validadorNumeroCaracteres.runat = "server";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligente.CssClass = "errorFormulario";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligente.Enabled = false;

            // 11/04/2018
            // alfanumerico 12
            RegularExpressionValidator validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell = new RegularExpressionValidator();
            validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.ValidationExpression = @"[0-9a-zA-Z]{12}";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.ErrorMessage = "*";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.ToolTip = "El número de serie debe de ser alfanumérico de 12 caracteres.";
            //validadorNumeroCaracteres.runat = "server";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.CssClass = "errorFormulario";
            validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.Enabled = false;

            // 17/02/2016 
            RegularExpressionValidator validadorNumeroConDosDecimales = new RegularExpressionValidator();
            validadorNumeroConDosDecimales.ValidationExpression = @"([0-9])+([\.\,]\d{2})?";
            validadorNumeroConDosDecimales.ErrorMessage = "*";
            validadorNumeroConDosDecimales.ToolTip = "El valor tiene que ser numérico con 2 decimales.";
            //validadorNumeroConDosDecimales.runat = "server";
            validadorNumeroConDosDecimales.CssClass = "errorFormulario";
            validadorNumeroConDosDecimales.Enabled = false;

            //15/12/2017 Validacion calentador Chaffoteaux
            /*
             •	Tienen que ser un total de 21 caracteres alfanuméricos
             •	7 primeros dígitos tienen que ser numéricos y empiezan por 3
             •	Los dígitos 8 y 9 deben ser siempre WU
             •	Los dígitos 10 y 11 corresponden al año de fabricación por lo que tienen que ser numéricos
             •	Los 12, 13, 14 son el día del año de fabricación así que también tienen que ser numéricos
             •	Los que faltan hasta 21, es el número de identificación del equipo
            */
            RegularExpressionValidator validadorNumEquipoCalentadorChaffoteaux = new RegularExpressionValidator();
            validadorNumEquipoCalentadorChaffoteaux.ValidationExpression = @"3[0-9]{6}WU[0-9]{12}";
            validadorNumEquipoCalentadorChaffoteaux.ErrorMessage = "*";
            validadorNumEquipoCalentadorChaffoteaux.ToolTip = "El número de equipo debe de ser de 21 caracteres, empezar por 3 seguido de 6 dígitos más, seguido de 'WU' y 12 digitos más.";
            validadorNumEquipoCalentadorChaffoteaux.CssClass = "errorFormulario";
            validadorNumEquipoCalentadorChaffoteaux.Enabled = false;

            //15/12/2017 Validacion caldera Chaffoteaux
            /*
            •	Tienen que ser un total de 21 caracteres alfanuméricos
            •	7 primeros dígitos tienen que ser numéricos y empiezan por 3
            •	Los dígitos 8 y 9 deben ser siempre “23”
            •	Los dígitos 10 y 11 corresponden al año de fabricación por lo que tienen que ser numéricos
            •	Los 12, 13, 14 son el día del año de fabricación así que también tienen que ser numéricos
            •	Los que faltan hasta 21, es el número de identificación del equipo
           */
            //RegularExpressionValidator validadorNumEquipoCalderaChaffoteaux = new RegularExpressionValidator();
            //validadorNumEquipoCalderaChaffoteaux.ValidationExpression = @"3[0-9]{6}23[0-9]{12}";
            //validadorNumEquipoCalderaChaffoteaux.ErrorMessage = "*";
            //validadorNumEquipoCalderaChaffoteaux.ToolTip = "El número de equipo debe de ser de 21 caracteres, empezar por 3 seguido de 6 dígitos más, seguido de '23' y 12 digitos más.";
            //validadorNumEquipoCalderaChaffoteaux.CssClass = "errorFormulario";
            //validadorNumEquipoCalderaChaffoteaux.Enabled = false;
            
            //16/05/2019 R17355
            RegularExpressionValidator validadorNumEquipoCalderaChaffoteaux = new RegularExpressionValidator();
            validadorNumEquipoCalderaChaffoteaux.ValidationExpression = @"3[0-9]{6}(23|CN)[0-9]{11,12}";
            validadorNumEquipoCalderaChaffoteaux.ErrorMessage = "*";
            validadorNumEquipoCalderaChaffoteaux.ToolTip = "El número de equipo debe de ser de 20/21 caracteres, empezar por 3 seguido de 6 dígitos más, seguido de '23' o 'CN' y 11/12 digitos más.";
            validadorNumEquipoCalderaChaffoteaux.CssClass = "errorFormulario";
            validadorNumEquipoCalderaChaffoteaux.Enabled = false;

            //15/12/2017 Validacion Beretta -> Total de caracteres 11: 4 son letras y  7 numéricos
            RegularExpressionValidator validadorNumSerieBeretta = new RegularExpressionValidator();
            validadorNumSerieBeretta.ValidationExpression = @"[0-9a-zA-Z]{4}[0-9]{7}";
            validadorNumSerieBeretta.ErrorMessage = "*";
            validadorNumSerieBeretta.ToolTip = "El número de serie debe de ser de 11 caracteres, 4 letras y 7 dígitos.";
            validadorNumSerieBeretta.CssClass = "errorFormulario";
            validadorNumSerieBeretta.Enabled = false;

            //06/03/2018 Validacion VIessman -> Total de caracteres 1, sölo números
            RegularExpressionValidator validadorNumSerieViessman = new RegularExpressionValidator();
            validadorNumSerieViessman.ValidationExpression = @"\d{16}";
            validadorNumSerieViessman.ErrorMessage = "*";
            validadorNumSerieViessman.ToolTip = "El número de serie debe tener 16 dígitos.";
            validadorNumSerieViessman.CssClass = "errorFormulario";
            validadorNumSerieViessman.Enabled = false;

            // 21/08/2019 #R17783
            RequiredFieldValidator validadorFicheroContratoInspeccion = new RequiredFieldValidator();
            validadorFicheroContratoInspeccion.ErrorMessage = "*";
            validadorFicheroContratoInspeccion.ToolTip = "Campo obligatorio.";
            //validadorNumeroConDosDecimales.runat = "server";
            validadorFicheroContratoInspeccion.CssClass = "errorFormulario";
            validadorFicheroContratoInspeccion.Enabled = false;

            int contadorAverias = 0;
            //******************************************************************************************

            for (int i = 0; i <= dsCaracteristicas.Tables[0].Rows.Count - 1; i++)
            {
                string tipoDato = dsCaracteristicas.Tables[0].Rows[i].ItemArray[4].ToString();
                Boolean obligatorio = Boolean.Parse(dsCaracteristicas.Tables[0].Rows[i].ItemArray[5].ToString());

                Cargar = true;
                // *****************************************************************************************
                // Kintell 26/02/2014 Espejo estados cracteristicas.
                // Para que saque características en otros estados, pero que no puedan ser modificados.
                //Habilitado = Boolean.Parse(dsCaracteristicas.Tables[0].Rows[i].ItemArray[6].ToString());
                // *****************************************************************************************
                string tipCar = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString();

                // *****************************************************************************************
                // (20/08/2015) Tema avería gas confort, miramos si venimos de un cambio en el combo de 'Avería en:'.
                if (argument != "" && argument != null)
                {
                    // (20/08/2015) Cargamos el argumento.
                    string[] sNumero = argument.Split('-');
                    // (20/08/2015) Piyamos el id del valor seleccionado.
                    string idTable = sNumero[0].ToString();
                    string codTableValue = sNumero[1].ToString();


                    // (20/08/2015) Necesitamos coger el valor seleccionado para ver que características cargar y cuales no.
                    // idTable = 31	     codTableValue = 1	    Descripcion = Aparato
                    // idTable = 31      codTableValue = 2	    Descripcion = Resto Instalación
                    // idTable = 31      codTableValue = 3	    Descripcion = Ambas
                    // idTable = 31      codTableValue = 4	    Descripcion = Aparato sin Recambio


                    if (idTable == "31" && codTableValue == "2")
                    {
                        // (21/08/2015) Tema avería gas confort, si es en "Resto de instalación".
                        // (21/08/2015) Tema avería gas confort, no sacamos las características de la 128 a la 140 ni de la 114 a la 122.
                        if (tipCar == "128" || tipCar == "129" || tipCar == "130" || tipCar == "131" || tipCar == "132" || tipCar == "133"
                            || tipCar == "134"
                             || tipCar == "135" || tipCar == "136" || tipCar == "137" || tipCar == "138" || tipCar == "139" || tipCar == "140")
                        // Quitamos porque Edurne dice que los datos del ticket de combustion tienen que salir siempre. (Mail: 08/02/2016)
                        //|| tipCar=="114"
                        //|| tipCar=="115" || tipCar=="116" || tipCar=="117" || tipCar=="118" || tipCar=="119" || tipCar=="120" || tipCar=="121"
                        //|| tipCar=="122")
                        {
                            Cargar = false;
                        }
                    }
                    else if (idTable == "31" && codTableValue != "2")
                    {
                        // (21/08/2015) Tema avería gas confort, si NO es en "Resto de instalación".
                        // (21/08/2015) Tema avería gas confort, no sacamos la característica 141 ni 140.
                        if (tipCar == "141" || tipCar == "140")
                        {
                            Cargar = false;
                        }
                    }

                    if (idTable == "31" && codTableValue == "1")
                    {
                        // (21/08/2015) Tema avería gas confort, si es en "Aparato".
                        // (21/08/2015) Tema avería gas confort, no sacamos las características de la 146 a la 151.

                        if (tipCar == "146" || tipCar == "147" || tipCar == "148" || tipCar == "149" || tipCar == "150" || tipCar == "151")
                        {
                            Cargar = false;
                        }
                        
                    }
                    else if (idTable == "31" && codTableValue == "2")
                    {
                        // (21/08/2015) Tema avería gas confort, si es en "Resto de instalación".
                        // (21/08/2015) Tema avería gas confort, no sacamos las características de la 142 a la 145.
                        if (tipCar == "142" || tipCar == "143" || tipCar == "144" || tipCar == "145")
                        {
                            Cargar = false;
                        }
                        //17/10/2019 BGN R#17876 Heredar Nº Serie en Averias GC en Aparato
                        if (tipCar == "169")
                        {
                            Cargar = false;
                        }
                    }
                    else
                    {
                        //17/10/2019 BGN R#17876 Heredar Nº Serie en Averias GC en Aparato
                        if (tipCar == "169")
                        {
                            Cargar = false;
                        }
                    }

                    if (idTable == "31" && codTableValue == "4")
                    {
                        // (21/08/2015) Tema avería gas confort, si es en "Aparato".
                        // (21/08/2015) Tema avería gas confort, no sacamos las características de la 146 a la 151.

                        if (int.Parse(tipCar) > 130 ||
                        
                                int.Parse(tipCar)==114 ||
                                int.Parse(tipCar)==115 ||
                                int.Parse(tipCar)==116 ||
                                int.Parse(tipCar)==117 ||
                                int.Parse(tipCar)==118 ||
                                int.Parse(tipCar)==119 ||
                                int.Parse(tipCar)==120 ||
                                int.Parse(tipCar)==121 ||
                                int.Parse(tipCar)==122 ||
                                int.Parse(tipCar) == 128 ||
                                int.Parse(tipCar)==129
                            )
                            {
                            Cargar = false;
                        }
                    }

                    // TEMA ELEGIR DIA PAGO, SOLO TIENE QUE CARGARSE CUANDO SEA PAGO FRACCIONADO.
                    //if (tipCar == "153")
                    //{
                    //    Cargar = false;
                    //}
                    if (idTable == "8")
                    {
                        if (codTableValue == "10" || codTableValue == "12" || codTableValue == "14" || codTableValue == "16" || codTableValue == "18"
                           || codTableValue == "2" || codTableValue == "20" || codTableValue == "22" || codTableValue == "24" || codTableValue == "26" || codTableValue == "35"
                           || codTableValue == "38" || codTableValue == "4" || codTableValue == "41" || codTableValue == "44" || codTableValue == "47" || codTableValue == "50"
                           || codTableValue == "53" || codTableValue == "56" || codTableValue == "6" || codTableValue == "8" || codTableValue == "82"
                           || codTableValue == "73"
                           || codTableValue == "76"
                           || codTableValue == "79"
                           || codTableValue == "85"
                           || codTableValue == "88"
                           || codTableValue == "91"
                           || codTableValue == "94"
                           || codTableValue == "97"
                           || codTableValue == "100"
                           || codTableValue == "103"
                           || codTableValue == "106"
                           || codTableValue == "109"
                           || codTableValue == "112"
                           || codTableValue == "115"
                           || codTableValue == "118"
                           || codTableValue == "121"
                           || codTableValue == "124"
                           )
                        {
                            if (tipCar == "153")
                            {
                                Cargar = true;
                                hdnMostrarDiaPago.Value = "true";
                            }
                        }
                        else
                        {
                            if (tipCar == "153")
                            {
                                Cargar = false;
                                hdnMostrarDiaPago.Value = "false";
                            }
                        }
                    }

                    //

                    //// DICE EDURNE QUE PREFIEREN QUE SALGAN REPETIDAS. ASI K LO MANTENEMOS (pero le he cambiado la descripción).
                    //if (idTable == "31" && codTableValue == "3")
                    //{
                    //    // (21/08/2015) Tema avería gas confort, si es "Ambas" las características de "Mano de obra" y "Coste repercutido" nos saldría repetido,
                    //    // por lo que decidimos quitar las que van con el de "Resto de instalación".
                    //    if (tipCar == "149" || tipCar == "151")
                    //    {
                    //        Cargar = false;
                    //    }
                    //}

                    // Ocultar datos bancarios si es único
                    if (idTable == "8" && codTableValue == "28")
                    {
                        // (21/08/2015) Tema avería gas confort, si es en "Resto de instalación".
                        // (21/08/2015) Tema avería gas confort, no sacamos las características de la 128 a la 140 ni de la 114 a la 122.
                        if (tipCar == "152")
                        {
                            Cargar = false;
                            hdnMostrarDatosBancarios.Value = "false";
                        }
                    }

                    // Mirar si venimos del combo modelo termostato inteligente
                    if (idTable == "154" && codTableValue == "1")
                    { 
                        // Honeywell
                        esHoneywell = true;
                    }

                    ////BEG BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección
                    //if (hdnCodigoOperacion.Value == "Caracteristica1")
                    //{
                    //    if (idTable == "161" && codTableValue == "02")
                    //    {
                    //        //ORD In Loco = No -> No mostramos Fornecimento Gas
                    //        hdnMostrarFornecimientoGas.Value = "false";
                    //    }
                    //    else if (idTable == "161" && codTableValue == "01")
                    //    {
                    //        //ORD In Loco = Si -> Mostramos Fornecimento Gas
                    //        hdnMostrarFornecimientoGas.Value = "true";
                    //    }
                    //}
                    ////END BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección

                    //**************************************************************************************************
                    // Pedir unidades de recambio si se ha seleccioado "sustituido".
                    if (idTable.ToString().Length >= 2 && codTableValue.ToString().Length >= 3)
                    {
                        if (idTable.Substring(0, 2) == "00" && codTableValue != "004")
                        {
                            // Si es la característica de codigo operacion 1 y no ha seleccionado "sustituido", marcamos para no cargar la característica "unidades de recambio 1".
                            if (hdnCodigoOperacion.Value == "Caracteristica5")
                            {
                                // Código de operación 1.
                                if (tipCar == "132")
                                {
                                    hdnVisibleUnidades1.Value = "0";
                                }
                            }
                            // Si es la característica de codigo operacion 2 y no ha seleccionado "sustituido", marcamos para no cargar la característica "unidades de recambio 2".
                            else if (hdnCodigoOperacion.Value == "Caracteristica11")
                            {
                                // Código de operación 2.
                                if (tipCar == "138")
                                {
                                    hdnVisibleUnidades2.Value = "0";
                                }
                            }
                        }
                        else if (idTable.Substring(0, 2) == "00" && codTableValue == "004")
                        {
                            // Si es la característica de codigo operacion 1 y SÍ ha seleccionado "sustituido", marcamos para cargar la característica "unidades de recambio 1".
                            if (hdnCodigoOperacion.Value == "Caracteristica5")
                            {
                                // Código de operación 1.
                                if (tipCar == "132")
                                {
                                    hdnVisibleUnidades1.Value = "1";
                                }
                            }
                            // Si es la característica de codigo operacion 2 y SÍ ha seleccionado "sustituido", marcamos para cargar la característica "unidades de recambio 2".
                            else if (hdnCodigoOperacion.Value == "Caracteristica11")
                            {
                                // Código de operación 2.
                                if (tipCar == "138")
                                {
                                    hdnVisibleUnidades2.Value = "1";
                                }
                            }
                        }
                        // Comprobamos si hay que cagar la característica o no.
                        if (hdnVisibleUnidades1.Value == "0" && tipCar == "132")
                        {
                            Cargar = false;
                        }
                        if (hdnVisibleUnidades2.Value == "0" && tipCar == "138")
                        {
                            Cargar = false;
                        }
                    }
                    //**************************************************************************************************
                }
                // *****************************************************************************************

                if (hdnMostrarDatosBancarios.Value == "false" && tipCar == "152")
                {
                    Cargar = false;
                }

                if (hdnMostrarDiaPago.Value == "false" && tipCar == "153")
                {
                    Cargar = false;
                }

                ////BEG BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección
                //if (hdnMostrarFornecimientoGas.Value == "false" && tipCar == "196")
                //{
                //    Cargar = false;
                //}
                ////END BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección

                // *****************************************************************************************
                // 18/08/2015 TEMA BERETTA

                // Si no es de condensación NO sacamos la característica del termostato HI (092) y nombre termostato (154).
                if (hdnTipologiaCalderaGC.Value.ToString().ToUpper().IndexOf("CONDENSACI").Equals(-1) && (tipCar == "092" || tipCar == "154"))
                {
                    Cargar = false;
                }
                // Si es Beretta no cargamos la característica número de serie (086).
                if (hdnMarcaCaldera.Value == "5" && tipCar == "086")
                {
                    Cargar = false;
                }
                // Si NO es Beretta no cargamos la característica matrícula del aparato (093).
                if (hdnMarcaCaldera.Value != "5" && tipCar == "093")
                {
                    Cargar = false;
                }
                // Si es calentador y caracteristica de la 099 a la 113 no hay que sacarlas.
                // 099
                // 113
                if (hdnTipologiaCalderaGC.Value.ToString().ToUpper().IndexOf("CALENTADOR") >= 0 && (tipCar == "099" || tipCar == "100"
                    || tipCar == "101" || tipCar == "102" || tipCar == "103" || tipCar == "104" || tipCar == "105" || tipCar == "106"
                    || tipCar == "107" || tipCar == "108" || tipCar == "109" || tipCar == "110" || tipCar == "111" || tipCar == "112"
                    || tipCar == "113"))
                {
                    Cargar = false;
                }
                // *****************************************************************************************

                if (Cargar)
                {
                    //contadorAverias = contadorAverias + 1;
                    TableRow lineaCaracteristica1 = new TableRow();
                    lineaCaracteristica1.ID = "lineaCaracteristica" + i;

                    TableCell ce = new TableCell();
                    ce.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString();
                    ce.CssClass = "labelFormulario";
                    ce.ID = "labelCaracteristica" + i;
                    lineaCaracteristica1.Cells.Add(ce);

                    ce = new TableCell();

                    if (tipoDato.IndexOf("Texto") >= 0 || tipoDato.IndexOf("Fecha") >= 0)
                    {
                        TextBox a = new TextBox();
                        a.ID = "Caracteristica" + i;
                        //a.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                        a.Enabled = Habilitado;

                        if (tipoDato.IndexOf("Fecha") >= 0)
                        {
                            a.Width = new Unit("60px");
                            a.MaxLength = 10;
                        }
                        else
                        {
                            a.Width = 300;
                        }

                        //17/10/2019 BGN R#17876 Heredar Nº Serie en Averias GC en Aparato
                        if (tipCar == "169" )//&& String.IsNullOrEmpty(a.Text.Trim()) && !String.IsNullOrEmpty(hdnNumSerie.Value))
                        {
                            a.Text = hdnNumSerie.Value;
                            a.Enabled = false;
                        }
                        else
                        {
                            a.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                        }

                        // 14 digitos 18/03/2015
                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString().ToUpper().IndexOf("BARRAS") >= 0 || dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString().ToUpper().IndexOf("BB") >= 0)
                        {
                            validadorNumeroCaracteres.ID = "txtInfoCodigoBarras_14digitos" + i;
                            validadorNumeroCaracteres.ControlToValidate = "Caracteristica" + i;
                            validadorNumeroCaracteres.Enabled = Habilitado;

                            ce.Controls.Add(validadorNumeroCaracteres);
                        }

                        // 7 digitos 16/12/2015
                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString().ToUpper().IndexOf("TERMOSTATO") >= 0)
                        {
                            if (esHoneywell)
                            {
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.ID = "txtInfoNumeroSerieTermostatoInteligenteHoneyWell_12digitosalfanumerico" + i;
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.ControlToValidate = "Caracteristica" + i;
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell.Enabled = Habilitado;

                                ce.Controls.Add(validadorNumeroCaracteresNumeroSerieTermostatoInteligenteHoneywell);
                            }
                            else
                            {
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligente.ID = "txtInfoNumeroSerieTermostatoInteligente_7digitosalfanumericoEmpiezaporG" + i;
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligente.ControlToValidate = "Caracteristica" + i;
                                validadorNumeroCaracteresNumeroSerieTermostatoInteligente.Enabled = Habilitado;

                                ce.Controls.Add(validadorNumeroCaracteresNumeroSerieTermostatoInteligente);
                            }
                        }

                        //15/12/2017 Validaciones Número de serie y Matricula
                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString().ToUpper().IndexOf("SERIE DEL EQUIPO") >= 0)
                        {
                            //Es Chaffoteaux
                            if (hdnMarcaCaldera.Value == "7")
                            {
                                if (hdnTipologiaCalderaGC.Value.ToString().ToUpper().IndexOf("CALENTADOR") >= 0)
                                {
                                    //Es calentador
                                    validadorNumEquipoCalentadorChaffoteaux.ID = "";
                                    validadorNumEquipoCalentadorChaffoteaux.ControlToValidate = "Caracteristica" + i;
                                    validadorNumEquipoCalentadorChaffoteaux.Enabled = Habilitado;
                                    ce.Controls.Add(validadorNumEquipoCalentadorChaffoteaux);
                                }
                                else //if (hdnTipologiaCalderaGC.Value.ToString().ToUpper().IndexOf("CALDERA") >= 0)
                                {
                                    //Es caldera de condensacion
                                    validadorNumEquipoCalderaChaffoteaux.ID = "";
                                    validadorNumEquipoCalderaChaffoteaux.ControlToValidate = "Caracteristica" + i;
                                    validadorNumEquipoCalderaChaffoteaux.Enabled = Habilitado;
                                    ce.Controls.Add(validadorNumEquipoCalderaChaffoteaux);
                                }
                            }
                            //Es Viessman
                            if (hdnMarcaCaldera.Value == "5798")
                            {
                                validadorNumSerieViessman.ID = "";
                                validadorNumSerieViessman.ControlToValidate = "Caracteristica" + i;
                                validadorNumSerieViessman.Enabled = Habilitado;
                                ce.Controls.Add(validadorNumSerieViessman);
                            }
                        }

                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString().ToUpper().IndexOf("MATRÍCULA") >= 0)
                        {
                            //Es Beretta
                            if (hdnMarcaCaldera.Value == "5")
                            {
                                validadorNumSerieBeretta.ID = "";
                                validadorNumSerieBeretta.ControlToValidate = "Caracteristica" + i;
                                validadorNumSerieBeretta.Enabled = Habilitado;
                                ce.Controls.Add(validadorNumSerieBeretta);
                            }
                        }

                        a.EnableViewState = false;
                        ce.Controls.Add(a);

                        lineaCaracteristica1.Cells.Add(ce);

                        if (tipoDato.IndexOf("Fecha") >= 0)
                        {

                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.ID = "imgBtn_Caracteristica" + i;
                            img.ImageUrl = "./HTML/IMAGES/imagenCalendario.gif";
                            img.ToolTip = "Calendario";
                            if (Habilitado == true)
                            {
                                img.Attributes.Add("onclick", "ShowCalendar(this, 'ctl00_ContentPlaceHolderContenido_" + a.ID + "',event);");
                            }
                            ce.Controls.Add(img);
                        }


                    }
                    else if (tipoDato.IndexOf("Sencilla") >= 0)
                    {
                        DropDownList a = new DropDownList();
                        a.ID = "Caracteristica" + i;
                        a.Items.Add("");
                        a.SelectedIndex = 0;
                        a.EnableViewState = false;

                        //a.AutoPostBack = true;
                        // a.Attributes.Add("runat","server");
                        // a.Attributes.Add("onselectedindexchanged", "a_CambioComboCaracteristica");
                        string idCaracteristica = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString();

                        // (20/08/2015) Si es la avería de gas confort hacemos postback para ver que ha seleccionado.
                        //if (hdnAltaIncidenciaGasConfort.Value == "1" && idCaracteristica == "127")
                        if (idCaracteristica == "127")
                        {
                            //a.AutoPostBack = true;
                            a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "','Caracteristica2')");
                            a.EnableViewState = true;
                        }
                        // (17/02/2016) Si es la avería de gas confort hacemos postback para ver que ha seleccionado (Operacion 1: sustituido).
                        //if (hdnAltaIncidenciaGasConfort.Value == "1" && idCaracteristica == "129")
                        if (idCaracteristica == "129")
                        {
                            //a.AutoPostBack = true;
                            a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "','Caracteristica5')");
                            a.EnableViewState = true;
                        }
                        // (17/02/2016) Si es la avería de gas confort hacemos postback para ver que ha seleccionado (Operacion 2: sustituido).
                        //if (hdnAltaIncidenciaGasConfort.Value == "1" && idCaracteristica == "135")
                        if (idCaracteristica == "135")
                        {
                            //a.AutoPostBack = true;
                            a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "','Caracteristica11')");
                            a.EnableViewState = true;
                        }
                        // (11/04/2018) Modelo termostato, si es Honeywell tienen que meter Nº alfanumérico de 12, si es Netatmo, como esta actualmente k es X.
                        if (idCaracteristica == "154")
                        {
                            //a.AutoPostBack = true;
                            a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "','Caracteristica5')");
                            a.EnableViewState = true;
                        }
                        ////BEG BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección
                        //if (idCaracteristica == "161")
                        //{
                        //    a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "','Caracteristica1')");
                        //    a.EnableViewState = true;
                        //}
                        ////END BGN 02/05/2022 R#36138 - Nuevo campo en las solicitudes de inspección

                        // Seek if it is a "father" characteristic.
                        DataSet datos = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE,DESC_TABLE_VALUE,VALUE_ORDER,VISIBLE,PARENT_ID_TABLE,PARENT_COD_TABLE_VALUE,ID_CARACTERISTICA,NOMBRE_TABLA FROM P_TABLE_VALUE WHERE ID_CARACTERISTICA='" + idCaracteristica + "' AND (PARENT_ID_TABLE IS NULL OR PARENT_ID_TABLE='') AND VISIBLE=1 AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");

                        if (datos.Tables[0].Rows.Count > 0)
                        {
                            // Look if we have to take the values from the table o from a sentence.
                            if (string.IsNullOrEmpty(datos.Tables[0].Rows[0].ItemArray[8].ToString()))
                            {
                                // Select the values of characteristic.
                                DataSet datosEsPadre = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE FROM P_TABLE_VALUE WHERE VISIBLE = 1 AND PARENT_ID_TABLE='" + datos.Tables[0].Rows[0].ItemArray[0].ToString() + "' AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");
                                if (datosEsPadre.Tables[0].Rows.Count > 0)
                                {
                                    a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "-" + idCaracteristica + "-" + datos.Tables[0].Rows[0].ItemArray[0].ToString() + "-" + datos.Tables[0].Rows[0].ItemArray[1].ToString() + "','" + a.ID + "');");
                                    a.EnableViewState = true;
                                }
                                else
                                {
                                    a.EnableViewState = false;
                                }

                                Boolean valorEncontradoEnCombo = false;
                                for (int numeroCarac = 0; numeroCarac < datos.Tables[0].Rows.Count; numeroCarac++)
                                {
                                    if (datos.Tables[0].Rows[0].ItemArray[4].ToString() == "True")
                                    {
                                        a.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                        a.Items[a.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString().ToUpper())
                                        {
                                            a.SelectedIndex = a.Items.Count - 1;
                                            valorEncontradoEnCombo = true;
                                        }
                                    }
                                }

                                // Si el combo tiene que estar deshabilitado y además no hemos encontrado
                                // la opción a seleccionar en el combo, lo añadimos y lo seleccionamos.
                                if (!valorEncontradoEnCombo && !Habilitado)
                                {
                                    a.Items.Add(dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper());
                                    a.Items[a.Items.Count - 1].Value = datos.Tables[0].Rows[i].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[i].ItemArray[1].ToString();
                                    a.SelectedIndex = a.Items.Count - 1;
                                }


                                //pasadoPadre = true;
                                idCaracteristicaPadre = idCaracteristica;
                            }
                            else
                            {
                                // If it is from a sentence.
                                string nombreTabla = datos.Tables[0].Rows[0].ItemArray[8].ToString();
                                // 24/11/2015
                                //*************************************************************************************************************************
                                DataSet datosTabla = new DataSet();
                                //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                                //UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                                if (nombreTabla == "TECNICOEMPRESA" && usuarioDTO.NombreProveedor != "ADICO" && usuarioDTO.NombreProveedor != "TELEFONO")
                                {
                                    datosTabla = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT * FROM " + nombreTabla + " WHERE PROVEEDOR_TECNICOEMPRESA='" + usuarioDTO.NombreProveedor + "' order by 2");
                                }
                                else
                                {
                                    if (nombreTabla == "CUI_DISTRIBUIDORA_HORARIO")
                                    {
                                        string contrato = hdnCodContrato.Value;
                                        SolicitudDB solDB = new SolicitudDB();
                                        DataTable dtHorario = new DataTable();
                                        dtHorario = solDB.ObtenerHorarioInspeccionPorContrato(contrato).Copy();
                                        datosTabla.Tables.Add(dtHorario);
                                    }
                                    else
                                    {
                                        datosTabla = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT * FROM " + nombreTabla + " order by 2");
                                    }
                                }
                                //DataSet datosTabla = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT * FROM " + nombreTabla + " order by 2");
                                //*************************************************************************************************************************
                                for (int numeroCaracTabla = 0; numeroCaracTabla < datosTabla.Tables[0].Rows.Count; numeroCaracTabla++)
                                {
                                    a.Items.Add(datosTabla.Tables[0].Rows[numeroCaracTabla].ItemArray[1].ToString());
                                    a.Items[a.Items.Count - 1].Value = datosTabla.Tables[0].Rows[numeroCaracTabla].ItemArray[0].ToString() + '-' + datosTabla.Tables[0].Rows[numeroCaracTabla].ItemArray[1].ToString();
                                    if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == datosTabla.Tables[0].Rows[numeroCaracTabla].ItemArray[1].ToString().ToUpper())
                                    {
                                        a.SelectedIndex = a.Items.Count - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // if it is not the father.
                            Boolean noPasado = false;
                            // Look if have a value in the value's table.
                            if (hdnCaracteristicaCalderasSeleccionado.Value != "")
                            {
                                // If have.
                                // Select the child values in the table.
                                string[] sNumero = hdnCaracteristicaCalderasSeleccionado.Value.Split('_');
                                datos.Clear();

                                datos = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE,DESC_TABLE_VALUE,VALUE_ORDER,VISIBLE,PARENT_ID_TABLE,PARENT_COD_TABLE_VALUE,ID_CARACTERISTICA,NOMBRE_TABLA FROM P_TABLE_VALUE WHERE VISIBLE = 1 AND PARENT_ID_TABLE='" + sNumero[0] + "' AND PARENT_COD_TABLE_VALUE='" + sNumero[1] + "' AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");
                                if (datos.Tables.Count > 0)
                                {
                                    if (datos.Tables[0].Rows.Count > 0)
                                    {

                                        a.Attributes.Add("onchange", "javascript:Cargacombo('" + a.ID + "-" + idCaracteristica + "-" + datos.Tables[0].Rows[0].ItemArray[0].ToString() + "-" + datos.Tables[0].Rows[0].ItemArray[1].ToString() + "','" + a.ID + "');");
                                        a.EnableViewState = true;

                                        for (int numeroCarac = 0; numeroCarac < datos.Tables[0].Rows.Count; numeroCarac++)
                                        {
                                            if (a.Items.Count == 0)
                                            {
                                                a.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                                a.Items[a.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                            }
                                            if (datos.Tables[0].Rows[0].ItemArray[4].ToString() == "True")
                                            {
                                                a.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                                a.Items[a.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                            }
                                        }
                                    }
                                    //}
                                    else { noPasado = true; }
                                }
                                else { noPasado = true; }
                            }
                            else { noPasado = true; }

                            if (noPasado)
                            {
                                // If don´t have.
                                DataSet datosExisteEnTablaMaestra = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE FROM P_TABLE_VALUE WHERE VISIBLE = 1 AND ID_CARACTERISTICA='" + idCaracteristica + "' AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");// AND PARENT_ID_TABLE='" + idPadre + "'");

                                if (datosExisteEnTablaMaestra.Tables[0].Rows.Count == 0)
                                {
                                    //if (pasadoPadre == false)
                                    a.Items.Add("CORRECTO");
                                    a.Items.Add("INCORRECTO");
                                    a.EnableViewState = false;
                                    if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "CORRECTO" || dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "SI" || dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "SÍ")
                                    {
                                        a.SelectedIndex = 1;
                                    }
                                    else if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "INCORRECTO" || dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "NO")
                                    {
                                        a.SelectedIndex = 2;
                                    }
                                    else
                                    {
                                        a.Items.Add(dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper());
                                        a.Items[a.Items.Count - 1].Value = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString() + '-' + dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString();
                                        a.SelectedIndex = a.Items.Count - 1;
                                    }
                                }
                            }

                            if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() != "")
                            {
                                a.Items.Add(dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper());
                                //a.Items[a.Items.Count - 1].Value = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString() + '-' + dsCaracteristicas.Tables[0].Rows[i].ItemArray[1].ToString();
                                a.SelectedIndex = a.Items.Count - 1;
                            }

                        }

                        a.Enabled = Habilitado;

                        // Si tiene valor OK en el combo, tenemos que marcarlo como valor por defecto.
                        for (int contValores = 0; contValores < a.Items.Count - 1; contValores++)
                        {
                            if (a.Items[contValores].Text == "OK")
                            {
                                a.SelectedIndex = contValores;
                            }
                        }


                        ce.Controls.Add(a);
                        lineaCaracteristica1.Cells.Add(ce);
                    }
                    else if (tipoDato.IndexOf("/No") >= 0)
                    {
                        CheckBox a = new CheckBox();
                        a.EnableViewState = false;
                        a.ID = "Caracteristica" + i;
                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "SI" || dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper() == "SÍ")
                        {
                            a.Checked = true;
                        }
                        else
                        {
                            a.Checked = false;
                        }
                        //R#36525 Kintell Begin
                        if (dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString().ToUpper()=="" && tipCar=="192")
                        {
                            a.Checked = true;
                        }
                        a.Attributes.Add("onClick", "InformarTermostato()");
                        a.Attributes.Add("onChange", "InformarTermostato()");
                        a.EnableViewState = true;
                        // Kintell End
                        a.Enabled = Habilitado;
                        ce.Controls.Add(a);
                        lineaCaracteristica1.Cells.Add(ce);
                    }
                    else if (tipoDato.IndexOf("Decimal") >= 0 || tipoDato.IndexOf("Numero") >= 0 || tipoDato.IndexOf("Hora") >= 0)
                    {
                        TextBox a = new TextBox();
                        a.Width = 300;
                        a.EnableViewState = false;
                        a.ID = "Caracteristica" + i;
                        a.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                        a.Enabled = Habilitado;
                        ce.Controls.Add(a);
                        lineaCaracteristica1.Cells.Add(ce);
                        if (tipCar == "145")
                        {
                            a.Text = "0€ " + Resources.TextosJavaScript.TEXTO_POR_GARANTIA_TOTAL;//por garantía total";
                            a.Enabled = false;
                        }
                        else
                        {
                            // Ponemos la validación de decimales para todos los importes.
                            if (tipoDato.IndexOf("Decimal") >= 0 || tipoDato.IndexOf("Numero") >= 0)
                            {
                                validadorNumeroConDosDecimales.ID = "txtCosteMaterial" + i;
                                validadorNumeroConDosDecimales.ControlToValidate = "Caracteristica" + i;
                                validadorNumeroConDosDecimales.Enabled = Habilitado;

                                ce.Controls.Add(validadorNumeroConDosDecimales);
                            }
                        }
                    }
                    else if (tipoDato.IndexOf("Fichero") >= 0)
                    {
                        if (Habilitado)
                        {
                            FileUpload a = new FileUpload();

                            a.Width = 300;
                            a.EnableViewState = false;
                            a.ID = "Caracteristica" + i;
                            a.Enabled = Habilitado;
                            a.Attributes.Add("onchange", "InformarNombreFichero()");
                            ce.Controls.Add(a);
                            lineaCaracteristica1.Cells.Add(ce);
                            if (tipCar == "165")
                            {
                                validadorFicheroContratoInspeccion.ID = "FileContratoInspeccion" + i;
                                validadorFicheroContratoInspeccion.ControlToValidate = "Caracteristica" + i;
                                validadorFicheroContratoInspeccion.Enabled = Habilitado;

                                ce.Controls.Add(validadorFicheroContratoInspeccion);
                            }
                        }
                        else 
                        {
                            TextBox a = new TextBox();
                            a.Width = 300;
                            a.EnableViewState = false;
                            a.ID = "Caracteristica" + i;
                            a.Enabled = Habilitado;
                            a.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                            ce.Controls.Add(a);
                            lineaCaracteristica1.Cells.Add(ce);
                        }
                    }



                    tblCaracteristicas.Rows.Add(lineaCaracteristica1);

                    hdnTotalCaracteristicas.Value = (i + 1).ToString();
                    //hdnTotalCaracteristicas.Value = (contadorAverias + 1).ToString();
                }
            }
            //hdnCaracteristicaCalderasSeleccionado.Value = "";
        }

        

        public void CambioComboCaracteristica_SelectedIndexChanged(string ID, string selectedValue)
        {
            //CambioComboCaracteristicaPorNombre_SelectedIndexChanged(ID, selectedValue);
            //return;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            int IdIdioma = usuarioDTO.IdIdioma;
            //throw new NotImplementedException();

            string[] sNumero = ID.Split('-');
            //string idcaracteristica = sNumero[1].ToString();
            //string idTable = sNumero[2].ToString();
            //string codTableValue = sNumero[3].ToString();
            string sNumCaracteristica = sNumero[0].ToString().Substring(14);
            //string idObjetoActual = "Caracteristica" + int.Parse(sNumCaracteristica);
            //Control ObjetoActual = FindControlById(tblCaracteristicas, idObjetoActual);
            string idObjeto = "Caracteristica" + (int.Parse(sNumCaracteristica) + 1);
            Control Objeto = FindControlById(tblCaracteristicas, idObjeto);
            SolicitudesDB objSolicitudesDB = new SolicitudesDB();

            string[] matrizSelectedValue = selectedValue.Split('-');

            DataSet datosActuales = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE FROM P_TABLE_VALUE WHERE VISIBLE = 1 AND ID_TABLE='" + matrizSelectedValue[0] + "' AND COD_TABLE_VALUE='" + matrizSelectedValue[1] + "' AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");
            DropDownList combo;
            try
            {
                combo = (DropDownList)Objeto;
            }
            catch (Exception ex)
            {
                // the object is not a dropdownlist.
                return;
            }
            if (selectedValue == "") { combo.Items.Clear(); ;}
            //DropDownList comboActual = (DropDownList)ObjetoActual;
            try
            {
                if (datosActuales.Tables[0].Rows.Count > 0)
                {
                    string idTable = datosActuales.Tables[0].Rows[0].ItemArray[0].ToString();
                    string codTableValue = datosActuales.Tables[0].Rows[0].ItemArray[1].ToString();
                    if (idTable == "1")
                    {
                        hdnCaracteristicaCalderasSeleccionado.Value = idTable + "_" + codTableValue;
                    }
                    else
                    {
                        //hdnCaracteristicaCalderasSeleccionado.Value = "";
                    }
                    if (Objeto != null)
                    {

                        DataSet datos = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE,DESC_TABLE_VALUE,VALUE_ORDER,VISIBLE,PARENT_ID_TABLE,PARENT_COD_TABLE_VALUE,ID_CARACTERISTICA,NOMBRE_TABLA FROM P_TABLE_VALUE WHERE VISIBLE = 1 AND PARENT_ID_TABLE='" + idTable + "' AND PARENT_COD_TABLE_VALUE='" + codTableValue + "' AND ID_IDIOMA='" + IdIdioma + "' order by VALUE_ORDER");

                        if (datos.Tables[0].Rows.Count > 0)
                        {

                            combo.Items.Clear();
                            string idCaracteristica = datos.Tables[0].Rows[0].ItemArray[7].ToString();
                            combo.ID = idObjeto;
                            combo.Items.Add("");
                            combo.SelectedIndex = 0;
                            combo.Attributes.Add("onchange", "javascript:Cargacombo('" + combo.ID + "-" + idCaracteristica + "-" + datos.Tables[0].Rows[0].ItemArray[0].ToString() + "-" + datos.Tables[0].Rows[0].ItemArray[1].ToString() + "','" + combo.ID + "');");
                            combo.EnableViewState = true;
                            for (int numeroCarac = 0; numeroCarac < datos.Tables[0].Rows.Count; numeroCarac++)
                            {
                                if (datos.Tables[0].Rows[0].ItemArray[4].ToString() == "True")
                                {
                                    // Si es el modo de pago, comprobamos si es solvente o no.
                                    if (idCaracteristica == "084")
                                    {
                                        // Si es KO solo permitimos pago anticipado.
                                        if (hdnSolCent.Value == "KO")
                                        {
                                            if (datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString().ToUpper().IndexOf("ANTICIPADO") >= 0)
                                            {
                                                combo.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                                combo.Items[combo.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            combo.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                            combo.Items[combo.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                        }
                                    }
                                    else
                                    {
                                        combo.Items.Add(datos.Tables[0].Rows[numeroCarac].ItemArray[2].ToString());
                                        combo.Items[combo.Items.Count - 1].Value = datos.Tables[0].Rows[numeroCarac].ItemArray[0].ToString() + '-' + datos.Tables[0].Rows[numeroCarac].ItemArray[1].ToString();
                                    }
                                }
                            }

                        }
                    }
                }

                for (int i = int.Parse(sNumCaracteristica) + 2; i < 99; i++)
                {
                    string idObjetoHijo = "Caracteristica" + i;
                    Control ObjetoHijo = FindControlById(tblCaracteristicas, idObjetoHijo);
                    DropDownList combo1 = (DropDownList)ObjetoHijo;
                    //DataSet datos = objSolicitudesDB.ObtenerDatosDesdeSentencia("SELECT ID_TABLE,COD_TABLE_VALUE,DESC_TABLE_VALUE,VALUE_ORDER,VISIBLE,PARENT_ID_TABLE,PARENT_COD_TABLE_VALUE,ID_CARACTERISTICA,NOMBRE_TABLA FROM P_TABLE_VALUE WHERE ID_CARACTERISTICA='" + idcaracteristica + "' AND (PARENT_ID_TABLE IS NULL OR PARENT_ID_TABLE='')");
                    //if (datos.Tables[0].Rows.Count == 0)
                    //{
                    if (combo1.EnableViewState) { combo1.Items.Clear(); }
                    //}
                }
            }
            catch (Exception ex)
            {
                // Object not a DropDownList or no more DropDownList´s in the tblCaracteristicas.
            }

        }

        private Control FindControlById(Control c, string id)
        {
            if (c.ID == id)
            {
                return c;
            }
            foreach (Control control in c.Controls)
            {
                Control ctlToFind = FindControlById(control, id);
                if (ctlToFind != null)
                    return ctlToFind;
            }

            return null;
        }

        private bool GuardarFicheroGC()
        {
            
            //******************************************************************************************************************************************
            // 11/10/2019 KINTELL, Guardamos los ficheros de GC si los hay.
            
            // COPIAMOS EL FICHERO.
            try
            {
                //HttpPostedFile fileContrato =FileGC
                string[] nombreFichero = FileGC.FileName.Split('\\');
                string nomFichero = nombreFichero[nombreFichero.Length - 1].Trim();
                if (ComprobarNombreFichero(nomFichero))
                {
                    using (ManejadorFicheros.GetImpersonator())
                    {
                        String Destino = (string)Documentos.ObtenerRutaFicheros();
                        {
                            string DestinoContrato = Destino + nomFichero;
                            FileGC.SaveAs(DestinoContrato);
                                
                            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                            //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                            //Documentos.InsertaDocumentoCargado(this.hdnCodContrato.Value, null, int.Parse(hdnIdSolicitud.Value), nombreFichero[nombreFichero.Length - 1].Trim(), 1, null, null, usuarioDTO.Login);
                            //Comprobar si para esta solicitud ya se ha creado en la tabla documentos el contrato y enviado a edatalia sin respuesta, pq en este caso habra que modificarlo
                            List<DocumentoDTO> lDoc = Documento.ObtenerDocumentosPorIdSolicitud(int.Parse(hdnIdSolicitud.Value));
                            DocumentoDTO docEncontrato = null;
                            foreach (DocumentoDTO doc in lDoc)
                            {
                                if ((doc.EnviarADelta==false) && (doc.FechaEnvioEdatalia != null) && doc.IdEstadoEdatalia==null)
                                {
                                    docEncontrato = doc;
                                    break;
                                }
                            }
	                        if (docEncontrato!=null)
	                        {
	                            DocumentoDTO documentoDto = docEncontrato;
	                            documentoDto.NombreDocumento = nomFichero;
	                            documentoDto.EnviarADelta = true;
	                            Documento.Actualizar(documentoDto, usuarioDTO.Login);
	                        }
	                        else
	                        {
	                            DocumentoDTO documentoDto = new DocumentoDTO();
	                            documentoDto.CodContrato = this.hdnCodContrato.Value;
	                            documentoDto.IdSolicitud = int.Parse(hdnIdSolicitud.Value);
	                            documentoDto.NombreDocumento = nomFichero;
	                            documentoDto.IdTipoDocumento = 1;
	                            documentoDto.EnviarADelta = true;
	                            documentoDto = Documento.Insertar(documentoDto, usuarioDTO.Login);
	                        }
	                        //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 
                        }
                    }
                }
                else
                {
                    // ERROR NOMBRE FICHERO CONTRATO. 27/08/2019 KINTELL Y BEA.
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
            
            //******************************************************************************************************************************************

            return true;
        }

        private bool GuardarFicheroInspeccion()
        {
            //******************************************************************************************************************************************
            // 28/08/2019 KINTELL, Guardamos los ficheros de inspeccion si los hay.
            if (hdnNombreficheroInspeccion.Value != "" && hdnSubtipo.Value=="016")
            {
                // COPIAMOS EL FICHERO.
                try
                {
                    HttpPostedFile fileContrato = Request.Files["ctl00$ContentPlaceHolderContenido$Caracteristica2"];
                    string[] nombreFichero = fileContrato.FileName.Split('\\');

                    if (ComprobarNombreFichero(nombreFichero[nombreFichero.Length - 1].Trim()))
                    {
                        using (ManejadorFicheros.GetImpersonator())
                        {
                            String Destino = (string)Documentos.ObtenerRutaFicherosInspeccion();
                            if (fileContrato != null)
                            {
                                string DestinoContrato = Destino + nombreFichero[nombreFichero.Length - 1].Trim();
                                fileContrato.SaveAs(DestinoContrato);
                                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                                //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                //Documentos.InsertaDocumentoCargado(this.hdnCodContrato.Value, null, int.Parse(hdnIdSolicitud.Value), nombreFichero[nombreFichero.Length - 1].Trim(), 111, null, null, usuarioDTO.Login);
                                DocumentoDTO documentoDto = new DocumentoDTO();
                                documentoDto.CodContrato = this.hdnCodContrato.Value;
                                documentoDto.IdSolicitud = int.Parse(hdnIdSolicitud.Value);
                                documentoDto.NombreDocumento = nombreFichero[nombreFichero.Length - 1].Trim();
                                documentoDto.IdTipoDocumento = 111;
                                documentoDto.EnviarADelta = true;
                                documentoDto = Documento.Insertar(documentoDto, usuarioDTO.Login);
                                //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital                                 

                                HttpPostedFile fileFactura = Request.Files["ctl00$ContentPlaceHolderContenido$Caracteristica3"];
                                if (fileFactura.FileName != "" && fileFactura.FileName!=null)
                                {
                                    string[] nombreFicheroFactura = fileFactura.FileName.Split('\\');
                                    if (ComprobarNombreFichero(nombreFicheroFactura[nombreFicheroFactura.Length - 1].Trim()))
                                    {
                                        string DestinoFactura = Destino + nombreFicheroFactura[nombreFicheroFactura.Length - 1].Trim();
                                        fileFactura.SaveAs(DestinoFactura);

                                        //20210119 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                                        //Documentos.InsertaDocumentoCargado(this.hdnCodContrato.Value, null, int.Parse(hdnIdSolicitud.Value), nombreFicheroFactura[nombreFicheroFactura.Length - 1].Trim(), 112, null, null, usuarioDTO.Login);
                                        DocumentoDTO documentoDtoFact = new DocumentoDTO();
                                        documentoDtoFact.CodContrato = this.hdnCodContrato.Value;
                                        documentoDtoFact.IdSolicitud = int.Parse(hdnIdSolicitud.Value);
                                        documentoDtoFact.NombreDocumento = nombreFicheroFactura[nombreFicheroFactura.Length - 1].Trim();
                                        documentoDtoFact.IdTipoDocumento = 112;
                                        documentoDtoFact.EnviarADelta = true;
                                        documentoDtoFact = Documento.Insertar(documentoDtoFact, usuarioDTO.Login);
                                        //20210119 MOD ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital 
                                    }
                                    else
                                    {
                                        // ERROR NOMBRE FICHERO FACTURA. 27/08/2019 KINTELL Y BEA.
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // ERROR NOMBRE FICHERO CONTRATO. 27/08/2019 KINTELL Y BEA.
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    this.ManageException(ex);
                }
            }
            hdnNombreficheroInspeccion.Value = "";
            //******************************************************************************************************************************************

            return true;
        }

        private void Guardar()
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
               
                //Para el caso de que hayan metido algo mal.
                string cod_estado = ddl_EstadoSol.SelectedValue;
                //************************************************
                string modoPago = "";
                string cuota = "";
                string id_solicitud = hdnIdSolicitud.Value;
                string[] valoresCarac = hdnCaracteristicasValores.Value.Split(';');
                string[] labelsCarac = hdnCaracteristicasTitulo.Value.Split(';');
                int caracConValores = 0;
                ConsultasDB consultas = new ConsultasDB();
                DataSet dsCaracteristicas = new DataSet();

                string telef_contacto = txtTelefono.Text;
                string Script = "";

                // Kintell R#33775
                if (hdnInspeccion.Value == "1")
                {
                    // COMPROBAR QUE LA FECHA AGENDAMIENTO SEA INFERIOR A LA FECHA DE HOY R#33775
                    //  “Data de Agendamento anterior ao dia de hoje. Por favor seleccionar nova data!”
                    for (int j = 0; j < valoresCarac.Length - 1; j++)
                    {
                        //ConsultasDB consultas=new ConsultasDB();
                        string descripCarac = labelsCarac[j].ToString();
                        //string tipCar = "162";// consultas.ObtenerCaracteristicaPorDescripcion(descripCarac, (Int16)usuarioDTO.IdIdioma);
                        string valor = valoresCarac[j].ToString();

                        if (descripCarac == "Fecha Prevista." || descripCarac == "Data Marcada.")
                        {
                            if (DateTime.Parse(valor) < DateTime.Now)
                            {
                                MostrarMensaje(Resources.TextosJavaScript.DATA_AGENDAMIENTO_ANTERIOR);

                                MasterPageModal mpm1 = (MasterPageModal)this.Master;
                                string script1 = "<script>window.parent.refrescar();</script>";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "REFRESCAR", script1, false);
                                mpm1.CerrarVentanaModal();
                                return;

                            }
                        }

                    }
                }
                
                //20210505 BRU R#29497 - Guardar el Ticket de combustión Fase 6, datos por Web
                PValidacionesTicketCombustion pValidacionesTicketCombustion = new PValidacionesTicketCombustion();
                string msgErrorTicket = string.Empty;
                int estadoCambiaVisita = int.Parse(cod_estado);

                //Comprobamos si tiene que tener ticket de combustion
                Boolean isDebeTenerTicket = pValidacionesTicketCombustion.HayQueProcesarTicketCombustionDesdeWEB_Solicitud(usuarioDTO
                                                                                                                    , hdnCodContrato.Value
                                                                                                                    , id_solicitud
                                                                                                                    , hdnSubtipo.Value
                                                                                                                    , estadoCambiaVisita
                                                                                                                    , ref msgErrorTicket);

                //Antes de guardar verificamos si le corresponde Ticket de combustion. De ser obligatorio mostramos la pantalla para insertar los datos.
                if (isDebeTenerTicket)
                {
                    String strUrl = "./FrmModalTicketCombustion.aspx" + "?COD_CONTRATO=" + hdnCodContrato.Value.ToString() + "&ID_SOLICITUD=" + id_solicitud + "&EDICION=true";
                    this.OpenWindow(strUrl, "ModalTicketCombustion", "Ticket de combustion");
                    
                    CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, ddl_EstadoSol.SelectedValue);

                    return;
                }
                //END

                // Kintell 07/07/2020 Pongo esto aqui, porque si no, lo que esta pasando es que crea la solicitud sin las características a pesar de tener el teléfono mal.
                telef_contacto = telef_contacto.Replace(" ", "");
                if (!String.IsNullOrEmpty(telef_contacto))
                {
                    //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    PhoneValidator telefVal = new PhoneValidator();
                    if (!telefVal.Validate(telef_contacto))
                    {
                        Script = "<script language='Javascript'>alert('" + Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO + "');</script>";
                    }
                    else
                    {
                        // Recording the contact phone number...
                        SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                        objSolicitudesDB1.EjecutarSentencia("UPDATE SOLICITUDES SET telefono_contacto='" + telef_contacto + "' WHERE ID_SOLICITUD='" + id_solicitud + "'");
                    }

                    //if (telef_contacto.Length != 12)
                    //{
                    //    Script = "<script language='Javascript'>alert('" + Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_TENER_12_DIGITOS + "');</script>";
                    //}
                    //else
                    //{
                    //    if (telef_contacto.Substring(0, 3) != "+34" && telef_contacto.Substring(0, 4) != "+351")
                    //    {
                    //        Script = "<script language='Javascript'>alert('" + Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO + "');</script>";
                    //    }
                    //    if (telef_contacto.Substring(3, 1) == "7" || telef_contacto.Substring(3, 1) == "6" || telef_contacto.Substring(3, 1) == "9" || telef_contacto.Substring(3, 1) == "8" || telef_contacto.Substring(3, 1) == "2" || telef_contacto.Substring(3, 1) == "3")
                    //    {
                    //        if (id_solicitud.ToString() != "0")
                    //        {
                    //            // Recording the contact phone number...
                    //            SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                    //            objSolicitudesDB1.EjecutarSentencia("UPDATE SOLICITUDES SET telefono_contacto='" + telef_contacto.Substring(0, 12) + "' WHERE ID_SOLICITUD='" + id_solicitud + "'");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Script = "<script language='Javascript'>alert('" + Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_COMENZAR_6789 + "');</script>";
                    //    }
                    //}
                    //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                }
                else
                {
                    if (hdnSubtipo.Value != "006")
                    {
                        Script = "<script language='Javascript'>alert('" + Resources.TextosJavaScript.TEXTO_TELEFONO_OBLIGATORIO + "');</script>";
                    }
                }
                if (Script != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ERROR", Script, false);
                    if (hdnSubtipo.Value == "008" && cod_estado == "-1") { cod_estado = "034"; }
                    //CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                    //if (hdnAltaIncidenciaGasConfort.Value == "0")
                    //{
                    CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                    //}
                    //else
                    //{
                    //  // (21/08/2015) Si venimos de la creación de avería de gas confort, tenemos que cargar sus características.
                    //CargaCaracteristicasPorTipoSolicitud("001", "012", "065");
                    //}
                    return;
                }
                Script = "";
                // (20/08/2015) Comprobamos si lo que tenemos que hacer es dar de alta una avería de gas confort o modificar una solicitud ya existente.
                if (hdnAltaIncidenciaGasConfort.Value == "0" && hdnInspeccion.Value=="0")
                {
                    // We catch the characteristics and here we treat (tratar) them...
                    for (int i = 0; i < valoresCarac.Length; i++)
                    {
                        string test = labelsCarac[i].ToString();
                        Boolean Obligatorio = false;
                        if (test != "")
                        {
                            Obligatorio = consultas.ObtenerObligatoriedadPorDescripcion(test,(Int16)usuarioDTO.IdIdioma);
                        }
                        // if the characteristic required is a date, we check that it has a correct value...
                        if (test.ToUpper().IndexOf("FECHA") >= 0
                            || test.ToUpper().IndexOf("DATA") >= 0)
                        {
                            string valorATestear = valoresCarac[i].ToString();
                            DateTime d;
                            if (Obligatorio || valorATestear != "")
                            {
                                if (!DateTime.TryParseExact(valorATestear, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                                {
                                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_COMPRUEBE_FECHA_INTRODUCIDA);//"Compruebe la fecha introducida.");
                                    CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                                    // Reload the values of the caracteristics.
                                    for (int j = valoresCarac.Length - 1; j >= 0; j--)
                                    {
                                        int valor = (j - (valoresCarac.Length - 1));
                                        if (valor != 0) { valor = valor * (-1); }
                                        TextBox a = (TextBox)tblCaracteristicas.Rows[valor].Cells[1].Controls[0];
                                        a.Text = valoresCarac[j - 1];
                                    }
                                    return;
                                }
                                // Reparation date must be less than today.
                                if (test.ToUpper().IndexOf("REPARACIÓN") >= 0 || test.ToUpper().IndexOf("FECHA INSTALACIÓN") >= 0
                                    || test.ToUpper().IndexOf("reparação".ToUpper()) >= 0 || test.ToUpper().IndexOf("Data da instalação".ToUpper()) >= 0)
                                {
                                    if (DateTime.Parse(valorATestear) > DateTime.Now)
                                    {
                                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_FECHA_INTRODUCIDA_NO_PUEDE_SER_MAYOR);//"Compruebe la fecha introducida. No puede ser mayor a la fecha de hoy.");
                                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                                        // Reload the values of the caracteristics.
                                        for (int j = valoresCarac.Length - 1; j >= 0; j--)
                                        {
                                            int valor = (j - (valoresCarac.Length - 1));
                                            if (valor != 0) { valor = valor * (-1); }
                                            TextBox a = (TextBox)tblCaracteristicas.Rows[valor].Cells[1].Controls[0];
                                            a.Text = valoresCarac[j - 1];
                                        }
                                        return;
                                    }
                                }

                                //R#20106 La fecha de instalación no tiene que ser inferior a 7 dias desde hoy.
                                if (test.ToUpper().IndexOf("FECHA INSTALACIÓN") >= 0 || test.ToUpper().IndexOf("Data da instalação".ToUpper()) >= 0)
                                {
                                    if (DateTime.Parse(valorATestear) <= DateTime.Now.AddDays(-7))
                                    {
                                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_FECHA_INTRODUCIDA_NO_PUEDE_SER_MENOR_A7DIAS_ATRAS);//"Compruebe la fecha introducida. No puede ser mayor a la fecha de hoy.");
                                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                                        // Reload the values of the caracteristics.
                                        for (int j = valoresCarac.Length - 1; j >= 0; j--)
                                        {
                                            int valor = (j - (valoresCarac.Length - 1));
                                            if (valor != 0) { valor = valor * (-1); }
                                            TextBox a = (TextBox)tblCaracteristicas.Rows[valor].Cells[1].Controls[0];
                                            a.Text = valoresCarac[j - 1];
                                        }
                                        return;
                                    }
                                }

                                // 24/02/2016 - Comentamos a petición de Edurne, mail del 24/02/2016 a las 8:48.
                                //// Comprobamos que la fecha prevista de instalación sea mayor o igual a la fecha de hoy.
                                //if (test.ToUpper().IndexOf("PREVISTA INSTALACIÓN") >= 0)
                                //{
                                //    if (DateTime.Parse(valorATestear) < DateTime.Now)
                                //    {
                                //        MostrarMensaje("Compruebe la fecha introducida. No puede ser menor a la fecha de hoy");
                                //        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                                //        // Reload the values of the caracteristics.
                                //        for (int j = valoresCarac.Length - 1; j >= 0; j--)
                                //        {
                                //            int valor = (j - (valoresCarac.Length - 1));
                                //            if (valor != 0) { valor = valor * (-1); }
                                //            TextBox a = (TextBox)tblCaracteristicas.Rows[valor].Cells[1].Controls[0];
                                //            a.Text = valoresCarac[j - 1];
                                //        }
                                //        return;
                                //    }
                                //}
                            }
                        }
                        // if the characteristic required is an intenger, we check that it has a correct value...
                        if (test.ToUpper().IndexOf("IMPORTE") >= 0
                            || test.ToUpper().IndexOf("MONTANTE") >= 0)
                        {
                            try
                            {
                                string valorATestear = valoresCarac[i].ToString();
                                Double a = Double.Parse(valorATestear);
                            }
                            catch (Exception ex)
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_COMPRUEBE_IMPORTE_INTRODUCIDO);//"Compruebe el Importe introducido.");
                                CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                                // Reload the values of the caracteristics.
                                for (int j = valoresCarac.Length - 1; j >= 0; j--)
                                {
                                    int valor = (j - (valoresCarac.Length - 1));
                                    if (valor != 0) { valor = valor * (-1); }
                                    TextBox a = (TextBox)tblCaracteristicas.Rows[valor].Cells[1].Controls[0];
                                    a.Text = valoresCarac[j - 1];
                                }
                                return;
                            }

                        }

                        // Obtain the Id of the required fields...
                        if (test != "")
                        {
                            if (Obligatorio)
                            {
                                if (valoresCarac[i].ToString() != "")
                                {
                                    caracConValores += 1;
                                }
                            }
                            else { caracConValores += 1; }
                        }

                    }
                }
                else
                {
                    //if (hdnInspeccion.Value == "0")
                    //{
                        caracConValores = 0;
                        // (21/08/2015) Tema avería gas confort.
                        // Miramos cuales son obligatorias.
                        for (int i = 0; i < valoresCarac.Length; i++)
                        {
                            string test = labelsCarac[i].ToString();
                            Boolean Obligatorio = false;
                            if (test != "")
                            {
                                Obligatorio = consultas.ObtenerObligatoriedadPorDescripcion(test, (Int16)usuarioDTO.IdIdioma);
                                // Comprobamos si es obligatorio.
                                if (Obligatorio)
                                {
                                    if (valoresCarac[i].ToString() != "")
                                    {
                                        // Si tiene valor sumamos una a las informadas.
                                        caracConValores += 1;
                                    }
                                }
                                else
                                {
                                    // Si no es obligatorio tambiçen sumamos una porque no vamos a exigir que tenga valor informado.
                                    caracConValores += 1;
                                }
                            }
                        }
                    //}
                    //else
                    //{
 
                    //}
                }


                // We must to check if all of the characteristics are informed...
                if (caracConValores != valoresCarac.Length - 1)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_SE_DEBEN_RELLENAR_CARACTERISTICAS_OBLIGATORIAS);//"Se deben rellenar todas las características Obligatorias.");
                    if (hdnSubtipo.Value == "008" && cod_estado == "-1") { cod_estado = "034"; }
                    if (hdnSubtipo.Value == "016" && cod_estado == "-1") { cod_estado = "069"; }
                    //if (hdnAltaIncidenciaGasConfort.Value == "0")
                    //{
                    //CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                    CargaCaracteristicasPorTipoSolicitudCambioMotivoCancelacion("001", hdnSubtipo.Value, ddl_MotivoCancel.SelectedValue, cod_estado);
                    //}
                    //else
                    //{
                    //  // (21/08/2015) Si venimos de la creación de avería de gas confort, tenemos que cargar sus características.
                    //CargaCaracteristicasPorTipoSolicitud("001", "012", "065");
                    //}
                    return;
                }


                ConfiguracionDTO confObligatorioFichero = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.OBLIGATORIO_FICHERO_GC);
                Boolean obligatorioFichero = false;

                if (confObligatorioFichero != null && !string.IsNullOrEmpty(confObligatorioFichero.Valor) && Boolean.Parse(confObligatorioFichero.Valor))
                {
                    obligatorioFichero = Boolean.Parse(confObligatorioFichero.Valor);
                }

                // Kintell fichero GC, contrato GC, 11/10/2019.
                if (FileGC.Visible)
                {
                    if (String.IsNullOrEmpty(FileGC.FileName.ToString()) && obligatorioFichero)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NOMBREFICHERO_INCORRECTO);
                        return;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(FileGC.FileName.ToString()))
                        {
                            if (!GuardarFicheroGC())
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_NOMBREFICHERO_INCORRECTO);
                                return;
                            }
                        }
                    }
                }

                // 28/08/2019 KINTELL, Guardamos los ficheros de inspeccion si los hay.
                if (!GuardarFicheroInspeccion())
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NOMBREFICHERO_INCORRECTO);
                    return;
                }

                for (int contCaracteristicas = 0; contCaracteristicas < valoresCarac.Length - 1; contCaracteristicas++)
                {
                    //ConsultasDB consultas=new ConsultasDB();
                    string descripCarac = labelsCarac[contCaracteristicas].ToString();
                    // Obtain the Id of the characteristic...
                    string tipCar = consultas.ObtenerCaracteristicaPorDescripcion(descripCarac,(Int16)usuarioDTO.IdIdioma);
                    string valor = valoresCarac[contCaracteristicas].ToString();

                    // Tema datos bancarios, comprobar que los datos son buenos.
                    Boolean bDatosBancarios = true;
                    if (tipCar == "152")
                    {
                        Commons.Validations.IbanValidator cIban = new Commons.Validations.IbanValidator();
                        bDatosBancarios = cIban.Validate(valor);
                        contCaracteristicas = valoresCarac.Length - 1;
                    }
                    if (bDatosBancarios == false)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_COMPRUEBE_DATOS_BANCARIOS);//"Compruebe los datos bancarios.");
                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                        return;
                    }

                    /*
                    //20180531 BGN: Comentamos esta validación que ya se hace previamente en el campo, 
                    //ademas para los termstaros inteligentes Honeywell la validación es distinta.
                    Boolean bNumeroSerieTermostatoInteligente = true;
                    if (tipCar == "092")
                    {
                        // Comprobar que la primera letra sea una G.
                        if (valor.ToUpper().Substring(0, 1) != "G")
                        {
                            bNumeroSerieTermostatoInteligente = false;
                        }
                        // Que sean 7 digitos.
                        if (valor.Length != 7)
                        {
                            bNumeroSerieTermostatoInteligente = false;
                        }
                        contCaracteristicas = valoresCarac.Length - 1;
                    }
                    if (bNumeroSerieTermostatoInteligente == false)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_COMPRUEBE_NUMERO_SERIE_TERMOSTATO);//"Compruebe el número de serie del Termostato Inteligente.");
                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                        return;
                    }
                    */
                }

                if (hdnInspeccion.Value == "1")
                {
                    string contrato = hdnCodContrato.Value;
                    SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                    ProveedoresDB objProveedoresDB = new ProveedoresDB();
                    string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo("001", "016", contrato).Split(';');
                    string proveedor = Proveedores[0].ToString().Substring(0, 3);

                    if (hdnInspeccionExtra.Value.Equals("1"))
                    {
                        id_solicitud = objSolicitudesDB1.AddSolicitud(contrato, "001", "016", "069", telef_contacto, "", "", txt_Observaciones.Text, proveedor, false, false, true).ToString();
                    }
                    else
                    {
                        id_solicitud = objSolicitudesDB1.AddSolicitud(contrato, "001", "016", "069", telef_contacto, "", "", txt_Observaciones.Text, proveedor, false, false, false).ToString();
                    }
                }
                //if (hdnSubtipo.Value == "008")
                //{
            
                //}

                // We check that if the selected state is cancelada a motiv for cancellation has been selected ...
                if (ddl_EstadoSol.SelectedItem.Text == Resources.TextosJavaScript.TEXTO_CANCELADA)//"Cancelada")
                {
                    if (this.ddl_MotivoCancel.SelectedValue == "-1" && hdnSubtipo.Value != "008" && hdnSubtipo.Value != "017")
                    {
                        // (21/08/2015) Tema avería gas confort, comprobamos que no es la creación de una averia de gas confort.
                        //if (hdnAltaIncidenciaGasConfort.Value == "0")
                        //{
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_MOTIVO_CANCELACION);//"Debe seleccionar un motivo de cancelación.");
                        CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);
                        
                        return;
                        //}
                    }
                }

                // We build the observations.
                string Observaciones;
                string Horas = DateTime.Now.Hour.ToString();
                if (Horas.Length == 1) Horas = "0" + Horas;
                string Minutos = DateTime.Now.Minute.ToString();
                if (Minutos.Length == 1) Minutos = "0" + Minutos;
                
                String usuario = usuarioDTO.Login;
                string observ_finales = "[" + DateTime.Now.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + txt_Observaciones.Text;
                Observaciones = observ_finales + (char)(13) + txt_ObservacionesAteriores.Text;

                // We change the state, if everything is allright, if the state has been changed, of course...
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                HistoricoDB objHistoricoDB = new HistoricoDB();

                int id_movimiento = 0;
                // (21/08/2015) Tema avería gas confort.
                // if (hdnAltaIncidenciaGasConfort.Value == "0")
                //{
                if (ddl_EstadoSol.SelectedValue != "-1")
                {
                    //2094:Pago Anticipado
                    string cod_estadoDll = ddl_EstadoSol.SelectedValue;

                    //2094: Pago Anticipado
                    // Si el estado seleccionado es Reparada o Reparada por Telefono
                    if (cod_estadoDll.Equals("010") || cod_estadoDll.Equals("011"))
                    {
                        //Actualizamos el estado y restamos 1 al contador de averias, si la averia es en el Equipo.
                        objSolicitudesDB.UpdateEstadoSolicitud(id_solicitud, ddl_EstadoSol.SelectedValue, int.Parse(dll_tipLugarAveria.SelectedValue));

                    }
                    else
                    {
                        //Metemos un valor 3, que hemos definido en la tabla como un N/A
                        objSolicitudesDB.UpdateEstadoSolicitud(id_solicitud, ddl_EstadoSol.SelectedValue, (int)Enumerados.TipoLugarAveria.NA);
                    }


                }
                // If it has a cancellation motif, we report it, on the data base...
                if (ddl_MotivoCancel.Enabled == true)
                {
                    if (ddl_MotivoCancel.SelectedIndex != -1)
                    {
                        string mot_cancel = ddl_MotivoCancel.SelectedValue;
                        objSolicitudesDB.UpdateSolicitudDesdeProveedor(id_solicitud, mot_cancel);
                    }
                }

                // Recording the historic...j
                //18/12/2017 BGN Capa Negocio actualizar historico para incluir llamada WS Alta Interaccion
                //id_movimiento = objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, ddl_EstadoSol.SelectedValue, Observaciones, "");
                Solicitud soli = new Solicitud();
                string cod_contrato = string.Empty;
                string cod_estado_sol = string.Empty;
                string cod_averia = string.Empty;
                string cod_visita = "0";
                DataSet ds = objSolicitudesDB.GetSolicitudesPorIDSolicitud(id_solicitud, 1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cod_contrato = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    cod_estado_sol = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                    cod_averia = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                }
                string tipoMovimiento = "002";
                string tipoOperacion = "M";
                //20180507 BGN: Estado 069 es el estado inicial de Inspeccion Gas
                if ((hdnInspeccion.Value == "1") && (cod_estado_sol.Equals("069")))
                {
                    tipoMovimiento = "001";
                    tipoOperacion = "A";
                }
                id_movimiento = soli.ActualizarHistoricoSolicitud(cod_contrato, id_solicitud, tipoMovimiento, usuario, cod_estado_sol, observ_finales, cmbProveedor.SelectedValue, hdnSubtipo.Value, cod_averia, cod_visita, tipoOperacion);

                //Comprobamos si el estado de la solicitud es un estado final de los parametrizados que incorporan el ticket de combustion, miramos si tiene una visita asociada en estado 13 para cerrarla.
                PValidacionesTicketCombustion.CerrarVisitaEnEstadoErroneaCuandoSolicitudResuelta(decimal.Parse(id_solicitud), hdnSubtipo.Value, cod_estado_sol, usuario);

                if ((hdnInspeccion.Value == "1") && (cod_estado_sol.Equals("069")))
                {
                    //Obtenemos el CUPS del mantenimiento
                    IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(hdnCodContrato.Value);
                    string CUPS = string.Empty;
                    if (datosMantenimiento != null)
                    {
                        while (datosMantenimiento.Read())
                        {
                            CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "CUPS");
                        }
                    }
                    //Añadir Caracteristica CUI con el valor del CUPS
                    int id_caracteristica_CUI = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), "160", CUPS, DateTime.Now.ToShortDateString());
                    // Add the hystoric of the movement...
                    objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_CUI, id_movimiento.ToString(), "160", CUPS);
                }
                //}
                //else
                //{
                //    // (21/08/2015) Tema avería gas confort, si es avería gas confort tenemos que dar de alta la solicitud en vez de actualizar una ya existente.
                //    id_solicitud = objSolicitudesDB.AddSolicitudAveriaGasConfort(hdnContrato.Value, hdnSubtipoAveriaSobreGC.Value).ToString();
                //    id_movimiento = objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, "001", Observaciones, "");
                //}
                if (hdnInspeccion.Value == "1")
                {
                    //Kintell Campaña R#29209
                    if (hdnCampania.Value != "")
                    {
                        CaracteristicasDB objCaracteristicasCampaniaDB = new CaracteristicasDB();
                        HistoricoDB objHistoricoCampaniaDB = new HistoricoDB();
                        DateTime fechaHora = DateTime.Now;

                        int idCaracteristica = objCaracteristicasCampaniaDB.AddCaracteristica(id_solicitud.ToString(), "187", hdnCampania.Value, fechaHora.ToShortDateString());
                        // Add the hystoric of the movement...
                        objHistoricoCampaniaDB.AddHistoricoCaracteristicas(idCaracteristica, id_movimiento.ToString(), "187", hdnCampania.Value);
                    }
                }


                // Recording the observations...
                if (txt_Observaciones.Text != "") { objSolicitudesDB.UpdateObservacionesSolicitud(id_solicitud, Observaciones); }

                // Update the caldera...
                if (!String.IsNullOrEmpty(hdnMarcaCaldera.Value.ToString()))
                {

                    objSolicitudesDB.ActualizarCalderaContrato(hdnContrato.Value, int.Parse(hdnMarcaCaldera.Value.ToString()), txt_ModeloCaldera.Text, "", "", 0, 0);
                }

                // Update the contact timetable...
                objSolicitudesDB.EjecutarSentencia("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" + cmbHorarioContacto.SelectedValue + "' WHERE COD_CONTRATO_SIC='" + hdnContrato.Value + "'");




                // Here we start the process of the characteristics....
                // First of all we will remove every one of the characteristics from the solicitud...
                string Sql = "Update caracteristicas_solicitud set Activo=0 where id_solicitud=" + id_solicitud;
                objSolicitudesDB.EjecutarSentencia(Sql);
                // On the second step we must find the ID of the characteristic and add the characteristics to the solicitud...
                for (int j = 0; j < valoresCarac.Length - 1; j++)
                {
                    //ConsultasDB consultas=new ConsultasDB();
                    string descripCarac = labelsCarac[j].ToString();
                    // Obtain the Id of the characteristic...
                    string tipCar = consultas.ObtenerCaracteristicaPorDescripcion(descripCarac,(Int16)usuarioDTO.IdIdioma);
                    string valor = valoresCarac[j].ToString();
                    // Insert the new characteristic of the "solicitud"...
                    if (valor == "on" || valor == "true") { valor = "Si"; }
                    if (valor == "off" || valor == "false") { valor = "No"; }

                    //// *****************************************************************************************
                    //// Kintell 26/02/2014 Espejo estados cracteristicas.
                    //// ver si es una solicitud del estado seleccionado o si lo es de estados espejo.
                    //Boolean activo = Boolean.Parse(consultas.ObtenerActivoCaracteristicaPorDescripcion(descripCarac, hdnSubtipo.Value, cod_estado, id_solicitud));
                    //if (activo)
                    //{
                    if (tipCar.ToString() == "084")
                    {
                        modoPago = valor;
                    }
                    if (tipCar.ToString() == "074")
                    {
                        cuota = valor;
                    }

                    if (tipCar.ToString() == "165" || tipCar.ToString() == "166")
                    {
                        string[] nombreFichero = valor.Split('\\');
                        valor = nombreFichero[nombreFichero.Length - 1].Trim();

                        if (!String.IsNullOrEmpty(valor))
                        {
                            int id_caracteristica = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), tipCar.ToString(), valor, DateTime.Now.ToShortDateString());
                            // Add the hystoric of the movement...
                            objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento.ToString(), tipCar.ToString(), valor);
                        }
                    }
                    else
                    {
                        int id_caracteristica = objCaracteristicasDB.AddCaracteristica(id_solicitud.ToString(), tipCar.ToString(), valor, DateTime.Now.ToShortDateString());
                        // Add the hystoric of the movement...
                        objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica, id_movimiento.ToString(), tipCar.ToString(), valor);
                    }

                    //}
                    // *****************************************************************************************
                }

                objCaracteristicasDB.RestarContadorAveriaSegunLugarAveria(int.Parse(id_solicitud.ToString()));
                //TWITTER
                //06/09/2013
                if (imgTwitter.Visible == true && chkTwitter.Checked == true)
                {
                    EnviarAviso(id_solicitud.ToString());
                }

                //FACEBOOK
                //11/03/2015
                if (imgFacebook.Visible == true && chkFacebook.Checked == true)
                {
                    EnviarAvisoFacebook(id_solicitud.ToString());
                }
                // CAMBIO PROVEEDOR
                // 20/02/2014
                // (21/08/2015) Tema avería gas confort, cambiamos el proveedor si no es un alta de avería de gas confort.
                if (cmbProveedor.Visible == true)//&& hdnAltaIncidenciaGasConfort.Value == "0")
                {
                    objSolicitudesDB.UpdateProveedorSolicitud(id_solicitud, cmbProveedor.SelectedValue.ToString());
                }


                string sMarca = "";
                string sModelo = "";
                // Si es gas confort y pasa al estado Instalacion Realizada, debemos salvar las caracteristicas seleccionadas a Calderas.
                if (((hdnSubtipo.Value == "008") && (cod_estado == "057")) || ((hdnSubtipo.Value == "008") && (cod_estado == "060")))
                {
                    dsCaracteristicas = objCaracteristicasDB.GetMarcaModeloBySolicitud(hdnIdSolicitud.Value);

                    DataRow[] drMarca = dsCaracteristicas.Tables[0].Select("[" + dsCaracteristicas.Tables[0].Columns["Tip_Car"].ColumnName + "]" + " = " + "'" + "072" + "'");


                    // Cogemos el valor de la marca.
                     sMarca = drMarca[0].ItemArray.GetValue(3).ToString();


                    // Cogemos el valor del modelo.
                    DataRow[] drModelo = dsCaracteristicas.Tables[0].Select("[" + dsCaracteristicas.Tables[0].Columns["Tip_Car"].ColumnName + "]" + " = " + "'" + "073" + "'");
                     sModelo = drModelo[0].ItemArray.GetValue(3).ToString();

                    // Actualizamos la tabla CALDERAS con estos valores, para el contrato correspondiente.
                    objSolicitudesDB.ActualizarCalderaMarcaModeloByContrato(hdnContrato.Value, sMarca, sModelo);
                }

                //20201210 BEG [R#28313] Cambio Flujo Gas Confort por digitalización contrato
                //Si se ha adjuntado el Contrato de GC manualmente tenemos que añadir la cracteristica 081 con el CCBB del documento
                if (FileGC.Visible && !String.IsNullOrEmpty(FileGC.FileName))
                {
                    //Guardar la referencia del CCBB del documento en la caracteristica 081
                    string CCBB = ObtenerCCBBDocumento(FileGC.FileName);
                    int id_caracteristica_CCBB = objCaracteristicasDB.AddCaracteristica(hdnIdSolicitud.Value, "081", CCBB, DateTime.Now.ToShortDateString());
                    objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_CCBB, id_movimiento.ToString(), "081", CCBB);
                }

                //Si es Gas Confort y pasa al estado 'Oferta Presentada Pdte. Firma' (080), si tenemos informadas las características de Email, tenemos que generar el contrato de GC y mandarlo a EDatalia
                if ((hdnSubtipo.Value == "008") && (cod_estado == "080"))
                {​​
                    //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                    string email = CaracteristicaHistorico.GetCaracteristicaValor(Int32.Parse(hdnIdSolicitud.Value), "185");
                    if (!String.IsNullOrEmpty(email))
                    {
                        GenerarPDF(email, id_movimiento.ToString()); // string nombreFichero = PCamposPdfContratoGasConfort.GenerarContratoGasconfort(this.hdnContrato.Value, Convert.ToDecimal(id_solicitud));                                      
                    }
                    //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
                }​​
                //20201210 END [R#28313] Cambio Flujo Gas Confort por digitalización contrato


                // (21/08/2015) Tema avería gas confort, vemos que mensaje sacar teniendo en cuenta si es un alta de avería de gas confort o no.
                // if (hdnAltaIncidenciaGasConfort.Value == "0")
                //{
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_MODIFICADA);//"Se ha modificado la solicitud");
                //}
                //else
                //{
                //  MostrarMensaje("Se ha dado de alta la solicitud: " +  id_solicitud);
                //}


                //else 
                //{ 
                //    // (20/08/2015) Damos de alta una avería de gas confort.

                //    MostrarMensaje("Solicitud dada de alta");
                //}



                // **************************
                // 14/12/2015 Si el estado pasa a oferta aceptada y esta seleccionado el pago por adelantado, mandamos mail
                if (ddl_EstadoSol.SelectedValue.ToString() == "060" && modoPago == "Pago Anticipado")
                {
                    MantenimientoDB manDB = new MantenimientoDB();
                    if (manDB.Get_EmailGCEnviados(int.Parse(id_solicitud)) == string.Empty)
                    {
                        IDataReader Datos = Mantenimiento.ObtenerAvisoSolicitud(id_solicitud);
                        String Destino = "";
                        while (Datos.Read())
                        {
                            Destino = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "email_aviso");
                        }

                        //ICISA:

                        //Quiroga, Jordi (Barcelona) (Jordi.Quiroga@sgs.com) 

                        //SIEL:

                        //Carlos Gomez Hernandez (resp_averias@sielsl.es) 

                        //ACTIVAIS:           

                        //Jose.Atajagueces@activais.es 

                        //MAPFRE:

                        //oscar.maroto@msmultimap.es

                        //gas@msmultimap.es 


                        string contrato = ObtenerContratoSolicitud();

                        //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web (quitamos a Alex para meter a Julene y quitamos las de gfi por el buzon de soporte)
                        string destinatarios = Destino;
                        
                        // + ";cobro@iberdrola.es;jpascualma@iberdrola.es;maquintela@gfi.es;rlopezh@iberdrola.es;eaceves@gfi.es;a.lazaro@iberdrola.es;jmiquelajauregui@iberdrola.es;bgnieto@gfi.es;jmarced@iberdrola.es;";
                        ConfiguracionDTO confDireccion = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CUENTA_CORREO_GC_PAGO_ADELANTADO);
                        if (confDireccion != null && !string.IsNullOrEmpty(confDireccion.Valor))
                        {
                            destinatarios += "," + confDireccion.Valor;
                        }
                        //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web

                        //25/09/2017 Petición de Alex.
                        IDataReader drDatosContrato = ObtenerDatosContratoPorSolicitud();
                        if (drDatosContrato.Read())
                        {
                            string strNombre = (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "NOM_TITULAR") + " " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "APELLIDO1") + " " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "APELLIDO2");
                            string strDNI = (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "DNI");
                            string strDireccion = (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "TIP_VIA_PUBLICA") + " " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "NOM_CALLE") + " " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "COD_FINCA") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "TIP_BIS") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "COD_PORTAL") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "COD_FINCA") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "TIP_ESCALERA") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "TIP_PISO") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "TIP_MANO") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "NOM_POBLACION") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "COD_POSTAL") + " - " + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "NOM_PROVINCIA");

                            StringBuilder strTexto = new StringBuilder();
                            strTexto.Append("<br><br>");
                            strTexto.Append(Resources.TextosJavaScript.TEXTO_LA_SOLICITUD + id_solicitud + Resources.TextosJavaScript.TEXTO_DEL_CONTRATO + contrato + Resources.TextosJavaScript.TEXTO_CON_CUOTA + cuota);
                            strTexto.Append("<br><br>");
                            strTexto.Append("Marca:" + sMarca);
                            strTexto.Append("<br><br>");
                            strTexto.Append("Modelo:" + sModelo);
                            strTexto.Append("<br><br>");
                            strTexto.Append("Nombre Cliente:" + strNombre);
                            strTexto.Append("<br><br>");
                            strTexto.Append("DNI Cliente:" + strDNI);
                            strTexto.Append("<br><br>");
                            strTexto.Append("Dirección Cliente:" + strDireccion);
                            strTexto.Append("<br><br>");
                            strTexto.Append("Proveedor:" + (string)DataBaseUtils.GetDataReaderColumnValue(drDatosContrato, "Proveedor"));
                            strTexto.Append("<br><br>");
                            strTexto.Append("Avisarnos por favor cuando realice el pago y generar la factura correspondiente");
                            strTexto.Append("<br><br>");
            
                            //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                            //MandarMailGC(strTexto.ToString(), Resources.TextosJavaScript.TEXTO_NUEVO_PAGO_ADELANTADO + contrato, destinatarios, true);
                            bool enviado = UtilidadesMail.EnviarAviso(Resources.TextosJavaScript.TEXTO_NUEVO_PAGO_ADELANTADO + contrato, strTexto.ToString(), "rlopezh@iberdrola.es", destinatarios);

                            if (enviado)
                            {
                                Int64 ok = manDB.Insertar_EmailGCEnviados(int.Parse(id_solicitud));
                            }
                            else
                            {
                                Int64 ok = manDB.Insertar_EmailGC_NoEnviado(int.Parse(id_solicitud));
                            }

                            //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                        }
                    }
                }
                // **************************



                MasterPageModal mpm = (MasterPageModal)this.Master;
                string script = "<script>window.parent.refrescar();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "REFRESCAR", script, false);
                mpm.CerrarVentanaModal();
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_NO_MODIFICADA);//);//"No se ha podido modificar la solicitud.");
            }
            finally
            {
                //Response.Redirect("Proveedores.aspx#PosicionModificar")
                String Script = "<script language='javascript'>OcultarCapaEspera();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);
            }
        }
        
        private Boolean ComprobarNombreFichero(String nombre)
        {
            //14/03/2018 Nomenclatura documentos
            //CodContrato_AAAAMMDD_proveedor_TipoDocumento.extension
            //CCCCCCCCCC_AAAAMMDD_PRO_DOC.PDF/TIF
            string[] sNombreFichero = nombre.Split('_');
             
            if (!String.IsNullOrEmpty(nombre))
            {
                // Kintell 21/05/2019 Exigir fichero en GC, con nomenclatura de: CCBB_contrato.pdf.
                // Petición redmine #17248.
                if (int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.GasConfort
                    || int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
                    || int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.InstalacionTermostatoInteligente)
                {
                    // COMPROBAMOS QUE TENGA 2 PARAMETROS EN EL NOMBRE.
                    if (sNombreFichero.Length == 2)
                    {
                        string codContratoSinExtension = sNombreFichero[1].ToString();
                        int posicionPunto = codContratoSinExtension.IndexOf(".");
                        codContratoSinExtension = codContratoSinExtension.Substring(0, posicionPunto);

                        // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                        if (codContratoSinExtension.Length == 10)
                        {
                            //20201008 Comprobar que el prametro del CCBB no sea CCBB
                            string ccbb = sNombreFichero[0].ToString();
                            if (ccbb.ToUpper().Equals("CCBB"))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Kintell 06072020 Las de inspección no tienen gestión documental, por lo que no comprobamos nombre.
                    return true;
                    // COMPROBAMOS QUE TENGA 4 PARAMETROS EN EL NOMBRE.
                    if (sNombreFichero.Length == 4)
                    {
                        // COMPROBAMOS QUE EL PARAMETRO DEL CONTRATO TENGA 10 CARACTERES.
                        if (sNombreFichero[0].Length == 10)
                        {
                            // COMPROBAMOS QUE EL PARAMTERO DEL TIPO DEL DOCUMENTO SEA ALGUNO DE LOS ACEPTADOS.
                            string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                            int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");
                            TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);
                            int idTipoDocumento = Documento.ObtenerIdTipoDocumento(TipoDocumentoSinExtension);
                            if (idTipoDocumento > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private string ObtenerContratoSolicitud()
        {
            IDataReader dr = null;

            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = null;
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                SolicitudesDB solDB = new SolicitudesDB();
                dr = solDB.GetSingleSolicitudes(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);

                if (dr.Read())
                {
                    return (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
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

        private IDataReader ObtenerDatosContratoPorSolicitud()
        {
            IDataReader dr = null;

            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = null;
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                SolicitudesDB solDB = new SolicitudesDB();
                
                dr = solDB.GetSingleSolicitudes(hdnIdSolicitud.Value, usuarioDTO.IdIdioma);
                return dr;
            }
            catch
            {
                throw;
            }
            finally
            {
                //if (dr != null)
                //{
                //   // dr.Close();
                //}
            }
        }

        private void CargaCaracteristicasSolicitud(string id_solicitud, string Subtipo)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
            DataSet dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitud(id_solicitud,usuarioDTO.IdIdioma);
            DataSet dsCaracteristicasAntiguas = dsCaracteristicas.Copy();

            CrearCaracteristicas(dsCaracteristicas, false);

            //string cod_estado = ddl_EstadoSol.SelectedValue;
            //if (cod_estado == "-1" && hdnSubtipo.Value == "008" && dsCaracteristicas.Tables[0].Rows.Count==0) { CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, "001"); }
        }

        private void CargaCaracteristicasPorTipoSolicitud(string cod_tipo, string cod_subtipo, string cod_estado)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
            DataSet dsCaracteristicas;
            Boolean habilitado = true;
            if (cod_estado == "-1")
            {
                dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitud(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);
                habilitado = false;
            }
            else
            {
                //20200629 BGN BEG R#25049 Modificar callback Inspeccions de Portugal - Carga Valor en caracteristicas
                if (String.IsNullOrEmpty(hdnIdSolicitud.Value))
                {
                    dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorTipo(cod_tipo, cod_subtipo, cod_estado,usuarioDTO.IdIdioma);
                }
                else
                { 
                    SolicitudDB solDB = new SolicitudDB();
                    DataTable dtCar = solDB.GetCaracteristicasSolicitudPorTipo(cod_tipo, cod_subtipo, cod_estado, usuarioDTO.IdIdioma, hdnIdSolicitud.Value);
                    DataTable dtCarCopy = dtCar.Copy();
                    dsCaracteristicas = new DataSet();
                    dsCaracteristicas.Tables.Add(dtCarCopy);
                }
                //20200629 BGN END R#25049 Modificar callback Inspeccions de Portugal - Carga Valor en caracteristicas
            }

            CrearCaracteristicas(dsCaracteristicas, habilitado);
        }

        private void CargaCaracteristicasPorTipoSolicitudCambioMotivoCancelacion(string cod_tipo, string cod_subtipo, string motivoCancelacion, string cod_estado)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
            DataSet dsCaracteristicas;
            Boolean habilitado = true;
            //7	Cliente no acepta presupuesto                     
            if (motivoCancelacion == "7")
            {
                dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorMotivoCancelacion(cod_tipo, cod_subtipo, cod_estado, usuarioDTO.IdIdioma);
            }
            else
            {
                dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasPorTipo(cod_tipo, cod_subtipo, cod_estado, usuarioDTO.IdIdioma);
            }
            CrearCaracteristicas(dsCaracteristicas, habilitado);
        }

        private void CargaComboEstadosFuturos(string cod_tipo, string cod_subtipo, string cod_estado, string des_estado)
        {
            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();

            int idePerfil = 0;
            if (user.Id_Perfil.HasValue) { idePerfil = int.Parse(user.Id_Perfil.ToString()); }

            this.ddl_EstadoSol.Items.Clear();
            if (Usuarios.EsTelefono(idePerfil))
            {
                this.ddl_EstadoSol.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 1,user.IdIdioma);
            }
            else if (Usuarios.EsProveedor(idePerfil))
            {
                this.ddl_EstadoSol.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 2, user.IdIdioma);
            }
            else
            {
                this.ddl_EstadoSol.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 3, user.IdIdioma);
            }

            this.ddl_EstadoSol.DataTextField = "descripcion";
            this.ddl_EstadoSol.DataValueField = "codigo";
            this.ddl_EstadoSol.DataBind();
            //EsTelefono(intCodPerfil)
            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = des_estado;
            this.ddl_EstadoSol.Items.Insert(0, defaultItem);

            //'Kintell 06/04/2009
            //'ESTADOS FINALES!!!!!!!!
            btnAceptar.Enabled = true;
            if (defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_BAJA_SERVICIO.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_VISITA_REALIZADA.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_CANCELADO_TRANSFERIDO_A_SAT.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_CANCELADA_POR_REASIGNACION.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_REPARADA_CON_DOCUMENTACION.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_CANCELADA.ToUpper() || defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_REPARADA_POR_TELEFONO.ToUpper())
            {
                this.ddl_EstadoSol.Enabled = false;
                //this.gv_Caracteristicas.Enabled = false;
                this.ddl_MotivoCancel.Enabled = false;
                txt_Observaciones.Enabled = false;
                btnAceptar.Enabled = false;
                //this.ddl_MarcaCaldera.Enabled = false;
                this.txt_ModeloCaldera.Enabled = false;
            }
            else if (defaultItem.Text.ToUpper() == Resources.TextosJavaScript.TEXTO_REPARADA.ToUpper())
            {
                //this.gv_Caracteristicas.Enabled = false;
                this.ddl_MotivoCancel.Enabled = false;
                txt_Observaciones.Enabled = false;
                //btnAceptar.Enabled = false;
                //this.ddl_MarcaCaldera.Enabled = false;
                this.txt_ModeloCaldera.Enabled = false;
            }
        }

        private void ValidaComboMotivoCancelacion(string estadoSolicitud)
        {
            try
            {
                if (("012".Equals(estadoSolicitud) || "025".Equals(estadoSolicitud) || "056".Equals(estadoSolicitud)))
                {
                    ddl_MotivoCancel.Enabled = true;
                }
                else
                {
                    ddl_MotivoCancel.Enabled = false;
                }

                ListItem li = null;
                li = ddl_MotivoCancel.Items.FindByText("Seleccione el motivo de la cancelación");
                if ((li == null))
                {
                    li = new ListItem();
                    li.Text = Resources.TextosJavaScript.TEXTO_SELECCIONE_MOTIVO_CANCELACION;//"Seleccione el motivo de la cancelación";
                    li.Value = "-1";
                    ddl_MotivoCancel.Items.Insert(0, li);
                }

                if ((ddl_MotivoCancel.SelectedItem == null))
                {
                    ddl_MotivoCancel.Items[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                //' por si acaso
            }
        }

        private void CargaComboMotivosCancelacion(string Subtipo)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            ddl_MotivoCancel.Items.Clear();

            MotivosCancelacionDB objMotivosCancelacionDB = new MotivosCancelacionDB();
            //Kintell 12/12/2011
            ddl_MotivoCancel.DataSource = objMotivosCancelacionDB.GetMotivosCancelacion(Subtipo,usuarioDTO.IdIdioma);
            ddl_MotivoCancel.DataTextField = "descripcion";
            ddl_MotivoCancel.DataValueField = "cod_mot";
            ddl_MotivoCancel.DataBind();

            ListItem defaultItem = new ListItem();
            //defaultItem.Value = "-1"
            //defaultItem.Text = "Ninguno"
            //ddl_MotivoCancel.Items.Insert(0, defaultItem)


            //' por defecto esta desactivado
            ddl_MotivoCancel.Enabled = false;

        }

        //#2094:Pago Anticipado
        private void CargaComboLugarAveria()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            dll_tipLugarAveria.Items.Clear();

            TiposAveriaDB objLugarAveriaDB = new TiposAveriaDB();
            //Kintell 12/12/2011
            dll_tipLugarAveria.DataSource = objLugarAveriaDB.GetTipoLugarAveria(usuarioDTO.IdIdioma);
            dll_tipLugarAveria.DataTextField = "DESC_LUGAR_AVERIA";
            dll_tipLugarAveria.DataValueField = "ID_LUGAR_AVERIA";
            dll_tipLugarAveria.DataBind();

            ListItem defaultItem = new ListItem();


        }

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
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
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnVieneDeAltaCalderas.Value != "")
                {
                    // Deleting the Caldera´s issue if the user push the cancel button in the first time.
                    SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                    objSolicitudesDB.EjecutarSentencia("DELETE HISTORICO WHERE ID_SOLICITUD='" + hdnIdSolicitud.Value + "'");
                    objSolicitudesDB.EjecutarSentencia("DELETE SOLICITUDES WHERE ID_SOLICITUD='" + hdnIdSolicitud.Value + "'");
                    MasterPageModal mpm = (MasterPageModal)this.Master;
                    string script = "<script>window.parent.refrescar();</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "REFRESCAR", script, false);
                    mpm.CerrarVentanaModal();
                }
                else
                {
                    MasterPageModal mpm1 = (MasterPageModal)this.Master;
                    mpm1.CerrarVentanaModal();
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void ddl_EstadoSol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                //gv_Caracteristicas.Enabled = true;

                //string cod_tipo = gv_Solicitudes.SelectedRow.Cells(3).Text.ToString();
                //string cod_subtipo = gv_Solicitudes.SelectedRow.Cells(4).Text.ToString();
                string cod_estado = ddl_EstadoSol.SelectedValue;
                if (cod_estado == "-1") { cod_estado = hdnEstadoInicialSolicitud.Value; }


                //2094: Pago Anticipado
                // Si el estado seleccionado es Reparada o Reparada por Telefono
                // Beretta. 27/02/2017
                if ((cod_estado.Equals("010") || cod_estado.Equals("011")) && (hdnSubtipo.Value != "012" && hdnSubtipo.Value != "013"))
                {
                    lblTipLugarAveria.Visible = true;
                    dll_tipLugarAveria.Visible = true;
                }
                else
                {
                    lblTipLugarAveria.Visible = false;
                    dll_tipLugarAveria.Visible = false;
                }


                ValidaComboMotivoCancelacion(cod_estado);

                CargaCaracteristicasPorTipoSolicitud("001", hdnSubtipo.Value, cod_estado);

                // Kintell fichero GC, contrato GC, 11/10/2019.
                
                ConfiguracionDTO confMostrarFichero = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.MOSTRAR_FICHERO_GC_A_GUARDAR);
                Boolean mostrarFchero = false;
                if (confMostrarFichero != null && !string.IsNullOrEmpty(confMostrarFichero.Valor) && Boolean.Parse(confMostrarFichero.Valor))
                {
                    mostrarFchero = Boolean.Parse(confMostrarFichero.Valor);
                }
                
                ConfiguracionDTO confEstadosMostrarFicheroGC = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ESTADOS_FICHERO_GC);
                Boolean mostrarFcheroPorEstado = false;
                if (confEstadosMostrarFicheroGC != null)
                {
                    foreach (string codigoEstado in confEstadosMostrarFicheroGC.Valor.Split(';'))
                    {
                        if (codigoEstado.Equals(cod_estado))
                        {
                            mostrarFcheroPorEstado = true;
                        }
                    }
                }

                //17/10/2019 BGN BEG R#17876 Heredar Nº Serie en Averias GC en Aparato
                if (hdnSubtipo.Value == "012" && cod_estado.Equals("010") && String.IsNullOrEmpty(hdnNumSerie.Value))
                {
                    string solGC = string.Empty;
                    string contrato = ObtenerContratoSolicitud();
                    string numSerie = CaracteristicaHistorico.GetNumSerieGC(contrato);
                    if (!String.IsNullOrEmpty(numSerie))
                    {
                        hdnNumSerie.Value = numSerie;
                    }   
                }
                //17/10/2019 BGN END R#17876 Heredar Nº Serie en Averias GC en Aparato

                //02/12/2020 BGN BEG R#28313 Cambio Flujo Gas Confort por digitalización contrato
                // Quitamos del if cod_estado.Equals("057") porque con el mostrarFcheroPorEstado parametrizado en BBDD por ESTADOS_FICHERO_GC lo cubrimos
                if (hdnSubtipo.Value == "008" && mostrarFchero && mostrarFcheroPorEstado)
                {
                    lblContratoGC.Visible = true;
                    FileGC.Visible = true;
                }
                else
                {
                    lblContratoGC.Visible = false;
                    FileGC.Visible = false;
                }
                //02/12/2020 BGN END R#28313 Cambio Flujo Gas Confort por digitalización contrato

                if (hdnSubtipo.Value == "008" && ((hdnEstadoInicialSolicitud.Value == cod_estado) || (hdnEstadoInicialSolicitud.Value == "044" && cod_estado == "038") || (hdnEstadoInicialSolicitud.Value == "042" && cod_estado == "041")))
                {
                    // Copy the characteristics between this two states.
                    //Pues eso...

                    CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                    DataSet dsCaracteristicas;

                    if (hdnEstadoInicialSolicitud.Value == "044")
                    {
                        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitud(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);
                    }
                    else
                    {
                        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitudGasConfortPdteAceptacionFinTrabajos(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);
                    }
                    //CambioComboCaracteristica_SelectedIndexChanged("0",dsCaracteristicas.Tables[0].Rows[0].ItemArray[2].ToString());

                    for (int i = 0; i < dsCaracteristicas.Tables[0].Rows.Count; i++)
                    {

                        string idObjeto = "Caracteristica" + i;
                        Control Objeto = FindControlById(tblCaracteristicas, idObjeto);
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();


                        string idCaracteristica = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString();

                        if (i < 4 && (cod_estado == "038" || cod_estado == "044"))
                        {
                            DropDownList combo;
                            combo = (DropDownList)Objeto;
                            if (combo != null && combo.Items.Count > 1)
                            {
                                combo.SelectedIndex =
                                    combo.Items.IndexOf(combo.Items.FindByText(dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString()));
                                if (hdnEstadoInicialSolicitud.Value == cod_estado)
                                {
                                    combo.Enabled = false;
                                }
                                //combo.SelectedValue = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                            }

                            string ID = "";
                            switch (i)
                            {
                                case 0:
                                    ID = "Caracteristica0-072-1-1";
                                    break;
                                case 1:
                                    ID = "Caracteristica1-073-2-14";
                                    break;
                                case 2:
                                    ID = "Caracteristica2-084-3-27";
                                    break;
                                case 3:
                                    ID = "Caracteristica3-074-4-27";
                                    break;
                            }

                            if (combo.SelectedValue != "")
                            {
                                CambioComboCaracteristica_SelectedIndexChanged(ID, combo.SelectedValue);
                            }
                        }
                        else
                        {
                            TextBox text;
                            text = (TextBox)Objeto;
                            text.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                            if (hdnEstadoInicialSolicitud.Value == cod_estado)
                            {
                                text.Enabled = false;
                            }
                        }

                    }



                }

                // (20/08/2015) Marcamos con un 0 para indicar que no estamos dando de alta la solicitud de averia de gas confort si no que estamos modificando
                // una existente.

                hdnAltaIncidenciaGasConfort.Value = "0";
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_VUELVA_A_SELECCIONAR_LA_SOLICITUD);//"Vuelva a seleccionar la solicitud en la búsqueda");
            }
        }

        protected void ddl_MotivoCancel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                //gv_Caracteristicas.Enabled = true;

                //string cod_tipo = gv_Solicitudes.SelectedRow.Cells(3).Text.ToString();
                //string cod_subtipo = gv_Solicitudes.SelectedRow.Cells(4).Text.ToString();
                string cod_estado = ddl_EstadoSol.SelectedValue;
                if (cod_estado == "-1") { cod_estado = hdnEstadoInicialSolicitud.Value; }


                //2094: Pago Anticipado
                // Si el estado seleccionado es Reparada o Reparada por Telefono
                // Beretta. 27/02/2017
                if ((cod_estado.Equals("010") || cod_estado.Equals("011")) && (hdnSubtipo.Value != "012" && hdnSubtipo.Value != "013"))
                {
                    lblTipLugarAveria.Visible = true;
                    dll_tipLugarAveria.Visible = true;
                }
                else
                {
                    lblTipLugarAveria.Visible = false;
                    dll_tipLugarAveria.Visible = false;
                }


                ValidaComboMotivoCancelacion(cod_estado);

                CargaCaracteristicasPorTipoSolicitudCambioMotivoCancelacion("001", hdnSubtipo.Value, ddl_MotivoCancel.SelectedValue, cod_estado);


                if (hdnSubtipo.Value == "008" && ((hdnEstadoInicialSolicitud.Value == cod_estado) || (hdnEstadoInicialSolicitud.Value == "044" && cod_estado == "038") || (hdnEstadoInicialSolicitud.Value == "042" && cod_estado == "041")))
                {
                    // Copy the characteristics between this two states.
                    //Pues eso...

                    CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                    DataSet dsCaracteristicas;

                    if (hdnEstadoInicialSolicitud.Value == "044")
                    {
                        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitud(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);
                    }
                    else
                    {
                        dsCaracteristicas = objCaracteristicasDB.GetCaracteristicasSolicitudGasConfortPdteAceptacionFinTrabajos(hdnIdSolicitud.Value,usuarioDTO.IdIdioma);
                    }
                    //CambioComboCaracteristica_SelectedIndexChanged("0",dsCaracteristicas.Tables[0].Rows[0].ItemArray[2].ToString());

                    for (int i = 0; i < dsCaracteristicas.Tables[0].Rows.Count; i++)
                    {

                        string idObjeto = "Caracteristica" + i;
                        Control Objeto = FindControlById(tblCaracteristicas, idObjeto);
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();


                        string idCaracteristica = dsCaracteristicas.Tables[0].Rows[i].ItemArray[0].ToString();

                        if (i < 4 && (cod_estado == "038" || cod_estado == "044"))
                        {
                            DropDownList combo;
                            combo = (DropDownList)Objeto;
                            if (combo != null && combo.Items.Count > 1)
                            {
                                combo.SelectedIndex =
                                    combo.Items.IndexOf(combo.Items.FindByText(dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString()));
                                if (hdnEstadoInicialSolicitud.Value == cod_estado)
                                {
                                    combo.Enabled = false;
                                }
                                //combo.SelectedValue = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                            }

                            string ID = "";
                            switch (i)
                            {
                                case 0:
                                    ID = "Caracteristica0-072-1-1";
                                    break;
                                case 1:
                                    ID = "Caracteristica1-073-2-14";
                                    break;
                                case 2:
                                    ID = "Caracteristica2-084-3-27";
                                    break;
                                case 3:
                                    ID = "Caracteristica3-074-4-27";
                                    break;
                            }

                            if (combo.SelectedValue != "")
                            {
                                CambioComboCaracteristica_SelectedIndexChanged(ID, combo.SelectedValue);
                            }
                        }
                        else
                        {
                            TextBox text;
                            text = (TextBox)Objeto;
                            text.Text = dsCaracteristicas.Tables[0].Rows[i].ItemArray[2].ToString();
                            if (hdnEstadoInicialSolicitud.Value == cod_estado)
                            {
                                text.Enabled = false;
                            }
                        }

                    }



                }

                // (20/08/2015) Marcamos con un 0 para indicar que no estamos dando de alta la solicitud de averia de gas confort si no que estamos modificando
                // una existente.

                hdnAltaIncidenciaGasConfort.Value = "0";
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_VUELVA_A_SELECCIONAR_LA_SOLICITUD);//"Vuelva a seleccionar la solicitud en la búsqueda");
            }
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void EnviarAviso(string idSolicitud)
        {
            IDataReader Datos = null;
            try
            {
                String usuario = CurrentSession.GetAttribute("usuarioValido").ToString();

                Datos = Mantenimiento.ObtenerAvisoSolicitud(idSolicitud);
                String Destino = "";
                while (Datos.Read())
                {
                    Destino = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "email_aviso");
                }



                if (Destino != "")
                {
                    //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                    //MandarMail(Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_TWITTER + hdnContrato.Value + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + txtTelefono.Text, Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString()), Destino, true);
                    string asunto = Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString());
                    string mensaje = Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_TWITTER + hdnContrato.Value + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + txtTelefono.Text;
                    UtilidadesMail.EnviarAviso(asunto, mensaje, "tcservicios@iberdrola.es", Destino);
                    //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                }

                SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                objSolicitudesDB1.EjecutarSentencia("UPDATE SOLICITUDES SET es_twitter=1 WHERE ID_SOLICITUD='" + idSolicitud + "'");
                //String Script = "<Script>alert('Proceso Completado');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script, false);
            }
            finally
            {
                if (Datos != null)
                {
                    Datos.Close();
                }
            }
        }

        private void EnviarAvisoFacebook(string idSolicitud)
        {
            IDataReader Datos = null;
            try
            {
                String usuario = CurrentSession.GetAttribute("usuarioValido").ToString();

                Datos = Mantenimiento.ObtenerAvisoSolicitud(idSolicitud);
                String Destino = "";
                while (Datos.Read())
                {
                    Destino = (String)DataBaseUtils.GetDataReaderColumnValue(Datos, "email_aviso");
                }

                if (Destino != "")
                {
                    //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                    //MandarMail(Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_FACEBOOK + hdnContrato.Value + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + txtTelefono.Text, Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString()), Destino, true);
                    string asunto = Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + hdnContrato.Value.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(hdnContrato.Value.ToString());
                    string mensaje = Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_FACEBOOK + hdnContrato.Value + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + txtTelefono.Text;
                    UtilidadesMail.EnviarAviso(asunto, mensaje, "tcservicios@iberdrola.es", Destino);
                    //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
                }

                SolicitudesDB objSolicitudesDB1 = new SolicitudesDB();
                objSolicitudesDB1.EjecutarSentencia("UPDATE SOLICITUDES SET es_facebook=1 WHERE ID_SOLICITUD='" + idSolicitud + "'");
                //String Script = "<Script>alert('Proceso Completado');parent.VentanaModal.cerrar();</Script>";//CerrarVentanaModal()
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", Script, false);
            }
            finally
            {
                if(Datos != null)
                {
                    Datos.Close();
                }
            }
        }

        protected void btnAltaAveriaCaldera_Click(object sender, EventArgs e)
        {
            try
            {
                string CodContrato = hdnContrato.Value;
                string id = hdnIdSolicitud.Value;

                // (20/08/2015) Marcamos con un 1 para indicar que estamos dando de alta la solicitud de averia de gas confort.
                hdnAltaIncidenciaGasConfort.Value = "1";
                // (20/08/2015) Cargamos las características para el alta de avería de gas confort.
                CargaCaracteristicasPorTipoSolicitud("001", "012", "065");

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCodificacionAverias0_Click(object sender, EventArgs e)
        {

        }

        protected void OnbtnTicketCombustion_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (hdnContrato.Value == null || hdnContrato.Value.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    String strUrl = "./FrmModalTicketCombustion.aspx" + "?COD_CONTRATO=" + this.hdnContrato.Value + "&ID_SOLICITUD=" + this.hdnIdSolicitud.Value + "&EDICION=false";
                    this.OpenWindow(strUrl, "ModalTicketCombustion", "Ticket de combustion");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        //20210120 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        private string ObtenerCCBBDocumento(string nombreDoc)
        {
            string CCBB = string.Empty;
            string[] sNombreFichero = nombreDoc.Split('_');
            if (int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.GasConfort
                    || int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.RevisionPorPrecinte
                    || int.Parse(hdnSubtipo.Value) == (int)Enumerados.SubtipoSolicitud.InstalacionTermostatoInteligente)
            {
                CCBB = sNombreFichero[0].ToString();
            }
            else
            {
                string contrato = sNombreFichero[0].ToString();
                string fecha = sNombreFichero[1].ToString();
                string prov = sNombreFichero[2].ToString();
                string TipoDocumentoSinExtension = sNombreFichero[3].ToString();
                int posicionPunto = TipoDocumentoSinExtension.IndexOf(".");
                TipoDocumentoSinExtension = TipoDocumentoSinExtension.Substring(0, posicionPunto);
                CCBB = contrato + fecha + prov + TipoDocumentoSinExtension;
            }

            return CCBB;
        }

        private void GenerarPDF(string email, string idMovimiento)
        {
            string cod_contrato = string.Empty;
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (hdnContrato.Value == null || hdnContrato.Value.Length == 0)
                {
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    if (String.IsNullOrEmpty(email))
                    {
                        MostrarMensaje(Resources.TextosJavaScript.NO_EMAIL_DESTINO);//"No hay email al que mandar el contrato para firmar digitalmente"
                    }
                    else
                    {
                        cod_contrato = ObtenerContratoSolicitud();
                        if (String.IsNullOrEmpty(cod_contrato))
                        {
                            cod_contrato = this.hdnContrato.Value;
                        }
                        //TODO: Comprobar nomenclatura fichero 1010394220_20210122_SIE_CGC.pdf o 101345793920210122SIECGC_1013457939.pdf este ultimo es el que te pide al adjuntar por pantalla
                        string nombreFichero = PCamposPdfContratoGasConfort.GenerarContratoGasconfort(cod_contrato, Convert.ToDecimal(this.hdnIdSolicitud.Value));
                        if (String.IsNullOrEmpty(nombreFichero))
                        {
                            MostrarMensaje(Resources.TextosJavaScript.ERROR_GENERAR_CONTRATO_GC_DIGITAL);//"Error al generar el contrato digital. No enviado a Edatalia"
                        }
                        else
                        {
                            string refExternaEdatalia = this.hdnIdSolicitud.Value + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string nombreCliente = String.Empty;
                            string dniCliente = String.Empty;
                            string movilCliente = String.Empty;
                            string idDocEdatalia = String.Empty;
                            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(this.hdnContrato.Value);
                            if (datosMantenimiento != null)
                            {
                                while (datosMantenimiento.Read())
                                {
                                    nombreCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2")).TrimEnd();
                                    dniCliente = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");
                                    movilCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_MOVIL")).TrimEnd(); 
                                }
                            }
                            if (!String.IsNullOrEmpty(email))
                            {
                                //Llamar a WS Edatalia para que envie un email al cliente para que firme el contrato de GC
                                UtilidadesWebServices callWS = new UtilidadesWebServices();
                                idDocEdatalia = callWS.llamadaWSEdataliaAltaDocumentoPorEmail(nombreFichero, refExternaEdatalia, nombreCliente, dniCliente, email, movilCliente);

                            }
                            if (!String.IsNullOrEmpty(idDocEdatalia))
                            {
                                //20210708 BGN MOD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
                                if (idDocEdatalia.Equals("-38"))
                                {
                                    //Pasamos la solicitud al estado 083 - Pdte. corregir e-mail
                                    Solicitud soli = new Solicitud();
                                    soli.CambioEstadoSolicitudeHistorico(this.hdnIdSolicitud.Value, "083", "WS_EDATA", "RecipientMail format is wrong.");
                                    MostrarMensaje(Resources.TextosJavaScript.ERROR_ENVIO_CONTRATO_GC_DIGITAL); //"Error al enviar el contrato digital a Edatalia"
                                }
                                else
                                {
                                    //Guardar en tabla DOCUMENTO
                                    DocumentoDTO documentoDto = new DocumentoDTO();
                                    documentoDto.CodContrato = cod_contrato;
                                    documentoDto.IdSolicitud = int.Parse(hdnIdSolicitud.Value);
                                    documentoDto.NombreDocumento = nombreFichero;
                                    documentoDto.IdTipoDocumento = 1;
                                    documentoDto.EnviarADelta = false;
                                    documentoDto.IdEnvioEdatalia = refExternaEdatalia;
                                    documentoDto.FechaEnvioEdatalia = DateTime.Now;
                                    documentoDto.IdDocEdatalia = idDocEdatalia;
                                    documentoDto = Documento.Insertar(documentoDto, usuarioDTO.Login);
                                    //Mover fichero a RUTA_FICHEROS_EDATALIA_ENVIADOS
                                    ConfiguracionDTO rutaEdatalia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA);
                                    String ficheroOrigen = rutaEdatalia.Valor + nombreFichero;
                                    ConfiguracionDTO rutaEdataliaEnviados = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA_ENVIADOS);
                                    FileUtils.FileMoveRewrite(ManejadorFicheros.GetImpersonator(), ficheroOrigen, rutaEdataliaEnviados.Valor);
                                    //Guardar la referencia del CCBB del documento en la caracteristica 081
                                    string CCBB = ObtenerCCBBDocumento(nombreFichero);
                                    CaracteristicasDB objCaracteristicasDB = new CaracteristicasDB();
                                    int id_caracteristica_CCBB = objCaracteristicasDB.AddCaracteristica(hdnIdSolicitud.Value, "081", CCBB, DateTime.Now.ToShortDateString());
                                    HistoricoDB objHistoricoDB = new HistoricoDB();
                                    objHistoricoDB.AddHistoricoCaracteristicas(id_caracteristica_CCBB, idMovimiento, "081", CCBB);
                                }
                                //20210708 BGN MOD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos  
                            }
                            else
                            {
                                MostrarMensaje(Resources.TextosJavaScript.ERROR_ENVIO_CONTRATO_GC_DIGITAL); //"Error al enviar el contrato digital a Edatalia"
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UtilidadesMail.EnviarMensajeError("Error al Generar contrato GC y enviarlo a Edatalia", "Se ha producido un error al Generar contrato GC y enviarlo a Edatalia. Contrato: " + cod_contrato + " IdSolicitud: " + this.hdnIdSolicitud.Value + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);

            }
            finally
            {
                //String Script = "<script language='javascript'>OcultarCapaEspera();</script>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);
            }
        }
        //20210120 BGN MOD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        private void DescargarArchivo(string strRutaFichero)
        {​
          using (ManejadorFicheros.GetImpersonator())
            {​​​​
            
                try
                {​​​​
                    Response.Clear();
                    LogHelper.Debug("DescargarArchivo - Inicio Descarga");
                    FileStream sourceFile = new FileStream(strRutaFichero, FileMode.Open);
                    long fileSize;
                    fileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)fileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();



                    string ext = Path.GetExtension(strRutaFichero);
                    string type = "";
                    // set known types based on file extension
                    if (ext != null)
                    {​​​​
                    switch (ext.ToLower())
                        {
                            case ".html":
                            case ".htm":
                                {
                                    type = "text/HTML";
                                    break;
                                }
                            case ".txt":
                                {
                                    type = "text/plain";
                                    break;
                                }
                            case ".doc":
                            case ".rtf":
                                {
                                    type = "Application/msword";
                                    break;
                                }
                            case ".pdf":
                                {
                                    type = "Application/pdf";
                                    break;
                                }
                            //case ".zip":
                            case ".rar":
                                {
                                    type = "pplication/x-zip-compressed";
                                    break;
                                }
                            default:
                                {
                                    type = "text/plain";
                                    break;
                                }
                        }
                    }


                    LogHelper.Debug("DescargarArchivo - READ OK");
                    Response.ContentType = type;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(strRutaFichero));
                    if (getContent.Length > 0)
                    {​​​​
                        Response.BinaryWrite(getContent);
                    }​​​​
                    else
                    {​​​​
                        Response.Write(string.Empty);
                    }​​​​
                    LogHelper.Debug("DescargarArchivo - OK");
                    Response.Flush();
                    //Workaround sacado de internet para que no de excepciones al cerrar la llamada (porque se supone que el Flush ya hace el end)
                    //Response.End();



                    // Prevents any other content from being sent to the browser
                    Response.SuppressContent = true;



                    // Directs the thread to finish, bypassing additional processing
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (Exception)
                {​​​​

                }​​​​
            }​​​​
        }​​​​

        
    }
}