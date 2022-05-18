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
using System.ComponentModel;
using System.Reflection;
using Iberdrola.SMG.UI;
using Iberdrola.Commons.Validations;

namespace Iberdrola.SMG.UI
{
    public partial class FrmSolicitudesCalderas : FrmBaseListado
    {

        public enum ModoPantalla
        {
            SIN_RESULTADOS = 1,
            VISUALIZANDO = 2,
            EDITANDO_SOLICITUD = 3,
            ALTA_SOLICITUD = 4,
            ALTA_CALDERA = 5
        }

        #region Propiedades
        /// <summary>
        /// Establece o retorna el modo de la pantalla
        /// </summary>
        public ModoPantalla Modo
        {
            get { return (ModoPantalla)int.Parse(this.hdnModo.Value); }
            set { this.hdnModo.Value = ((int)value).ToString(); }
        }
        /// <summary>
        /// Establece o retorna el cod. de la marca de la caldera.
        /// </summary>
        public decimal? codMarcaCaldera
        {
            get { return decimal.Parse(this.hdnCodMarcaCaldera.Value); }
            set { this.hdnCodMarcaCaldera.Value = ((decimal)value).ToString(); }
        }

        /// <summary>
        /// Establece o retorna el modo código del perfil del usuario
        /// </summary>
        public int CodPerfil
        {
            get { return int.Parse(this.hdnCodPerfil.Value); }
            set { this.hdnCodPerfil.Value = value.ToString(); }
        }


        /// <summary>
        /// Establece o retorna el código del proveedor
        /// </summary>
        public string CodProveedor
        {
            get { return this.hdnCodProveedor.Value; }
            set { this.hdnCodProveedor.Value = value; }
        }

        private string _DNI;
        /// <summary>
        /// Establece o retorna el DNI del cliente.
        /// </summary>
        public string DNI
        {
            get { return this.hdnDNI.Value; }
            set { this.hdnDNI.Value = value; }
        }

        private string _CP;
        /// <summary>
        /// Establece o retorna el código postal.
        /// </summary>
        public string CP
        {
            get { return this.hdnCP.Value; }
            set { this.hdnCP.Value = value; }
        }
        #endregion

        #region Metodos Auxiliares
        /// <summary>
        /// Establece el modo de la pantalla según el origen.
        /// </summary>
        /// <param name="strOrigen"></param>
        public void EstablecerModoPantallaSegunOrigen(string strOrigen)
        {
            // Si tenemos un origen diferente del MENU strOrigen estará informado
            if (!string.IsNullOrEmpty(strOrigen))
            {
                // Si venimos de visita entonces tenemos que entrar en modo ALTA_CALDERA
                if (strOrigen.Equals("VISITA"))
                {
                    this.Modo = ModoPantalla.ALTA_CALDERA;
                }
            }
            else
            {
                // Si el origen no es el menú strOrigen no estará informado y el modo de entrada sera
                // SIN_RESULTADOS
                this.Modo = ModoPantalla.SIN_RESULTADOS;
            }
        }




        void list_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    break;
                case ListChangedType.ItemChanged:
                    break;
                case ListChangedType.ItemDeleted:
                    break;
                case ListChangedType.ItemMoved:
                    break;
                // some more minor ones, etc.
            }
        }



        /// <summary>
        /// Carga los datos del usuario en las variables de la pantalla
        /// </summary>
        public void CargarDatosUsuario()
        {
            UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
            if (user != null)
            {
                //Si el código del proveedor viene en blanco es que es el teléfono.
                if (!string.IsNullOrEmpty(user.CodProveedor))
                {
                    this.CodProveedor = user.CodProveedor;
                }
                else
                {
                    this.CodProveedor = "TEL";
                }
                //Siempre va a venir el código del perfil...
                if (user.Id_Perfil.HasValue)
                {
                    this.CodPerfil = (int)user.Id_Perfil.Value;
                }
            }
            else
            {
                // TODO: poner código de error.
                throw new UIException("1111");
            }
        }

        /// <summary>
        /// Muestra u oculta los paneles en función del modo
        /// </summary>
        public void MostrarOcultarPaneles()
        {
            switch (this.Modo)
            {
                case ModoPantalla.SIN_RESULTADOS:
                    // La opción por defecto en la pantalla de búsqueda es la de solicitudes
                    this.ExecuteScript("VerSolicitudes();");
                    break;
                case ModoPantalla.VISUALIZANDO:
                    // Si ya estamos visualizando datos tenemos en cuenta
                    // el panel que estuviera seleccionado.
                    if (hdnPanelSeleccionado.Value == "VISITAS")
                    {
                        this.ExecuteScript("VerVisitas()");
                    }
                    else if (hdnPanelSeleccionado.Value == "SOLICITUDES")
                    {
                        this.ExecuteScript("VerSolicitudes();");
                    }
                    break;
                default:
                    // Si no es ninguna de las anteriores entonces estamos en editar/alta
                    this.ExecuteScript("VerEditar()");
                    break;
            }
        }

        /// <summary>
        /// Muestra u oculta los controles generales en función del perfil
        /// </summary>
        public void MostrarOcultarControlesPerfil()
        {
            // Mostramos u ocultamos los botones
            if (Usuarios.EsProveedor(this.CodPerfil))
            {
                //11-04-2016 Comentado a petición de Edurne.
              //  this.btnBuscarCalderas.Visible = false;
                this.btnNuevaSolicitud.Visible = false;
                btnCobertura.Visible = false;
                btnNuevaReclamacion.Visible = false;
            }
            else
            {
                this.btnBuscarCalderas.Visible = true;
                this.btnNuevaSolicitud.Visible = true;
                btnCobertura.Visible = true;
                if (Usuarios.EsReclamacion(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                {
                    btnNuevaReclamacion.Visible = true;
                }
                //20200309 BGN BEG R#22813 Bloquear el acceso al canal (puntos y teléfono) para generar reclamaciones
                else
                {
                    btnNuevaReclamacion.Visible = false;
                }
                //20200309 BGN END R#22813 Bloquear el acceso al canal (puntos y teléfono) para generar reclamaciones
            }

            // Mostramos u ocultamos las visitas
            if (Usuarios.EsProveedor(this.CodPerfil))
            {
                this.ExecuteScript("document.getElementById('divPestaniaVisitas').style.visibility = 'hidden';");
                //this.ExecuteScript("document.getElementById('divPestaniaSolicitudes').style.visibility = 'visible';");
            }
            else
            {
                this.ExecuteScript("document.getElementById('divPestaniaVisitas').style.visibility = 'visible';");
                //this.ExecuteScript("document.getElementById('divPestaniaSolicitudes').style.visibility = 'hidden';");
            }

            //17/05/2013
            this.ExecuteScript("document.getElementById('divPestaniaVisitas').style.visibility = 'visible';");
            // Habilitamos/Deshabilitamos los controles del filtro.
            HabilitarDeshabilitarControlesPorPerfil();
            // Kintell 16/11/2021 R#25576
            //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            //UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            //txtDNI.Enabled = false;
            //txtCUI.Enabled = false;
            //ConfiguracionDTO configPais = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.PAIS_BUSQUEDA_POR_DNI);
            //if (configPais != null && !string.IsNullOrEmpty(configPais.Valor) && configPais.Valor.ToUpper() == usuarioDTO.Pais.ToUpper())
            //{ 
            //    txtDNI.Enabled = true;
            //    txtCUI.Enabled = true;
            //}
        }

        /// <summary>
        /// Obtiene el número de páginas en las que se dividirá el resultado
        /// </summary>
        /// <param name="numRegs"></param>
        /// <returns></returns>
        public Int32 ObtenerNumPaginas(Int32 numRegs)
        {
            if (numRegs > 0)
            {
                Int32 resto;
                Int32 resultado = Math.DivRem(numRegs, Int32.Parse(Resources.SMGConfiguration.GridViewPageSize), out resto);
                if (resto > 0)
                {
                    resultado++;
                }
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Carga los controles para poder moverse entre las diferentes páginas
        /// </summary>
        /// <param name="NumPagina"></param>
        public void CargarPaginador(Int32 NumPagina)
        {
            if (this.hdnPageCount.Value == "") { this.hdnPageCount.Value = "0"; }
            Int32 intNumPaginas = Int32.Parse(this.hdnPageCount.Value);
            Int32 intPaginaActual = NumPagina;
            int numPaginasMostradas = 10;
            int rangoActual = intPaginaActual / numPaginasMostradas;
            int limiteSuperior = (rangoActual + 1) * numPaginasMostradas;

            if (limiteSuperior > intNumPaginas)
            {
                limiteSuperior = intNumPaginas;
            }

            int limiteInferior = (rangoActual * numPaginasMostradas) + 1;

            if (limiteInferior == intPaginaActual && intPaginaActual > 1)
            {
                limiteInferior -= numPaginasMostradas;
                if (limiteInferior < 0)
                {
                    limiteInferior = 1;
                }
                limiteSuperior -= numPaginasMostradas;
            }

            this.placeHolderPaginacion.Controls.Clear();

            if ((limiteInferior) > numPaginasMostradas)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion1";
                lb.CommandName = "hlPaginacion1";
                lb.CommandArgument = 1.ToString();
                lb.OnClientClick = "return ClickPaginacion('1')";
                lb.CssClass = "linkPaginacion";
                lb.Text = ("<<").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);
            }

            for (int i = limiteInferior; i <= limiteSuperior; i++)
            {
                if (i == intPaginaActual)
                {
                    Label lb = new Label();
                    lb.ID = "hlPaginacion" + i;
                    lb.CssClass = "linkPaginacionDeshabilitado";
                    lb.Text = (i).ToString();

                    this.placeHolderPaginacion.Controls.Add(lb);
                }
                else
                {
                    Button lb = new Button();
                    lb.ID = "hlPaginacion" + i;
                    lb.CommandName = "hlPaginacion" + i;
                    lb.CommandArgument = i.ToString();
                    lb.OnClientClick = "return ClickPaginacion('" + i + "')";
                    lb.CssClass = "linkPaginacion";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacion.Controls.Add(lb);
                }
            }

            if (limiteSuperior < intNumPaginas - 10)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion" + intNumPaginas;
                lb.CommandName = "hlPaginacion" + intNumPaginas;
                lb.CommandArgument = intNumPaginas.ToString();
                lb.OnClientClick = "return ClickPaginacion('" + intNumPaginas + "')";
                lb.CssClass = "linkPaginacion";
                lb.Text = (">>").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);
            }
        }
        #endregion

        #region Metodos Auxiliares de datos
        protected void CargarDatos()
        {
            try
            {
                DatosCliente(hdnIdSolicitud.Value);//, this.CodProveedor.ToString());
                if (txt_contrato.Text != "")
                {
                    if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                    {
                        CargarVisitas(lblCodContratoInfo.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                //ShowMessage("Se ha producido un error en la carga de datos, revise los filtros seleccionados.");
                ShowMessage(Resources.TextosJavaScript.TEXTO_ERROR_CARGA_DATOS);
            }
        }

        private void RemoveElement(DropDownList dCombo, string sValor, int iPosicion)
        {
            //12/06/2015 Administrador puede hacer todo.
            if (!Usuarios.EsAdministrador(this.CodPerfil))
            {
                if (sValor != "")
                {
                    dCombo.Items.Remove(dCombo.Items.FindByText(sValor));
                }
                else
                {
                    dCombo.Items.RemoveAt(iPosicion);
                }
            }
        }

        protected void DatosCliente(String IdSolicitud)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            InicializarControlesDatosContrato();
            if (IdSolicitud != "" || txt_contrato.Text != "") { btnCobertura.Enabled = true; }
            Mantenimiento mantenimiento = new Mantenimiento();
            String CodContrato = "";

            if (IdSolicitud == "" && txt_contrato.Text == "" && (txtDNI.Text != "" || txtCUI.Text != ""))
            {
                CodContrato = Mantenimiento.GetContratoPorDNICUPS(txtDNI.Text, txtCUI.Text);
            }
            else
            {
                if (txt_contrato.Text == "")
                {
                    CodContrato = Mantenimiento.GetContratoPorSolicitud(IdSolicitud);
                }
                else
                {
                    CodContrato = txt_contrato.Text;
                }
            }

            if (String.IsNullOrEmpty(CodContrato))
            {
                return;
            }

            MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();
            if (Usuarios.EsAdministrador(this.CodPerfil))
            {
                mantenimientoDTO = mantenimiento.DatosMantenimientoSinPais(CodContrato);
            }
            else
            {
                mantenimientoDTO = mantenimiento.DatosMantenimiento(CodContrato, usuarioDTO.Pais);
            }

            ConfiguracionDTO confActivoRepairAndCare = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOREPARIANDCARE);
            Boolean ActivoRepairAndCare = false;

            if (confActivoRepairAndCare != null && !string.IsNullOrEmpty(confActivoRepairAndCare.Valor) && Boolean.Parse(confActivoRepairAndCare.Valor))
            {
                ActivoRepairAndCare = Boolean.Parse(confActivoRepairAndCare.Valor);
            }


            if (String.IsNullOrEmpty(mantenimientoDTO.COD_CONTRATO_SIC) && ActivoRepairAndCare)
            {
                // KINTELL BEG R#35439
                // Si no encotramos el contrato en Opera, lo buscamos en Delta.
                UtilidadesWebServices uw = new UtilidadesWebServices();
                String resultadoLlamadaObtenerDatosContratoDelta = uw.llamadaWSobtenerDatosCtoOperaSMG(CodContrato);
                XmlNode datosXML = uw.StringAXMLRepairAndCare(resultadoLlamadaObtenerDatosContratoDelta,CodContrato);
                if (datosXML != null)
                {
                    string codContrato = datosXML.SelectNodes("codContrato").Item(0).InnerText.ToString();
                    string codReceptor = datosXML.SelectNodes("codRS").Item(0).InnerText.ToString();
                    string codSociedad = datosXML.SelectNodes("codSociedad").Item(0).InnerText.ToString();

                    mantenimientoDTO = mantenimiento.DatosMantenimientoPorCodReceptor(codContrato,codReceptor,codSociedad);
                    if (String.IsNullOrEmpty(mantenimientoDTO.COD_CONTRATO_SIC))
                    {
                        // Cargamos los datos en mantenimientoDTO y lo metemos en Opera si el estado es TA.
                        mantenimientoDTO = mantenimiento.DatosMantenimientoDesdeDeltaYAltaContrato(datosXML, true);
                        if (!String.IsNullOrEmpty(mantenimientoDTO.OBSERVACIONES))
                        {
                            MostrarMensaje(mantenimientoDTO.OBSERVACIONES);
                        }
                    }
                        //Buscar();
                        //return;
                    
                }
                // Dejamos que el proceso siga igual.
                // KINTELL END R#35439
            }

            try
            {
                //Reader.Read();
                if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                {
                    btnBajaServicio.Visible = false;// true;
                    //#2095:Pago Anticipado btnAltaCaldera.Visible = true;
                    // (10/10/2014) Lo quitamos a petición de Rubén vía mail.
                    // #2042 Botón baja desde Opera.
                    btnBajaServicio.Visible = false;
                }
                else
                {
                    btnBajaServicio.Visible = false;
                    //#2095:Pago Anticipado btnAltaCaldera.Visible = false;
                }
                btnBajaServicio.Visible = false;
                //#2095:Pago Anticipado
                //btnAltaCaldera.Visible = false;

                

                if (!String.IsNullOrEmpty(mantenimientoDTO.BCS))
                {
                    if (mantenimientoDTO.BCS.ToString().ToUpper() == "C" && (mantenimientoDTO.FEC_BAJA_SERVICIO == null || lblFecBajaServInfo.Text == "01/01/1900"))
                    {
                        if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                        {
                            btnBajaServicio.Visible = false;//true;
                            // (10/10/2014) Lo quitamos a petición de Rubén vía mail.
                            // #2042 Botón baja desde Opera.
                            btnBajaServicio.Visible = false;
                        }
                    }
                }
                else
                {
                    if ((mantenimientoDTO.DESEFV.IndexOf("Independente") > 0 || mantenimientoDTO.DESEFV.IndexOf("Independiente") > 0) && (mantenimientoDTO.FEC_BAJA_SERVICIO == null || lblFecBajaServInfo.Text == "01/01/1900"))
                    {
                        if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                        {
                            btnBajaServicio.Visible = false;//true;
                            // (10/10/2014) Lo quitamos a petición de Rubén vía mail.
                            // #2042 Botón baja desde Opera.
                            btnBajaServicio.Visible = false;
                        }
                    }
                }

                //If Not String.IsNullOrEmpty(Reader("BCS").ToString()) Then
                //If UCase(Reader("BCS").ToString().Substring(0, 1)) = "C" And (String.IsNullOrEmpty(Reader("FEC_BAJA_SERVICIO").ToString()) Or txt_FecBajaSer.Text = "01/01/1900") Then

                btnNuevaSolicitud.Enabled = true;
                lblCodContratoInfo.Text = mantenimientoDTO.COD_CONTRATO_SIC.ToString();
                lblEstadoContratoInfo.Text = mantenimientoDTO.ESTADO_CONTRATO;
                //lblEFVInfo.Text = mantenimientoDTO.CODEFV;
                lblEFVInfo.Text = mantenimientoDTO.DESEFV;
                hdnCodEFV.Value = mantenimientoDTO.CODEFV;
                //#2095:Pago Anticipado
                lblSolcent.Text = mantenimientoDTO.SOLCENT;

                // 23/02/2017 Cargamos el combo de los subtipos de nueva solicitud, para que filtre por efv.
                CargaComboSubtipoNuevaSolicitud();

                //TEL:lblTelefonoContacto.Text = mantenimientoDTO.TELEFONO1 + "/" + mantenimientoDTO.TELEFONO2;
                if (!string.IsNullOrEmpty(mantenimientoDTO.NUM_TEL_CLIENTE))
                {
                    lblTelefonoContacto.Text = mantenimientoDTO.NUM_TEL_CLIENTE;
                }

                if (!string.IsNullOrEmpty(mantenimientoDTO.NUM_MOVIL))
                {
                    if (!string.IsNullOrEmpty(lblTelefonoContacto.Text))
                    {
                        lblTelefonoContacto.Text += "/";
                    }
                    lblTelefonoContacto.Text += mantenimientoDTO.NUM_MOVIL;
                }


                // *****************************************************************
                if (mantenimientoDTO.BAJA_SOLICITADA) { lblBajaSolicitada.Text = "S"; }
                else { lblBajaSolicitada.Text = "N"; }
                // *****************************************************************

                //lblEFVInfo.Text = "Vaillant // TURBOTEC PLUS BAJO NOX VMW ES 21/245/4-5 NATURAL // Precio 2";
                ////txtEditarObservacionesAnteriores.Text = mantenimientoDTO.OBSERVACIONES;
                ////txtEditarObservacionesAnteriores.Enabled = false;

                string Nombre = mantenimientoDTO.NOM_TITULAR.ToString() + " " + mantenimientoDTO.APELLIDO1.ToString() + " " + mantenimientoDTO.APELLIDO2.ToString();
                if (mantenimientoDTO.DNI != null) { Nombre += " " + mantenimientoDTO.DNI.ToString(); hdnDNI.Value = mantenimientoDTO.DNI.ToString(); }

                lblClienteInfo.Text = Nombre;
                string Suministro = mantenimientoDTO.TIP_VIA_PUBLICA.ToString() + " " + mantenimientoDTO.NOM_CALLE.ToString() + " " + mantenimientoDTO.COD_PORTAL.ToString() + ", " + mantenimientoDTO.TIP_BIS.ToString() + ", " + mantenimientoDTO.TIP_ESCALERA.ToString() + ", " + mantenimientoDTO.TIP_PISO.ToString() + " " + mantenimientoDTO.TIP_MANO.ToString() + ", " + mantenimientoDTO.COD_POSTAL.ToString() + ", " + mantenimientoDTO.COD_POBLACION.ToString() + ", " + mantenimientoDTO.COD_PROVINCIA.ToString();
                hdnCP.Value = mantenimientoDTO.COD_POSTAL.ToString();
                lblSuministroInfo.Text = Suministro;

                //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                lblEmail.Text = mantenimientoDTO.EMAIL;
                //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email

                lblIdSolicitudInfo.Text = IdSolicitud;

                //EAF MOD BEG 10.06.2014 Añadir categoria ultima visita en datos del Mantenimiento
                Visitas visitas = new Visitas();

                //Boolean Acceso = usuarios.ObtenerUsuarioCompleto(this.txtUsuario.Text, this.txtPassword.Text);
                VisitaDTO visitasDTO = new VisitaDTO();
                visitasDTO = visitas.DatosVisitas(mantenimientoDTO.COD_CONTRATO_SIC.ToString(), mantenimientoDTO.COD_ULTIMA_VISITA.ToString());
                if (visitasDTO.CategoriaVisita != null)
                {
                    lblCategoriaVisita.Text = (String)visitasDTO.CategoriaVisita.ToString();
                }

                //EAF MOD END 10.06.2014 Añadir categoria ultima visita en datos del Mantenimiento





                string proveedor;
                proveedor = mantenimientoDTO.PROVEEDOR.ToString();

                if (!String.IsNullOrEmpty(proveedor))
                {
                    if (!String.IsNullOrEmpty(proveedor))
                    {
                        DataSet Datos = new DataSet();
                        Datos = GetDatosProveedor(proveedor);
                        if (Datos.Tables.Count > 0)
                        {
                            //if (!String.IsNullOrEmpty(Datos.Tables[0].Rows[0]["EMAIL.ToString())) { txt_EmailProDet.Text = Datos.Tables[0].Rows[0]["EMAIL"]; }
                            if (!String.IsNullOrEmpty(Datos.Tables[0].Rows[0]["TELEFONO"].ToString())) { lblProvTelInfo.Text = Datos.Tables[0].Rows[0]["TELEFONO"].ToString(); }

                            if (!String.IsNullOrEmpty(Datos.Tables[0].Rows[0]["NOMBRE"].ToString())) { lblProveedorInfo.Text = Datos.Tables[0].Rows[0]["NOMBRE"].ToString(); }
                        }
                    }
                }
                else
                {
                    lblCodContratoInfo.Text = "";
                }

                ////Kintell 14/01/10
                string proveedorAveria;
                proveedorAveria = mantenimientoDTO.PROVEEDOR_AVERIA.ToString();
                string proveedorInspeccion;
                proveedorInspeccion = mantenimientoDTO.PROVEEDOR_INSPECCION.ToString();
                if (!String.IsNullOrEmpty(proveedorAveria))
                {
                    DataSet Datos = new DataSet();
                    Datos = GetDatosProveedor(proveedorAveria);
                    if (Datos.Tables.Count > 0)
                    {
                        //txt_EmailProAveria.Text = Datos.Tables[0].Rows[0]["EMAIL.ToString();
                        lblProvAveriaTelInfo.Text = Datos.Tables[0].Rows[0]["TELEFONO"].ToString();

                        lblProvAveriaInfo.Text = Datos.Tables[0].Rows[0]["NOMBRE"].ToString();
                    }
                    Datos = new DataSet();
                    Datos = GetDatosProveedor(proveedorInspeccion);
                    if (Datos.Tables.Count > 0)
                    {
                        //txt_EmailProAveria.Text = Datos.Tables[0].Rows[0]["EMAIL.ToString();
                        lblProvAveriaTelInfo.Text += "/" + Datos.Tables[0].Rows[0]["TELEFONO"].ToString();

                        lblProvAveriaInfo.Text += "/" + Datos.Tables[0].Rows[0]["NOMBRE"].ToString();
                    }
                }
                else
                {
                    //txt_ContratoDet.Text = ""
                }

                //lblEstadoContratoInfo.Text = mantenimientoDTO.DES_ESTADO.ToString();
                //lblUrgenciaInfo.Text = mantenimientoDTO.URGENCIA.ToString();

                lblCODRECEPTORInfo.Text = mantenimientoDTO.COD_RECEPTOR.ToString();

                if (!String.IsNullOrEmpty(mantenimientoDTO.FEC_LIMITE_VISITA.ToString()))
                {
                    lblFecLimiteVisInfo.Text = mantenimientoDTO.FEC_LIMITE_VISITA.ToString().Substring(0, 10);
                }

                string servicio;
                string T1;
                string T2;
                string T5;

                if (mantenimientoDTO.T1.ToString() == "S")
                {
                    T1 = Resources.TextosJavaScript.TEXTO_SERVICIO_MANTENIMIENTO_GAS_CALEFACCION;// "Servicio Mantenimiento Gas Calefacción";
                }
                else
                {
                    T1 = "";
                }

                if (mantenimientoDTO.T2.ToString() == "S")
                {
                    T2 = Resources.TextosJavaScript.TEXTO_SERVICIO_MANTENIMIENTO_GAS;//"Servicio Mantenimiento Gas";
                }
                else
                {
                    T2 = "";
                }

                if (mantenimientoDTO.T5.ToString() == "S")
                {
                    T5 = Resources.TextosJavaScript.TEXTO_CON_PAGO_FRACCIONADO;//"con pago fraccionado";
                }
                else
                {
                    T5 = "";
                }

                servicio = T1 + T2 + " " + T5;

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

                    //lblServicioInfo.ForeColor = Color.Black;
                }

                lblServicioInfo.Text = servicio;

              //  if (lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") < 0)
               // {
                 //   RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_AVERIA_GAS_CONFORT"), -1);
                   // RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_AVERIA_INCORRECTA_GAS_CONFORT"), -1);
                //}
                //else
                //{
                //    RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_AVERIA_MANTENIMIENTO, -1);
                //    RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_AVERIA_INCORRECTA, -1);
                //}

                lblCalderaInfo.Text = "";
                lblFecAltaServInfo.Text = mantenimientoDTO.FEC_ALTA_SERVICIO.ToString().Substring(0, 10);
                if (mantenimientoDTO.FEC_BAJA_SERVICIO.ToString() != "") { lblFecBajaServInfo.Text = mantenimientoDTO.FEC_BAJA_SERVICIO.ToString().Substring(0, 10); }
                if (!String.IsNullOrEmpty(lblFecBajaServInfo.Text) && lblFecBajaServInfo.Text != "01/01/1900")
                {
                    lbl_Mensaje.Text = Resources.TextosJavaScript.SERVICIO_DADO_DE_BAJA;// "EL SERVICIO ESTA DADO DE BAJA";

                    string SQL = "select case when dateadd(month,-6,getdate()) < max(time_stamp) then '1' else '0' end as val, (select case when dateadd(month,6,getdate()) < max(h.fecha) then '1' else '0' end as Valido from visita v inner join visita_historico h on v.cod_contrato=h.cod_contrato and v.cod_visita=h.cod_visita inner join TIPO_ESTADO_VISITA e on h.COD_ESTADO_NUEVO=e.COD_ESTADO where v.cod_contrato='" + lblCodContratoInfo.Text + "' and COD_ESTADO_NUEVO IN ('02','10')) as val1 from solicitudes s inner join historico h on s.id_solicitud=h.id_solicitud inner join estado_solicitud e on h.estado_solicitud=e.codigo where cod_contrato='" + lblCodContratoInfo.Text + "' and estado_final=1 and subtipo_solicitud IN ('001') ";
                    SolicitudesDB db = new SolicitudesDB();
                    DataSet datos = db.ObtenerDatosDesdeSentencia(SQL);
                    if ((datos.Tables[0].Rows.Count > 0))
                    {
                        //hdnSubtipo6meses.Value = datos.Tables[0].Rows[0][0] + ";" + datos.Tables[0].Rows[0][1];
                        if (datos.Tables[0].Rows[0][0].ToString().Equals("0") && datos.Tables[0].Rows[0][1].ToString().Equals("0"))
                        {
                            btnAceptarEditarSolicitud.Enabled = false;
                            btnNuevaSolicitud.Enabled = false;
                            btnAbrirSolicitudProveedor.Enabled = false;
                            //btnNuevaReclamacion.Visible = false;
                        }
                        else
                        {
                            //12/12/2017 BGN 6 meses de COBERTURA
                            if (Usuarios.EsTelefono(this.CodPerfil))
                            {
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_AVERIA_MANTENIMIENTO));
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_SOLICITUD_DE_VISITA));
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_REVISION_POR_PRECINTE));
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_VISITA_SUPERVISION));
                                //BGN ADD BEG 11/02/2020: [Redmine:#21965] Servicio Supervision
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_AVERIA_SUPERVISION));
                                //BGN ADD END 11/02/2020: [Redmine:#21965] Servicio Supervision
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_ACEPTACION_DE_PRESUPUESTO));
                                ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION));
                                // 26/01/2016 Vemos si la baja es por reclamación o no.
                                SQL = "select cod_contrato from RECLAMACION_CONTRATO where cod_contrato='" + lblCodContratoInfo.Text + "'";
                                DataSet datosReclamacion = db.ObtenerDatosDesdeSentencia(SQL);
                                if (datosReclamacion.Tables[0].Rows.Count>0)
                                {
                                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_ATENCION_CONTRATO_BAJA_POR_RECLAMACION);//"ATENCIÓN!!!\\nCONTRATO DADO DE BAJA POR RECLAMACION\\nNo se permite dar de alta solicitudes."); //AveriaAbierta()
                                    btnNuevaSolicitud.Enabled = false;
                                }
                                else
                                {
                                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_ATENCION_CONTRATO_BAJA);//"ATENCIÓN!!!\\nCONTRATO DADO DE BAJA\\nSolo se permite operar con solicitudes de Visitas o Averias incorrectas."); //AveriaAbierta()
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Usuarios.EsTelefono(this.CodPerfil))
                        {
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_AVERIA_MANTENIMIENTO));
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_SOLICITUD_DE_VISITA));
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_REVISION_POR_PRECINTE));
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_VISITA_SUPERVISION));
                            //BGN ADD BEG 11/02/2020: [Redmine:#21965] Servicio Supervision
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_AVERIA_SUPERVISION));
                            //BGN ADD END 11/02/2020: [Redmine:#21965] Servicio Supervision
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_ACEPTACION_DE_PRESUPUESTO));
                            ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText(Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION));

                            // 26/01/2016 Vemos si la baja es por reclamación o no.
                            SQL = "select cod_contrato from RECLAMACION_CONTRATO where cod_contrato='" + lblCodContratoInfo.Text + "'";
                            DataSet datosReclamacion = db.ObtenerDatosDesdeSentencia(SQL);
                            if (datosReclamacion.Tables[0].Rows.Count > 0)
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_ATENCION_CONTRATO_BAJA_POR_RECLAMACION);//"ATENCIÓN!!!\\nCONTRATO DADO DE BAJA POR RECLAMACION\\nNo se permite dar de alta solicitudes."); //AveriaAbierta()
                                btnNuevaSolicitud.Enabled = false;
                            }
                            else
                            {
                                MostrarMensaje(Resources.TextosJavaScript.TEXTO_ATENCION_CONTRATO_BAJA);//"ATENCIÓN!!!\\nCONTRATO DADO DE BAJA\\nSolo se permite operar con solicitudes de Visitas o Averias incorrectas."); //AveriaAbierta()
                            }
                        }
                    }

                    //btnAceptarEditarSolicitud.Enabled = false;
                    //btnNuevaSolicitud.Enabled = false;
                    
                    // Kintell 18/10/2017
                    // Si el contrato tiene de baja el servicio, no le permitimos dar de alta solicitud alguna.
                    ddlSubtipo.Items.Clear();
                                        
                }
                else
                {
                    lbl_Mensaje.Text = "";

                    btnAceptarEditarSolicitud.Enabled = true;
                    btnNuevaSolicitud.Enabled = true;
                    //btnNuevaReclamacion.Visible = true;
                    //this.btn_CrearSolicitud.Enabled = true;
                    //this.btn_ModificarSolicitud.Enabled = true;

                }

                HabilitarBotonAltaCalderas(mantenimientoDTO);

                if (!String.IsNullOrEmpty(mantenimientoDTO.OBSERVACIONES)) { lblObservacionesInfo.Text = mantenimientoDTO.OBSERVACIONES.ToString(); }




                //********************************************************************************************************************
                // 12/11/2013
                // TEMA NUMERO VISITA SMG AMPLIADO.
                // SI ES SMG AMPLIADO O SMG AMPLIADO INDEPENDIENTE Y TIENE MAS DE DOS AVERIAS RESUELTAS NO PERMITIMOS CREAR SOLICITUD
                EstadosSolicitudDB objEstadosSolicitudesDB = new EstadosSolicitudDB();
                DataSet dtEfvNumeroVisitas = objEstadosSolicitudesDB.GetEfvNumerovisitaSmgAmpliado(lblCodContratoInfo.Text);

                if (dtEfvNumeroVisitas.Tables[0].Rows.Count > 0)
                {
                    lblEFVInfo.Text = dtEfvNumeroVisitas.Tables[0].Rows[0].ItemArray[1].ToString();
                    if (dtEfvNumeroVisitas.Tables[0].Rows[0].ItemArray[0] == null)
                    {
                        lblNAveriasResueltas.Text = "0";
                    }
                    else
                    {
                        lblNAveriasResueltas.Text = dtEfvNumeroVisitas.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                    hdnEsSMGAmpliado.Value = dtEfvNumeroVisitas.Tables[0].Rows[0].ItemArray[2].ToString();
                    hdnEsSMGAmpliadoIndependiente.Value = dtEfvNumeroVisitas.Tables[0].Rows[0].ItemArray[3].ToString();
                    if (lblNAveriasResueltas.Text == "") { lblNAveriasResueltas.Text = "0"; }
                    if (int.Parse(lblNAveriasResueltas.Text.ToString()) > 2)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_CONTRATO_MAS_DE_DOS_AVERIAS);//"CONTRATO SMG AMPLIADO CON MAS DE DOS AVERIAS RESUELTAS EN EL AÑO. El importe total de las siguientes averías será asumido por el cliente. Avisar al cliente.");
                    }
                }
                //********************************************************************************************************************
                // (29/10/2014).
                // Si han pasado menos de 21 dias desde que se hizo la visita no se permite generar averias ni averias incorrectas.
                // Si han pasado menos de 30 dias desde que se ha reparado una averia no dejamos generar avería.
                // Si tiene visita hecha y avería reparada no dejamos generar avería.
                if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                {
                    IDataReader drFechaVisitaFechaCierreSolicitud = null;
                    try
                    {
                        VisitasDB objVisitasDB = new VisitasDB();
                        drFechaVisitaFechaCierreSolicitud = objVisitasDB.ObtenerFechaVisitaFechaCierreSolicitudAveria(lblCodContratoInfo.Text, hdnCodVisita.Value, "");
                        
                        DateTime fechaVisita = DateTime.Parse("01/01/1900");
                        string estadoSolicitud = "";
                        DateTime fechaCierreSolicitud = DateTime.Parse("01/01/1900");
                        string estadoVisita = "";
                        int categoriaVisita = 1;

                        while (drFechaVisitaFechaCierreSolicitud.Read())
                        {
                            // No limitar para las de GC. (01/03/2016 Mail Alex 12:24).
                           // if (lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") < 0)
                            //{
                                if (DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA") != null)
                                {
                                    estadoVisita = DataBaseUtils.GetDataReaderColumnValue(drFechaVisitaFechaCierreSolicitud, "COD_ESTADO_VISITA").ToString();
                                }
                                
                                lblEstadoVisitaActual.Text = "Estado Visita Actual: " + objVisitasDB.ObtenerDescripcionEstadovisitaPorCodigo(Int64.Parse(estadoVisita),usuarioDTO.IdIdioma);

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


                                //12/02/2016
                                // Si el estado de la solicitud esta en blanco es que no tiene averia anterior por lo que no dejamos abrir avería incorrecta.
                                if (estadoSolicitud == "")
                                {
                                    RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_AVERIA_INCORRECTA, -1);
                                    RemoveElement(ddlSubtipo, "Avería Incorrecta Gas Confort", -1);
                                }

                                Boolean mas21 = false;
                                Boolean mas21Larga = false;
                                Boolean mas30 = false;

                                if (fechaVisita.Year.ToString() != "1900")
                                {
                                    if (fechaVisita.AddDays(21) > DateTime.Now)
                                    {
                                        if (estadoVisita == "02" || estadoVisita == "09")
                                        {
                                            if (categoriaVisita != 1)
                                            {
                                                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_AVERIA_MANTENIMIENTO, -1);
                                                RemoveElement(ddlSubtipo, "Avería Gas Confort", -1);
                                                mas21Larga = true;
                                            }
                                            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_AVERIA_INCORRECTA, -1);
                                            RemoveElement(ddlSubtipo, "Avería Incorrecta Gas Confort", -1);
                                            mas21 = true;
                                        }
                                        // Si es reducida y cerrada en 21 días solo dejamos subtipo de averia de limpieza de quemadores.
                                        //10/03/2015
                                        if (categoriaVisita == 1)
                                        {
                                            //12/06/2015 Administrador puede hacer todo.
                                            if (!Usuarios.EsAdministrador(this.CodPerfil))
                                            {
                                                TiposAveriaDB objTiposAveriaDB = new TiposAveriaDB();
                                                ddlAveria.DataSource = objTiposAveriaDB.GetTipoAveriaMasDe21DiasDesdeCierreVisita(usuarioDTO.IdIdioma);
                                                ddlAveria.DataTextField = "descripcion_averia";
                                                ddlAveria.DataValueField = "cod_averia";
                                                ddlAveria.DataBind();

                                                ListItem defaultItem = new ListItem();
                                                defaultItem.Value = "-1";
                                                defaultItem.Text = Resources.TextosJavaScript.SELECCIONE_TIPO_AVERIA;// "Seleccione un tipo de averia";
                                                ddlAveria.Items.Insert(0, defaultItem);
                                            }
                                        }

                                    }
                                }

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

                                if (hdnModificandoSolicitud.Value == "False" || hdnModificandoSolicitud.Value == "")
                                {

                                    if (mas30)
                                    {
                                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_ABRIR_AVERIAS_30_DIAS);//"No se pueden abrir averias al tener una avería abierta hace menos de 30 días. \\n EL PROVEEDOR DEBERA DE PONERSE EN CONTACTO CON SU RESPONSABLE.");
                                    }
                                    else if (mas21 || mas21Larga)
                                    {
                                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_ABRIR_AVERIAS_21_DIAS);//"No se pueden abrir averias al tener una visita cerrada hace menos de 21 días. \\n EL PROVEEDOR DEBERA DE PONERSE EN CONTACTO CON SU RESPONSABLE.");
                                    }

                                }
                            //}
                        }


                        

                    }
                    finally
                    {
                        if (drFechaVisitaFechaCierreSolicitud != null)
                        {
                            drFechaVisitaFechaCierreSolicitud.Close();
                        }
                    }
                }
                if (Usuarios.EsAdministrador(this.CodPerfil))
                {
                    btnBajaServicio.Visible = false;//true;
                }


                //16/01/2015: Importe que debe de pagar el cliente si se da de baja.
                lblLeyendaImporteSiBaja.Visible = false;
                lblImporteSiBaja.Visible = false;
                if (Usuarios.EsAdministrador(this.CodPerfil) || Usuarios.EsTelefono(this.CodPerfil))
                {
                    lblLeyendaImporteSiBaja.Visible = true;
                    lblImporteSiBaja.Visible = true;
                    if (hdnEsSMGAmpliado.Value == "1" || hdnEsSMGAmpliadoIndependiente.Value == "1" || hdnEsSMGAmpliado.Value == "True" || hdnEsSMGAmpliadoIndependiente.Value == "True")
                    {
                        lblImporteSiBaja.Text = mantenimientoDTO.PAGAR_SI_BAJA.ToString() + " €";
                    }
                    else
                    {
                        lblImporteSiBaja.Text = "N/A";
                    }
                }

            }
            catch (Exception err)
            {
                // 19/07/2016 Tema Idioma.
                //MostrarMensaje("CONTRATO NO ENCONTRADO (Al cargar datos del cliente)");

                MostrarMensaje(Resources.TextosJavaScript.CONTRATO_NO_ENCONTRADO);
                //InicializarControles();
            }
        }

        /// <summary>
        /// Habilita o desabilita el botón de calderas en función de los datos del mantenimiento.
        /// 
        /// </summary>
        /// <param name="mantenimientoDTO"></param>
        private void HabilitarBotonAltaCalderas(MantenimientoDTO mantenimientoDTO)
        {
            bool bHabilitar = false;
            // Si tiene establecida la fecha de baja de servicio quiere decir que no se puede
            // dar de alta una solicitud de calderas para el mismo.
            if (mantenimientoDTO.FEC_BAJA_SERVICIO.HasValue
                && mantenimientoDTO.FEC_BAJA_SERVICIO.ToString().Substring(0, 10) != "01/01/1900")
            {
                bHabilitar = false;
            }
            else
            {
                if (string.IsNullOrEmpty(mantenimientoDTO.SCORING))
                {
                    bHabilitar = false;
                    lblScoring.Text = "N";
                }
                else
                {
                    if (mantenimientoDTO.SCORING.ToUpper() == "S")
                    {
                        bHabilitar = true;
                    }
                    else
                    {
                        bHabilitar = false;
                    }

                    lblScoring.Text = mantenimientoDTO.SCORING.ToString();
                }
            }

            //#2095 Pago Anticipado
            //btnAltaCaldera.Enabled = bHabilitar;
        }


        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_ALERTA, alerta, false);
        }

        protected void CargarDatosVisitas(String codVisita, String CodContrato)
        {
            try
            {
                InicializarControlesVisita();
                string connectionString = WebConfigurationManager.ConnectionStrings["OPCOMCMD"].ConnectionString;

                //Dim selectSQL As String = "SELECT dbo.VISITAS.REPARACION, dbo.VISITAS.TIPO_REPARACION, "
                //selectSQL &= "dbo.VISITAS.TIEMPO_MANO_OBRA, dbo.VISITAS.IMPORTE_REPARACION, "
                //selectSQL &= "dbo.VISITAS.IMPORTE_MANO_OBRA_ADICIONAL, dbo.VISITAS.IMPORTE_MATERIALES_ADICIONAL, "
                //selectSQL &= "dbo.VISITAS.OBSERVACIONES, dbo.VISITAS.DES_ESTADO,dbo.VISITAS.COD_VISITA, "
                //selectSQL &= "dbo.VISITAS.FEC_VISITA, dbo.VISITAS.FEC_LIMITE_VISITA "
                //selectSQL &= "FROM dbo.VISITAS "
                //selectSQL &= "WHERE dbo.VISITAS.COD_VISITA = '" & codvisita & "' AND COD_CONTRATO = '" & codcontrato & "'"
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                string selectSQL = "SELECT REPARACION.ID_REPARACION, TIPO_REPARACION.DESC_TIPO_REPARACION, ";
                selectSQL += "TIPO_TIEMPO_MANO_OBRA.DESC_TIPO_TIEMPO_MANO_OBRA AS TIEMPO_MANO_OBRA, REPARACION.IMPORTE_REPARACION, ";
                selectSQL += "REPARACION.IMPORTE_MANO_OBRA_ADICIONAL, REPARACION.IMPORTE_MATERIALES_ADICIONAL, ";
                selectSQL += "VISITA.OBSERVACIONES, TIPO_ESTADO_VISITA.DES_ESTADO AS DES_ESTADO,VISITA.COD_VISITA, ";
                selectSQL += "VISITA.FEC_VISITA, VISITA.FEC_LIMITE_VISITA, VISITA.COD_BARRAS ";
                // 04052016 CAMBIO PETICION ALEX.
                selectSQL += ",VISITA.TIPOVISITA,TC.DESC_TIPO_CALDERA,TMC.DESC_MARCA_CALDERA,C.MODELO_CALDERA,C.POTENCIA,TE.NOMBRE_TECNICOEMPRESA,VISITA.PRL ";
                //*******************************
                selectSQL += "FROM VISITA ";
                //************************************************************
                selectSQL += "LEFT JOIN REPARACION ON REPARACION.ID_REPARACION=VISITA.ID_REPARACION ";
                selectSQL += "LEFT JOIN TIPO_REPARACION ON REPARACION.ID_TIPO_REPARACION=TIPO_REPARACION.ID_TIPO_REPARACION ";
                selectSQL += "LEFT JOIN TIPO_ESTADO_VISITA ON VISITA.COD_ESTADO_VISITA=TIPO_ESTADO_VISITA.COD_ESTADO AND TIPO_ESTADO_VISITA.ID_IDIOMA='" + usuarioDTO.IdIdioma + "'";
                selectSQL += "LEFT JOIN TIPO_TIEMPO_MANO_OBRA ON TIPO_TIEMPO_MANO_OBRA.ID_TIPO_TIEMPO_MANO_OBRA=REPARACION.ID_TIPO_TIEMPO_MANO_OBRA ";
                //************************************************************
                // 04052016 CAMBIO PETICION ALEX.
                selectSQL += "LEFT JOIN CALDERAS C ON VISITA.COD_CONTRATO=C.COD_CONTRATO ";
				selectSQL += "LEFT JOIN TIPO_CALDERA TC ON C.ID_TIPO_CALDERA=TC.ID_TIPO_CALDERA ";
				selectSQL += "LEFT JOIN TIPO_MARCA_CALDERA TMC ON C.ID_MARCA_CALDERA=TMC.ID_MARCA_CALDERA ";
                selectSQL += "LEFT JOIN TECNICOEMPRESA TE ON VISITA.TECNICO=TE.ID_TECNICOEMPRESA ";
                //************************************************************
                selectSQL += "WHERE VISITA.COD_VISITA = '" + codVisita + "' AND VISITA.COD_CONTRATO = '" + CodContrato + "'";
                
                // 25/04/2016 CAMBIO ALEX.
                //selectSQL = "select c.cod_contrato,tc.desc_tipo_caldera,tm.desc_marca_caldera,c.modelo_caldera,c.potencia,cv.desc_categoria_visita,te.nombre_tecnicoempresa ";
                //selectSQL += "from calderas c ";
                //selectSQL += "inner join tipo_caldera tc on c.id_tipo_caldera=tc.id_tipo_caldera ";
                //selectSQL += "inner join tipo_marca_caldera tm on c.id_marca_caldera=tm.id_marca_caldera ";
                //selectSQL += "inner join mantenimiento m on m.cod_contrato_sic=c.cod_contrato ";
                //selectSQL += "inner join visita v on v.cod_contrato=m.cod_contrato_sic and v.cod_visita=m.cod_ultima_visita ";
                //selectSQL += "inner join tipo_categoria_visita cv on v.cod_categoria_visita = cv.cod_categoria_visita ";
                //selectSQL += "left join tecnicoempresa te on v.Tecnico=te.id_tecnicoempresa ";
                //selectSQL += "where c.cod_contrato='" + CodContrato + "'";

                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(selectSQL, con);
                con.Open();
                SqlDataReader Reader;
                Reader = cmd.ExecuteReader();

                Reader.Read();

                try
                {
                    //If Not GridView1.SelectedRow.Cells(3).Text = "Cerrada" And codvisita = GridView1.Rows.Count Then
                    if (int.Parse(codVisita) >= gvVisitas.Rows.Count ||
                        (gvVisitas.Rows[gvVisitas.Rows.Count - 1].Cells[1].Text == "0" && int.Parse(codVisita) + 1 >= gvVisitas.Rows.Count))
                    {
                        btnAvisoVisita.Enabled = true;
                    }
                    else
                    {
                        btnAvisoVisita.Enabled = false;
                    }
                    //21/05/2014
                    // Botón para ver el detalle de la visita.
                    // Lo habilitamos.
                    btnDetalleVisita.Enabled = true;

                    lblEstadoVisita.Text = Reader["DES_ESTADO"].ToString();
                    // 05/06/2013
                    lblCBVisita.Text = Reader["COD_BARRAS"].ToString();
                    lblNumeroVisita.Text = Reader["COD_VISITA"].ToString();
                    if (Reader["FEC_VISITA"].ToString() != "") { lblFechaVisita.Text = Reader["FEC_VISITA"].ToString().Substring(0, 10); }
                    lblFechaLimiteVisita.Text = Reader["FEC_LIMITE_VISITA"].ToString().Substring(0, 10);

                    if (Reader["ID_REPARACION"].ToString() == "")
                    {
                        lblTieneReparacionVisita.Text = Resources.TextosJavaScript.TEXTO_NO;// "NO";
                    }
                    else
                    {
                        lblTieneReparacionVisita.Text = Resources.TextosJavaScript.TEXTO_SI;// "SI";
                    }


                    //lblTipoReparacion.Text = Reader["DESC_TIPO_REPARACION"].ToString();
                    lblTipoVisita.Text = Reader["TIPOVISITA"].ToString(); ;
                    //lblTiempoManoObraVisita.Text = Reader["TIEMPO_MANO_OBRA"].ToString();
                    lblTipoCaldera.Text = Reader["DESC_TIPO_CALDERA"].ToString(); ;
                    //lblManoObraAdicionalVisita.Text = Reader["IMPORTE_MANO_OBRA_ADICIONAL"].ToString() + " €";
                    lblMarcaCaldera.Text = Reader["DESC_MARCA_CALDERA"].ToString(); ;
                    //lblMaterialesVisita.Text = Reader["IMPORTE_MATERIALES_ADICIONAL"].ToString() + " €";
                    lblModeloCaldera.Text = Reader["MODELO_CALDERA"].ToString(); ;
                    //lblImporteVisita.Text = Reader["IMPORTE_REPARACION"].ToString() + " €";
                    lblPotencia.Text = Reader["POTENCIA"].ToString(); ;
                    lblTecnico.Text = Reader["NOMBRE_TECNICOEMPRESA"].ToString();

                    //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web     
                    if (!Reader["PRL"].Equals(DBNull.Value) && Convert.ToBoolean(Reader["PRL"]))
                    {
                        lblValorPRL.Text = Resources.TextosJavaScript.TEXTO_SI;// "SI";
                    }
                    else
                    {
                        lblValorPRL.Text = Resources.TextosJavaScript.TEXTO_NO;// "NO";
                    }
                    //BGN ADD FIN R#25266: Visualizar si una visita tiene PRL por Web

                    lblObservacionesVisita.Text = Reader["OBSERVACIONES"].ToString();

                    MostrarAviso("0");



                }
                catch (Exception err)
                {
                    //lbl_Mensaje.Text = "Datos no disponibles. Error: ";
                }
                finally
                {

                    Reader.Close();
                    con.Close();
                }
				
				try
                {
                    // Kintell carencia PMG y AG, Redmine #17446 26/06/2019.
                    if ((lblServicioInfo.Text.ToUpper().IndexOf("PACK") >= 0 ) && (lblFecBajaServInfo.Text == ""))
                    {
                        //Pack Mantenimiento Gas
                        //Asistencia Gas Iberdrola

                        DateTime fecAltaServicio = DateTime.Parse(lblFecAltaServInfo.Text);

                        if (fecAltaServicio.AddDays(15) > DateTime.Now && fecAltaServicio>DateTime.Parse("2019/08/15 00:00:00.000"))
                        {
                            btnNuevaSolicitud.Enabled = false;
                            btnNuevaReclamacion.Visible = false;
                            btnAbrirSolicitudProveedor.Enabled = false;
                            btnAbrirSolicitudInspeccionGas.Enabled = false;
                            //MostrarMensaje("Contrato en periodo de carencia.");
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected DataSet GetDatosProveedor(String Proveedor)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["OPCOMCMD"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter("MTOGASBD_GetDatosProveedor", con);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            //' Add Parameters to SPROC
            SqlParameter parameterTipoSolicitud = new SqlParameter("@Proveedor", SqlDbType.NVarChar, 3);
            parameterTipoSolicitud.Value = Proveedor;
            myCommand.SelectCommand.Parameters.Add(parameterTipoSolicitud);

            DataSet myDataSet = new DataSet();
            myCommand.Fill(myDataSet);

            return myDataSet;
        }

        protected void InicializarControlesVisita()
        {
            //VISITAS
            btnAvisoVisita.Enabled = false;
            btnDetalleVisita.Enabled = false;
            lblEstadoVisita.Text = "";
            lblNumeroVisita.Text = "";
            lblFechaVisita.Text = "";
            lblFechaLimiteVisita.Text = "";
            lblTieneReparacionVisita.Text = "";
            lblTipoVisita.Text = "";
            lblTipoCaldera.Text = "";
            lblMarcaCaldera.Text = " ";
            lblModeloCaldera.Text = " ";
            lblPotencia.Text = " ";
            lblTecnico.Text = " ";
            lblObservacionesVisita.Text = "";
            lblCBVisita.Text = "";
        }

        protected void InicializarControlesDatosContrato()
        {
            lblEFVInfo.Text = "";
            hdnCodEFV.Value = "";
            lblCodContratoInfo.Text = "";
            lblScoring.Text = "";
            lblEstadoContratoInfo.Text = "";
            lblTelefonoContacto.Text = "";
            lblCODRECEPTORInfo.Text = "";
            lblClienteInfo.Text = "";
            lblSuministroInfo.Text = "";
            lblCalderaInfo.Text = "";
            lblUrgenciaInfo.Text = "";
            lblServicioInfo.Text = "";
            lblFecAltaServInfo.Text = "";
            lblFecBajaServInfo.Text = "";
            lblFecLimiteVisInfo.Text = "";
            lblProveedorInfo.Text = "";
            lblProvTelInfo.Text = "";
            lblProvAveriaInfo.Text = "";
            lblProvAveriaTelInfo.Text = "";
            lblObservacionesInfo.Text = "";
            lbl_Mensaje.Text = "";
            lblIdSolicitudInfo.Text = "";
            //#2095:Pago Anticipado
            lblSolcent.Text = "";
            //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
            lblEmail.Text = "";
            //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
        }

        protected void InicializarControles()
        {
            InicializarControlesNuevaSolicitud();
            InicializarControlesDatosContrato();
            InicializarControlesVisita();
            InicializarListados();
            this.hdnPageIndex.Value = "1";
        }

        protected void InicializarListados()
        {
            hdnIdSolicitud.Value = "";
            hdnIdSolicitudHistorico.Value = "";
            hdnCodVisita.Value = "";

            gv_HistoricoSolicitudes.DataSource = null;
            gv_HistoricoSolicitudes.DataBind();

            gv_historicoCaracteristicas.DataSource = null;
            gv_historicoCaracteristicas.DataBind();

            gvVisitas.DataSource = null;
            gvVisitas.DataBind();

            gvSolicitudes.DataSource = null;
            gvSolicitudes.DataBind();

            gv_HistoricoSolicitudes.DataSource = null;
            gv_HistoricoSolicitudes.DataBind();
        }

        protected void InicializarControlesNuevaSolicitud()
        {
            if (ddlEstado.Items.Count > 0) { ddlEstado.SelectedIndex = 0; }

            if (ddlMotivoCancelacion.Items.Count > 0) { ddlMotivoCancelacion.SelectedIndex = 0; }
            txtEditarTelefono.Text = "";
            txtPersonaContacto.Text = "";
            cmbHorarioContacto.SelectedIndex = 0;
            ddlAveria.SelectedIndex = 0;
            chkUrgente.Checked = false;
            txtEditarObservacionesAnteriores.Text = "";
            txtEditarObservaciones.Text = "";
            //txtMarcaCaldera.Text = "";
            txtModelocaldera.Text = "";

            UsuarioDTO Usuario = Usuarios.ObtenerUsuarioLogeado();
            //si el login del usuario comienza por C, significa q es un perfil telefono de retención, en tal caso se habilita el check
            if (Usuario.Login.Substring(1).Equals("C"))
            {
                lblRetencion.Visible = true;
                ChkRetencion.Visible = true;
            }
            else
            {
                lblRetencion.Visible = false;
                ChkRetencion.Visible = false;
            }

        }

        protected void InicializarFiltros()
        {
            //Eliminamos filtros.
            txt_contrato.Text = "";
            txt_solicitud.Text = "";
            txtApellido1.Text = "";
            txtApellido2.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            txtNombre.Text = "";
            chk_Pendientes.Checked = true;
            chk_Urgente.Checked = false;
            ddl_Provincia.SelectedIndex = 0;
            ddl_Subtipo.SelectedIndex = 0;
            ddl_Estado.SelectedIndex = 0;
            // Ruben demands that with the "filtros" inizialization inizializate too the contract data.
            InicializarControlesDatosContrato();
        }

        protected void HabilitarDeshabilitarControlesPorPerfil()
        {
            // Habilitamos/Deshabilitamos filtros.
            if (Usuarios.EsTelefono(this.CodPerfil))
            {
                //No permitimos exportar a Excel.
                btnExportarExcelSolicitudes.Visible = true;
                // HABILITAMOS FILTROS
                txt_contrato.Enabled = true;
                txt_contrato.BackColor = Color.White;
                txtNombre.Enabled = true;
                txtNombre.BackColor = Color.White;
                txtApellido1.Enabled = true;
                txtApellido1.BackColor = Color.White;
                txtApellido2.Enabled = true;
                txtApellido2.BackColor = Color.White;
                // DESHABILITAMOS FILTROS
                //txt_solicitud.Enabled = false;
                //txt_solicitud.BackColor = Color.Gray;

                ddl_Poblacion.Enabled = false;
                ddl_Provincia.Enabled = false;

                //***************************************************************
                // 15/05/2015 - Cambiamos a petición de Alex.
                // Para k el teléfono pueda filtrar por estados de Gas confort.
                //***************************************************************
                ddl_Estado.Enabled = true;
                ddl_Subtipo.Enabled = true;
                //***************************************************************

                txtFechaDesde.Enabled = false;
                txtFechaDesde.BackColor = Color.Gray;
                txtFechaHasta.Enabled = false;
                txtFechaHasta.BackColor = Color.Gray;
                chk_Pendientes.Enabled = false;
                chk_Urgente.Enabled = false;
                imgFechaHasta.Disabled = true;
                img1.Disabled = true;
                // BOTON VISITAS
                btnPestanyaVisitas.Enabled = true;
                //Si el usuario conectado es de retención activar el check de Prioritario de Retención
                //If Usuarios.
                // ChkRetencion.Enabled = true;

                UsuarioDTO Usuario = Usuarios.ObtenerUsuarioLogeado();
                //si el login del usuario comienza por C, significa q es un perfil teléfono de retención, en tal caso se habilita el check
                if (Usuario.Login.Substring(1).Equals("C"))
                {
                    lblRetencion.Visible = true;
                    ChkRetencion.Visible = true;
                }
                else
                {
                    lblRetencion.Visible = false;
                    ChkRetencion.Visible = false;
                }


            }
            else if (Usuarios.EsProveedor(this.CodPerfil))
            {
                // DESHABILITAMOS FILTROS
                txtNombre.Enabled = false;
                txtNombre.BackColor = Color.Gray;
                txtApellido1.Enabled = false;
                txtApellido1.BackColor = Color.Gray;
                txtApellido2.Enabled = false;
                txtApellido2.BackColor = Color.Gray;
                // HABILITAMOS FILTROS
                txt_contrato.Enabled = true;
                txt_contrato.BackColor = Color.White;
                txt_solicitud.Enabled = true;
                txt_solicitud.BackColor = Color.White;
                ddl_Estado.Enabled = true;
                ddl_Poblacion.Enabled = true;
                ddl_Provincia.Enabled = true;
                ddl_Subtipo.Enabled = true;
                txtFechaDesde.Enabled = true;
                txtFechaDesde.BackColor = Color.White;
                txtFechaHasta.Enabled = true;
                txtFechaHasta.BackColor = Color.White;
                chk_Pendientes.Enabled = true;
                chk_Urgente.Enabled = true;
                imgFechaHasta.Disabled = false;
                img1.Disabled = false;
                // BOTON VISITAS
                btnPestanyaVisitas.Enabled = true;
                //Retencion
                lblRetencion.Visible = false;
                ChkRetencion.Visible = false;
            }
            else
            {
                // HABILITAMOS FILTROS
                txt_contrato.Enabled = true;
                txt_contrato.BackColor = Color.White;
                txtNombre.Enabled = true;
                txtNombre.BackColor = Color.White;
                txtApellido1.Enabled = true;
                txtApellido1.BackColor = Color.White;
                txtApellido2.Enabled = true;
                txtApellido2.BackColor = Color.White;
                txt_solicitud.Enabled = true;
                txt_solicitud.BackColor = Color.White;
                ddl_Estado.Enabled = true;
                ddl_Poblacion.Enabled = true;
                ddl_Provincia.Enabled = true;
                ddl_Subtipo.Enabled = true;
                txtFechaDesde.Enabled = true;
                txtFechaDesde.BackColor = Color.White;
                txtFechaHasta.Enabled = true;
                txtFechaHasta.BackColor = Color.White;
                chk_Pendientes.Enabled = true;
                chk_Urgente.Enabled = true;
                imgFechaHasta.Disabled = false;
                img1.Disabled = false;
                // BOTON VISITAS
                btnPestanyaVisitas.Enabled = true;
                //Retencion
                //Retencion
                lblRetencion.Visible = false;
                ChkRetencion.Visible = false;
            }

            if (Usuarios.EsReclamacion(this.CodPerfil))
            {
                // DESHABILITAMOS FILTROS
                txtNombre.Enabled = false;
                txtNombre.BackColor = Color.Gray;
                txtApellido1.Enabled = false;
                txtApellido1.BackColor = Color.Gray;
                txtApellido2.Enabled = false;
                txtApellido2.BackColor = Color.Gray;
                // HABILITAMOS FILTROS
                txt_contrato.Enabled = true;
                txt_contrato.BackColor = Color.White;
                txt_solicitud.Enabled = true;
                txt_solicitud.BackColor = Color.White;
                ddl_Estado.Enabled = true;
                ddl_Poblacion.Enabled = true;
                ddl_Provincia.Enabled = true;
                ddl_Subtipo.Enabled = true;
                txtFechaDesde.Enabled = true;
                txtFechaDesde.BackColor = Color.White;
                txtFechaHasta.Enabled = true;
                txtFechaHasta.BackColor = Color.White;
                chk_Pendientes.Enabled = true;
                chk_Urgente.Enabled = true;
                imgFechaHasta.Disabled = false;
                img1.Disabled = false;
                // BOTON VISITAS
                btnPestanyaVisitas.Enabled = true;
                btnNuevaSolicitud.Visible = true;
                //Retencion
                ChkRetencion.Enabled = false;
            }
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            InicializarFiltros();
            InicializarControles();
            //20200507 BGN ADD BEG R#18312: Limpiar filtro cuando el acceso es desde Delta
            if (!String.IsNullOrEmpty(Request.QueryString["CONTRATO"]))
            {
                Uri u = new Uri(Request.Url.AbsoluteUri);
                HttpContext.Current.Response.Redirect(u.AbsolutePath, false);
            }
            //20200507 BGN ADD END R#18312: Limpiar filtro cuando el acceso es desde Delta
        }

        protected void CargaSolicitudes(int Pagina)
        {
            try
            {
                // Para las llamadas de cortesía
                Int32 intPageIndex = Pagina;
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;

                string cod_proveedor = "";
                if (Usuarios.EsTelefono(this.CodPerfil))
                {
                    cod_proveedor = "TEL";
                }
                else
                {
                    if (!Usuarios.EsAdministrador(this.CodPerfil))
                    {
                        cod_proveedor = this.CodProveedor;
                    }
                }

                if (Usuarios.EsReclamacion(this.CodPerfil))
                {
                    cod_proveedor = "REC";
                }

                String cod_contrato = txt_contrato.Text;
                Boolean consultaPendietes = chk_Pendientes.Checked;
                String cod_tipo = "001";
                String cod_subtipo = ddl_Subtipo.SelectedValue;
                String cod_estado = ddl_Estado.SelectedValue;

                if (cod_subtipo == "") { cod_subtipo = "-1"; }
                if (cod_estado == "") { cod_estado = "-1"; }

                String fechaDesde = txtFechaDesde.Text;
                String fechaHasta = txtFechaHasta.Text;

                if (fechaDesde != "") { if (fechaDesde.Length == 9) { fechaDesde = "0" + fechaDesde; } fechaDesde = fechaDesde.Substring(6, 4) + "-" + fechaDesde.Substring(3, 2) + "-" + fechaDesde.Substring(0, 2); }
                if (fechaHasta != "") { if (fechaHasta.Length == 9) { fechaHasta = "0" + fechaHasta; } fechaHasta = fechaHasta.Substring(6, 4) + "-" + fechaHasta.Substring(3, 2) + "-" + fechaHasta.Substring(0, 2); }

                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                String Urgente = "False";
                if (chk_Urgente.Checked) { Urgente = "True"; }

                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                CultureInfo culture;
                //culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);//"FR-fr");
                culture = new CultureInfo(CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_CULTURA).ToString());
            
                //if (cod_proveedor == "TEL" && (txt_contrato.Text != "" || txtApellido1.Text != "" || txtApellido2.Text != "" || txtNombre.Text != ""))  {cod_proveedor = ""; }

                //DataSet dsSolicitudes = objSolicitudesDB.GetSolicitudesPorProveedorCalderas(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Desde.ToString(), Hasta.ToString(), this.CodPerfil.ToString(), txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, "0", usuarioDTO.IdIdioma.ToString(), usuarioDTO.Pais.ToString());//culture.ToString());
                SolicitudDB objSol = new SolicitudDB();
                DataTable dtSolicitudes = objSol.ObtenerSolicitudes(cod_proveedor, cod_contrato, consultaPendietes.ToString(), cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue.ToString(), Urgente, Desde.ToString(), Hasta.ToString(), this.CodPerfil.ToString(), txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, "0", usuarioDTO.IdIdioma.ToString(), usuarioDTO.Pais.ToString(),txtDNI.Text,txtCUI.Text);//culture.ToString());
                
                gvSolicitudes.Columns[0].Visible = true;
                gvSolicitudes.Columns[1].Visible = true;
                gvSolicitudes.Columns[2].Visible = true;

                gvSolicitudes.DataSource = dtSolicitudes;//dsSolicitudes.Tables[0];//
                gvSolicitudes.DataBind();

                if (gvSolicitudes.Rows.Count == 1 || (Usuarios.EsTelefono(this.CodPerfil)) && gvSolicitudes.Rows.Count > 0)
                {
                    hdnIdSolicitud.Value = dtSolicitudes.Rows[0].ItemArray[1].ToString(); //dsSolicitudes.Tables[0].Rows[0].ItemArray[1].ToString(); //
                }

                if ((Desde == 0 || Desde == 1) && gvSolicitudes.Rows.Count > 0)
                {
                    //String contador = gvSolicitudes.Rows.Count.ToString();
                    //if (gvSolicitudes.Rows.Count > PageSize - 1)
                    //{
                    //    contador = objSolicitudesDB.GetSolicitudesPorProveedorContador(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Desde.ToString(), Hasta.ToString(), this.CodPerfil.ToString(), txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, "0",culture.ToString());
                    //}
                    String contador = dtSolicitudes.Rows[0]["conteo"].ToString();
                    lblNumEncontrados.Text = contador.ToString();
                    hdnPageCount.Value = ObtenerNumPaginas(Convert.ToInt32(contador)).ToString();
                }
                else
                {
                   // lblNumEncontrados.Text = "0";
                }
                lblNumEncontrados1.Text = (Desde + 1).ToString();
                if (Hasta <= int.Parse(lblNumEncontrados.Text))
                {
                    lblNumRegistros.Text = Hasta.ToString();
                }
                else
                {
                    lblNumRegistros.Text = lblNumEncontrados.Text.ToString();
                }
                CargarPaginador(Pagina);
            }
            catch (Exception ex)
            {
                // TODO: aquí nos estamos comiendo la excepción ver qué hacer
                gvSolicitudes.DataSource = null;
                gvSolicitudes.DataBind();
            }
        }



        protected void CargaHistoricoCaracteristicas(String id_movimiento)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            HistoricoDB objHistoricoDB = new HistoricoDB();
            DataSet dsHistorico = objHistoricoDB.GetHistoricoCaracteristicasPorMovimiento(id_movimiento,usuarioDTO.IdIdioma);

            gv_historicoCaracteristicas.Columns[0].Visible = true;

            gv_historicoCaracteristicas.DataSource = dsHistorico;
            gv_historicoCaracteristicas.DataBind();

            gv_historicoCaracteristicas.Columns[0].Visible = false;
        }

        protected void CargaHistorico(String id_solicitud)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            HistoricoDB objHistoricoDB = new HistoricoDB();
            DataSet dsHistorico = objHistoricoDB.GetHistoricoSolicitud(id_solicitud,usuarioDTO.IdIdioma);

            gv_HistoricoSolicitudes.Columns[0].Visible = true;
            gv_HistoricoSolicitudes.Columns[1].Visible = true;

            gv_HistoricoSolicitudes.DataSource = dsHistorico;
            gv_HistoricoSolicitudes.DataBind();

            //'' las oculto despues del databind para que me mantenga los datos
            gv_HistoricoSolicitudes.Columns[0].Visible = false;
            gv_HistoricoSolicitudes.Columns[1].Visible = false;

            grdHistorical.Columns[0].Visible = true;
            grdHistorical.Columns[1].Visible = true;
            grdHistorical.DataSource = dsHistorico;
            grdHistorical.DataBind();
            //'' las oculto despues del databind para que me mantenga los datos
            grdHistorical.Columns[0].Visible = false;
            grdHistorical.Columns[1].Visible = false;

            if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil) || Usuarios.EsReclamacion(this.CodPerfil))
            {
                CargarVisitas(lblCodContratoInfo.Text);
            }


            //// We catch the aviso_solicitudes from the solicitudes table.
            //SolicitudesDB objSolicitudesDB = new SolicitudesDB();
            //string Aviso = objSolicitudesDB.ObtenerAvisoSolicitud(id_solicitud);
            //Aviso = Aviso.Replace(";#", @"\n");//Convert.ToChar(13).ToString());
            //// If we have something in the field, we should show the "aviso".
            //if (Aviso != "" && Aviso != "0")
            //{
            //    // Here we must to show the field.
            //    string alerta = "<script type='text/javascript'>MostrarDatos('" + Aviso + "','" + id_solicitud + "');</script>";

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AVISO", alerta, false);
            //    //MostrarMensaje("AVISO:" + Aviso);
            //}
            //else
            //{
            //    // Here we must to hide the field.
            //}

        }

        protected void CargarVisitas(String contrato)
        {
            UsuarioDTO usuarioDTO = null;
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            IDataReader dr = null;
            IDataReader drUrgencia = null;
            try
            {
                dr = Contrato.ObtenerVisitas(contrato, (Int16)usuarioDTO.IdIdioma);
                DataTable dt = new DataTable();
                dt.Load(dr);
                
                gvVisitas.DataSource = dt;
                gvVisitas.DataBind();

                Visitas visitas = new Visitas();
                drUrgencia = visitas.ObtenerTipoUrgencia(dt.Rows[0].ItemArray[9].ToString(), (int)usuarioDTO.IdIdioma);
                while (drUrgencia.Read())
                {
                    this.lblUrgenciaInfo.Text = (string)DataBaseUtils.GetDataReaderColumnValue(drUrgencia, "DESC_TIPO_URGENCIA");
                }
            }
            catch (Exception err)
            {
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                if (drUrgencia != null)
                {
                    drUrgencia.Close();
                }
            }
            
        }
        #endregion


        public void HandleRemoveUserControl(object sender, EventArgs e)
        {

        }

        //private override void DropDownList.Remove(ListItem item)
        //{
        //    string a = "";
        //}

        #region Implementación de los eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ddlSubtipo.Items.Remove += new EventHandler(HandleRemoveUserControl);
                btnNuevaSolicitud.Enabled = true;
                btnNuevaReclamacion.Visible = true;
                btnAbrirSolicitudProveedor.Enabled = true;
                btnAbrirSolicitudInspeccionGas.Enabled = true;
				
                cmbProveedor.Enabled = false;
                if (!NavigationController.IsBackward())
                {
                    this.SaveSessionData();
                }
                var controlName = Request.Params.Get("__EVENTTARGET");
                var argument = Request.Params.Get("__EVENTARGUMENT");
                hdnNumeroEquivalencias.Value = "0";
                lblMensajeEditar.Text = "";

                if (hdnIdSolicitudHistorico.Value != "")
                {
                    //CargaDatosSolicitud(Id_SOlicitud);
                    this.Modo = ModoPantalla.VISUALIZANDO;
                }
                if (hdnCodVisita.Value != "")
                {
                    //CargaDatosSolicitud(Id_SOlicitud);
                    this.Modo = ModoPantalla.VISUALIZANDO;
                }
                if (!IsPostBack)
                {
                    // Si es la primera vez que entramos en la pantalla tenemos que ver
                    // si venimos de otra pantalla o hemos accedido desde el menú
                    // Si hemos venido de otra pantalla tendremos en la URL la variable Origen
                    String strOrigen = (String)Request.QueryString["Origen"];
                    CargaComboProveedores();
                    // Establecemos el modo de la pantalla en función del punto de entrada-
                    EstablecerModoPantallaSegunOrigen(strOrigen);

                    // Cargamos los datos del usuario en las variables de ventana.
                    CargarDatosUsuario();

                    //btnPestanyaVisitas.Enabled = false;
                    InicializarControles();

                    CargarComboProvincias();

                    // Permitir al telefono filtrar por gas confort y sus estados.
                    if (this.CodPerfil == 3)
                    {
                        CargaComboSubtipoSolicitud("T08");
                    }
                    else
                    {
                        CargaComboSubtipoSolicitud("001");
                    }

                    // R#35436
                    btnAltaGC.Visible = false;
                    
                    CargaComboEstadoSolicitud("001", ddl_Subtipo.SelectedValue);
                    CargaComboEstadoSolicitudNueva("001", ddlSubtipo.SelectedValue);

                    // Kintell 22/11/2011 Tema HORARIO CONTACTO.
                    SolicitudesDB db = new SolicitudesDB();
                    string contra = lblCodContratoInfo.Text;
                    DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT top 1 HORARIO_CONTACTO FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" + contra + "'");
                    if (datos.Tables[0].Rows.Count > 0)
                    {
                        cmbHorarioContacto.SelectedValue = datos.Tables[0].Rows[0].ItemArray[0].ToString();
                    }

                    CargaComboTiposAveria();
                    CargaComboTiposVisita();

                    // Show or Hide the visitas link
                    if (Usuarios.EsProveedor(this.CodPerfil))
                    {
                        VisitasProveedor.Visible = true;
                    }
                    else
                    {
                        VisitasProveedor.Visible = false;
                    }

                    //17/05/2013
                    VisitasProveedor.Visible = false;
                }

                //Motramos u ocultamos los paneles en función del modo.
                MostrarOcultarPaneles();
                MostrarOcultarControlesPerfil();
                if (hdnSeleccionadoAlta.Value == "1")
                {
                    lblProceso.Text = Resources.TextosJavaScript.ALTA_SOLICITUD;// "ALTA SOLICITUD";
                    if (controlName == "ctl00$ContentPlaceHolderContenido$ddlSubtipo")
                    {
                        NuevoSubtipoSeleccionado();
                    }

                }
                // TODO: revisar
                if (hdnBuscarPulsado.Value == "0")
                {
                    if (hdnIdSolicitud.Value != "" && hdnIdSolicitudHistorico.Value == "" && hdnCodVisita.Value == "")
                    {
                        if (controlName == "gv_Solicitudes")
                        {
                            SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                            string subtipo = objSolicitudesDB.ObtenerSubtipoPorIdSolicitud(int.Parse(hdnIdSolicitud.Value));

                            MostrarAvisoGeneradaTrasLlamadaCortesia(hdnIdSolicitud.Value);

                            if (subtipo == "009")
                            {
                                // RECLAMACION.
                                DatosCliente(hdnIdSolicitud.Value);
                                CargaDatosSolicitud(hdnIdSolicitud.Value);
                                string Script = "<Script>NuevaReclamacion('" + hdnIdSolicitud.Value + "','" + hdnEstadoSol.Value + "','" + txtProveedor.Text + "');</Script>";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANARECLAMACION", Script, false);
                                //Buscar();
                                CargaHistorico(hdnIdSolicitud.Value.ToString());
                                btnNuevaSolicitud.Enabled = false;
                            }
                            else
                            {
                                cmbProveedor.Enabled = true;
                                
                                lblProceso.Text = Resources.TextosJavaScript.MODIF_SOLICITUD; //"MODIF. SOLICITUD";

                                this.Modo = ModoPantalla.VISUALIZANDO;
                                if (lblIdSolicitudInfo.Text != hdnIdSolicitud.Value) { DatosCliente(hdnIdSolicitud.Value); }
                                CargaHistorico(hdnIdSolicitud.Value);


                                InicializarControlesVisita();
                                gv_historicoCaracteristicas.DataSource = null;
                                gv_historicoCaracteristicas.DataBind();

                                CargarPaginador(Int32.Parse(this.hdnPageIndex.Value));

                                if (Usuarios.EsTelefono(this.CodPerfil))
                                {
                                    InicializarControlesNuevaSolicitud();
                                    CargaDatosSolicitud(hdnIdSolicitud.Value);

                                    //if (ddlSubtipo.SelectedValue == "008") { lblDescrAveria.Text = ""; ddlVisita.Visible = false; CargaComboEstadoSolicitudCalderas(); }
                                    //ddlEstado.SelectedValue = hdnEstadoSol.Value;
                                    if (ddlSubtipo.SelectedValue != "008")
                                    {
                                        string estado = ddlEstado.SelectedItem.Text;
                                        CargaComboEstadoSolicitudNueva("001", ddlSubtipo.SelectedValue);// hdnEstadoSol.Value);
                                        try
                                        {
                                            ddlEstado.SelectedValue = hdnEstadoSol.Value;
                                        }
                                        catch (Exception ex)
                                        {
                                            ListItem defaultItem = new ListItem();
                                            defaultItem.Value = hdnEstadoSol.Value;
                                            defaultItem.Text = estado;
                                            ddlEstado.Items.Insert(0, defaultItem);
                                            ddlEstado.SelectedValue = hdnEstadoSol.Value;
                                        }
                                        this.ExecuteScript("VerEditar()");
                                    }
                                }

                                MostrarAviso(hdnIdSolicitud.Value);
                                if (Usuarios.EsTelefono(this.CodPerfil))
                                {
                                    if (ddlEstado.SelectedValue == "001")
                                    {
                                        lblProveedorEditar.Visible = true;
                                        cmbProveedor.Visible = true;
                                        string proveedor = txtProveedor.Text;// Proveedores[0].ToString().Substring(0, 3);
                                        cmbProveedor.SelectedValue = proveedor.ToUpper();
                                        txtProveedor.Visible = false;
                                    }
                                }
                                try
                                {
                                    //ProveedoresDB objProveedoresDB = new ProveedoresDB();
                                    //string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo("001", ddlSubtipo.SelectedValue.ToString(), lblCodContratoInfo.Text).Split(';');

                                    string proveedor = txtProveedor.Text;// Proveedores[0].ToString().Substring(0, 3);
                                    //txtProveedor.Text = proveedor.ToUpper();
                                    cmbProveedor.SelectedValue = proveedor.ToUpper();
                                }
                                catch (Exception ex)
                                {
                                }
                                ddlSubtipo.Enabled = false;
                            }
                        }

                        //if (controlName == "pnlVisita" && argument == "Click"){ PanelVisita(); }
                        //if (controlName == "pnlSolVisita" && argument == "Click") { PanelSolVisita(); }
                        //if (controlName == "pnlSolVisitaIncorrecta" && argument == "Click") { PanelSolVisitaIncorrecta(); }
                        //if (controlName == "pnlSolReviPrecinte" && argument == "Click") { PanelSolReviPrecinte(); }
                        //if (controlName == "pnlSolAveria" && argument == "Click") { PanelSolAveria(); }
                        //if (controlName == "pnlSolAveriaIncorrecta" && argument == "Click") { PanelSolAveriaIncorrecta(); }
                        //if (controlName == "pnlReparaciones" && argument == "Click") { PanelReparaciones(); }

                        if (controlName != "ctl00$ContentPlaceHolderContenido$ddl_Subtipo")
                        {
                            SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                            string subtipo = objSolicitudesDB.ObtenerSubtipoPorIdSolicitud(int.Parse(hdnIdSolicitud.Value));
                            if (subtipo == "009")
                            {
                                // RECLAMACION.
                                string Script = "<Script>NuevaReclamacion('" + hdnIdSolicitud.Value + "','" + hdnEstadoSol.Value + "','" + lblProvAveriaInfo.Text + "');</Script>";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANARECLAMACION", Script, false);
                            }
                            else
                            {
                                if (Usuarios.EsProveedor(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil) || ddlSubtipo.SelectedValue == "008")
                                {
                                    //String Script = "";
                                    //if (lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") > 0)
                                    //{
                                    //    Script = "<SCRIPT>AbrirVentana_SolAveGasConfort('" + lblCodContratoInfo.Text + "');</SCRIPT>";
                                    //}
                                    //else
                                    //{
                                    //    Script = "<Script>AbrirVentana();</Script>";//CerrarVentanaModal()
                                    //}
                                    
                                    String Script = "<Script>AbrirVentana('" + hdnIdSolicitud.Value + "','" + lblCodContratoInfo.Text + "');</Script>";//CerrarVentanaModal()
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANAMODAL", Script, false);
                                    
                                }
                            }
                        }
                    }
                    else
                    {
                        //string proveedor = "";// Proveedores[0].ToString().Substring(0, 3);
                        ////txtProveedor.Text = proveedor.ToUpper();
                        //cmbProveedor.SelectedValue = "";
                        //if ((lblProceso.Text == ObtenerValorPropiedadTextoJavascript("ALTA_SOLICITUD && !IsPostBack) { InicializarControlesNuevaSolicitud(); }
                        if ((lblProceso.Text == "ALTA SOLICITUD" || lblProceso.Text == "PEDIDO DE ADESÃO") && !IsPostBack) { InicializarControlesNuevaSolicitud(); }
                        //if (lblProceso.Text == ObtenerValorPropiedadTextoJavascript("ALTA_SOLICITUD && lblCodContratoInfo.Text != "")
                        if ((lblProceso.Text == "ALTA SOLICITUD" || lblProceso.Text == "PEDIDO DE ADESÃO") && lblCodContratoInfo.Text != "")
                        {
                            //Y la caldera debe de estar seleccionada.
                            CalderasDB objCalderasDB = new CalderasDB();

                            DataSet ds = new DataSet();
                            ds = objCalderasDB.GetCalderasPorContrato(lblCodContratoInfo.Text.ToString());
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0].ItemArray[3] != null)
                                {
                                    //txtMarcaCaldera.Enabled = true;
                                    txtMarcaCaldera.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                                    //txtMarcaCaldera.Enabled = false;
                                }
                                txtModelocaldera.Text = ds.Tables[0].Rows[0][4].ToString();
                                codMarcaCaldera = decimal.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                            }
                            else
                            {
                                codMarcaCaldera = 0;
                            }
                        }
                    }

                    if (lblProceso.Text == "") { lblProceso.Text = Resources.TextosJavaScript.ALTA_SOLICITUD; }
                    //if (lblProceso.Text == ObtenerValorPropiedadTextoJavascript("ALTA_SOLICITUD && lblCodContratoInfo.Text != "")
                    if ((lblProceso.Text == "ALTA SOLICITUD" || lblProceso.Text == "PEDIDO DE ADESÃO") && lblCodContratoInfo.Text != "")
                    {
                        //Y la caldera debe de estar seleccionada.
                        CalderasDB objCalderasDB = new CalderasDB();

                        DataSet ds = new DataSet();
                        ds = objCalderasDB.GetCalderasPorContrato(lblCodContratoInfo.Text.ToString());
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray[3] != null)
                            {
                                //txtMarcaCaldera.Enabled = true;
                                txtMarcaCaldera.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                                //txtMarcaCaldera.Enabled = false;
                            }
                            txtModelocaldera.Text = ds.Tables[0].Rows[0][4].ToString();
                            codMarcaCaldera = decimal.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                        }
                    }


                    if (hdnIdSolicitudHistorico.Value != "")
                    {
                        //CargaDatosSolicitud(Id_SOlicitud);
                        this.Modo = ModoPantalla.VISUALIZANDO;
                        CargaHistoricoCaracteristicas(hdnIdSolicitudHistorico.Value);

                        CargarPaginador(Int32.Parse(this.hdnPageIndex.Value));
                    }
                    if (hdnCodVisita.Value != "")
                    {
                        //CargaDatosSolicitud(Id_SOlicitud);
                        this.Modo = ModoPantalla.VISUALIZANDO;
                        CargarDatosVisitas(hdnCodVisita.Value, lblCodContratoInfo.Text);

                        CargarPaginador(Int32.Parse(this.hdnPageIndex.Value));
                    }
                }
                hdnBuscarPulsado.Value = "0";
                divUltimoMovimiento.Visible = true;
                btnBajaServicio.Visible = false;

                Mantenimiento mantenimiento = new Mantenimiento();
                MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                mantenimientoDTO = mantenimiento.DatosMantenimiento(lblCodContratoInfo.Text, usuarioDTO.Pais);
                if (!String.IsNullOrEmpty(mantenimientoDTO.BCS))
                {
                    if (mantenimientoDTO.BCS.ToString().ToUpper() == "C" && (mantenimientoDTO.FEC_BAJA_SERVICIO == null || lblFecBajaServInfo.Text == "01/01/1900"))
                    {
                        if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                        {
                            btnBajaServicio.Visible = false;//true;
                            // (10/10/2014) Lo quitamos a petición de Rubén vía mail.
                            // #2042 Botón baja desde Opera.
                            btnBajaServicio.Visible = false;
                        }
                    }
                }
                if (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                {
                    if (Usuarios.EsTelefono(this.CodPerfil)) { divUltimoMovimiento.Visible = false; }
                }

                if (Usuarios.EsTelefono(this.CodPerfil))
                {
                    QuitarEstadosNoDisponiblesParaTelefono(ddlSubtipo.SelectedValue);


                    try
                    {
                        if (hdnDesEstadoSol.Value != "")
                        {
                            if (ddlEstado.Items.FindByText(hdnDesEstadoSol.Value) == null)
                            {
                                ListItem defaultItem = new ListItem();
                                defaultItem.Value = hdnEstadoSol.Value;
                                defaultItem.Text = hdnDesEstadoSol.Value;
                                ddlEstado.Items.Insert(0, defaultItem);
                                //Correo 04/12/2017 CAMBIO DE SOLICITUD A PRESUPUESTO ACEPTADO
                                //no se mantiene el estado seleccionado, se recarga el que estaba
                                if (!IsPostBack)
                                {
                                ddlEstado.SelectedValue = hdnEstadoSol.Value;
                            }
                        }
                    }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por Reclamación"));
              
                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION, -1);
                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por reclamación"));
                //RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_INFORMACION_POR_RECLAMACION, -1);
                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Gas Confort"));
                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_GAS_CONFORT, -1);

                if (NavigationController.IsBackward())
                {
                    // COGEMOS EL CONTRATO DE DETALLE VISITA Y LO BUSCAMOS.
                    // DE DETALLE VISITA PORKE ES LA UNICA WEB DESDE LA QUE PODEMOS LLEGAR CON UN BOTÓN DE VOLVER A DIA DE 11/06/2014.
                    string[] datos = Request.UrlReferrer.Query.ToString().Split('&');
                    string[] contrato = datos[0].ToString().Split('=');
                    //this.txt_contrato.Text = contrato[1];
                    //Buscar();
                    if (datos.Length > 2)
                    {
                        this.txt_contrato.Text = contrato[1];
                        Buscar();
                    }
                }

                // (10/10/2014) Lo quitamos a petición de Rubén vía mail.
                btnBajaServicio.Visible = false;

                // 03/12/2015 Exportación Beretta.
                if ((Usuarios.EsAdministrador(this.CodPerfil) || Usuarios.EsProveedor(this.CodPerfil)) && this.CodPerfil.ToString() != "17")
                {
                    btnExportarExcelBeretta.Visible = true;
                }
                else
                {
                    if (Usuarios.EsTelefono(this.CodPerfil))
                    {
                        btnExportarExcelBeretta.Visible = false;
                    }
                }
                ////if (!IsPostBack)
                ////{
                  //  RecControles(Page);
                ////}

                // TEMA PROVEEDOR ALTA SOLICITUDES DE AVERIA Y TERMOSTATO INTELIGENTE.
                btnAbrirSolicitudProveedor.Visible = false;
                if (this.CodPerfil != 20 && this.CodPerfil != 3 && (lblCodContratoInfo.Text != "" || txt_contrato.Text != ""))
                {
                    btnAbrirSolicitudProveedor.Visible = true;
                    
                }
               
                btnAbrirSolicitudInspeccionGas.Visible = false;
                //20180507 BGN Boton alta inspeccion, lo verán el telefono y el administrador
                if (this.CodPerfil == 4 || this.CodPerfil == 15 || this.CodPerfil == 18)
                {
                    btnAbrirSolicitudInspeccionGas.Visible = true;
                }

                string contratoDeDelta = Request.QueryString["CONTRATO"];
                if (contratoDeDelta != null)
                {
                    if (txt_contrato.Text == "")
                    {
                        //20200521 MAQ MOD BEG R#18312: Limpiar filtro cuando el acceso es desde Delta
                        var busquedaDesdeDelta = CurrentSession.GetAttribute(CurrentSession.SESSION_BUSQUEDA_DESDE_DELTA);
                        if (busquedaDesdeDelta != "NO")
                        {
                            txt_contrato.Text = contratoDeDelta;
                            Buscar();
                            
                            CurrentSession.SetAttribute(CurrentSession.SESSION_BUSQUEDA_DESDE_DELTA, "NO");
                        }
                        //20200521 MAQ MOD END R#18312: Limpiar filtro cuando el acceso es desde Delta
                    }
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }


         
        }

        private void QuitarEstadosNoDisponiblesParaTelefono(string cod_subtipo)
        {
            if (ddlEstado.Enabled)
            {
                for (int i = 0; i < ddlEstado.Items.Count; i++)
                {
                    //if (lblProceso.Text == ObtenerValorPropiedadTextoJavascript("ALTA_SOLICITUD)
                    if ((lblProceso.Text == "ALTA SOLICITUD" || lblProceso.Text == "PEDIDO DE ADESÃO"))
                    {
                        //if (ddlEstado.Items[i].Text.IndexOf("Visitado") < 0 && ddlEstado.Items[i].Text.IndexOf("aplaz. o ausencia") < 0 && ddlEstado.Items[i].Text.IndexOf(" SAT") < 0 && ddlEstado.Items[i].Text.IndexOf("ontactar") < 0 && ddlEstado.Items[i].Text.IndexOf("Ninguno") < 0)
                        if (ddlEstado.Items[i].Text.IndexOf(" SAT") < 0 && ddlEstado.Items[i].Text.IndexOf("ontactar") < 0 && ddlEstado.Items[i].Text.IndexOf("Ninguno") < 0 && ddlEstado.Items[i].Text.IndexOf("ACEPTADO") < 0
                            && cod_subtipo != "016"
                            )
                        {
                            // TERMOSTATO INTELIGENTE 13/08/2015
                            if ((cod_subtipo != "011") && (cod_subtipo != "008"))
                            {
                                RemoveElement(ddlEstado, "", i);
                                i = i - 1;
                            }
                            else
                            {
                                if (ddlEstado.Items[i].Text.ToUpper().IndexOf(" OFERTA") < 0 && ddlEstado.Items[i].Text.ToUpper().IndexOf("CANCELADA") < 0)
                                {
                                    RemoveElement(ddlEstado, "", i);
                                    i = i - 1;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (ddlEstado.Items[i].Text.IndexOf(" SAT") < 0 && ddlEstado.Items[i].Text.IndexOf("Ninguno") < 0 //&& ddlEstado.Items[i].Text.IndexOf("ACEPTADO") < 0
                            && ddlEstado.Items[i].Text.IndexOf("Rechazo presupuesto temporal") < 0)
                        {
                            // TERMOSTATO INTELIGENTE 13/08/2015
                            if (cod_subtipo != "011")
                            {
                                //Para el perfil del teléfono debemos permitir que cuando el contrato se encuentre SOLO en  “PRESUPUESTO PTE. DE CLIENTE”  les salga la opción de  PRESUPUESTO ACEPTADO ,  
                                // Kintell 19/09/2018
                                if (this.hdnDesEstadoSol.Value != "Presupuesto pendiente de Cliente")
                                {
                                    RemoveElement(ddlEstado, "", i);
                                    i = i - 1;
                                }
                                else
                                {
                                    if (ddlEstado.Items[i].Text.IndexOf("ACEPTADO") < 0)
                                    {
                                        RemoveElement(ddlEstado, "", i);
                                        i = i - 1;
                                    }
                                }
                            }
                            else
                            {
                                // Kintell 19/09/2018
                                //Para el perfil del teléfono debemos permitir que cuando el contrato se encuentre SOLO en  “PRESUPUESTO PTE. DE CLIENTE”  les salga la opción de  PRESUPUESTO ACEPTADO ,  
                                if (ddlEstado.SelectedValue != "005")
                                {
                                    if (ddlEstado.Items[i].Text.ToUpper().IndexOf(" OFERTA") < 0 && ddlEstado.Items[i].Text.ToUpper().IndexOf("CANCELADA") < 0)
                                    {
                                        RemoveElement(ddlEstado, "", i);
                                        i = i - 1;
                                    }
                                }
                                else
                                {
                                    if (ddlEstado.Items[i].Text.ToUpper().IndexOf(" OFERTA") < 0 && ddlEstado.Items[i].Text.ToUpper().IndexOf("CANCELADA") < 0 && ddlEstado.Items[i].Text.IndexOf("ACEPTADO") < 0)
                                    {
                                        RemoveElement(ddlEstado, "", i);
                                        i = i - 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                        bMostrarAviso = strObservaciones.IndexOf("Generada tras llamada de cortesía") > -1;
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        {
            ConfiguracionDTO confActivoAltaGC = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVO_ALTA_GC_FICTICIO);
            Boolean ActivoAltaGC = false;

            if (confActivoAltaGC != null && !string.IsNullOrEmpty(confActivoAltaGC.Valor) && Boolean.Parse(confActivoAltaGC.Valor))
            {
                ActivoAltaGC = Boolean.Parse(confActivoAltaGC.Valor);
            }

            // R#35436
            if ((Usuarios.EsProveedor(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil)) && ActivoAltaGC)
            {
                btnAltaGC.Visible = true;
            }

            btnCobertura.Enabled = false;
            hdnBusquedaColores.Value = "0";
            this.ExecuteScript("VerSolicitudes();");
            // Kintell 13/07/2020 Modificado porque si es reclamación no deja filtrar por subtipo... MODIFICAR PORKE EN Usuarios.EsTelefono DEVUELVE TAMBIEN COMO TELELFONO EL PERFIL RECLAMACION.
            if (!Usuarios.EsReclamacion(this.CodPerfil))
            {
                if (Usuarios.EsTelefono(this.CodPerfil)) { ddl_Subtipo.SelectedValue = "-1"; }
            }
            lblMensajeEditar.Text = "";
            //if (ddl_Subtipo.SelectedValue == "008") { ddl_Subtipo.SelectedValue = "-1"; }
            // Borramos los datos que haya cargados de una búsqueda anterior.
            InicializarControles();
            btnNuevaSolicitud.Enabled = false;
            //btnNuevaReclamacion.Visible = false;
            if (!Usuarios.EsReclamacion(this.CodPerfil) && Usuarios.EsTelefono(this.CodPerfil) && txtDNI.Text == "" && txtCUI.Text == "" && txt_solicitud.Text == "" && txt_contrato.Text == "" && txtApellido1.Text == "" && txtApellido2.Text == "" && txtNombre.Text == "")
            {
                if (!Usuarios.EsReclamacion(this.CodPerfil))
                {
                    this.ExecuteScript("alert('" + Resources.TextosJavaScript.TEXTO_DEBE_INIDCAR_PARAMETRO_BUSQUEDA + "');");
                }
            }
            else
            {
                int NumeroClientes = 0;
                if (!Usuarios.EsReclamacion(this.CodPerfil) && (Usuarios.EsTelefono(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil)) && (txtDNI.Text != "" || txtCUI.Text != "" || txt_solicitud.Text == "" || txt_contrato.Text == "" || txtApellido1.Text != "" || txtApellido2.Text != "" || txtNombre.Text != ""))
                {
                    //Buscamos si hay mas de un resultado para estos parametros.
                    NumeroClientes = BuscarNumeroRegistrosPorDatosCliente();
                }
                if (txt_solicitud.Text != "" && Usuarios.EsTelefono(this.CodPerfil)) { NumeroClientes = 1; }
                btnNuevaReclamacion.Visible = false;
                if (NumeroClientes == 0 || NumeroClientes == 1)
                {
                    // Realizamos la búsqueda.
                    btnNuevaSolicitud.Enabled = true;
                    //20200310 BGN BEG R#22813 Bloquear el acceso al canal (puntos y teléfono) para generar reclamaciones
                    if (NumeroClientes <= 1 && Usuarios.EsReclamacion(this.CodPerfil)) { btnNuevaReclamacion.Visible = true; }
                    //20200310 BGN END R#22813 Bloquear el acceso al canal (puntos y teléfono) para generar reclamaciones
                    CargaSolicitudes(1);
                    CargarDatos();

                    if (txt_contrato.Text != "")
                    {
                        if (!Usuarios.EsTelefono(this.CodPerfil) && !Usuarios.EsAdministrador(this.CodPerfil))
                        {
                            CargarVisitas(lblCodContratoInfo.Text);
                        }
                    }
                }
                else
                {
                    if (NumeroClientes > 50 || NumeroClientes < 0)
                    {
                        MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEMASIADAS_COINCIDENCIAS);//"DEMASIADAS COINCIDENCIAS.\\n ACOTE LOS RESULTADOS INFORMANDO MAS FILTROS DE LA BUSQUEDA");
                    }
                    else
                    {
                        CargaVariosClientes();
                    }
                }
            }

            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por Reclamación"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por reclamación"));
            //RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_INFORMACION_POR_RECLAMACION, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Gas Confort"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_GAS_CONFORT, -1);

            //2020/01/08 R#13803 Añadir validación para evitar Altas de solicitud de Aceptacion Presupuesto si tiene una ya abierta
            if (!String.IsNullOrEmpty(lblCodContratoInfo.Text))
            {
                UsuarioDTO usuarioDTO = null;
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                SolicitudesDB objSolicDB = new SolicitudesDB();
                DataSet dsSolicitudes = objSolicDB.GetSolicitudesPorContrato(lblCodContratoInfo.Text, usuarioDTO.IdIdioma);
                if (dsSolicitudes != null && dsSolicitudes.Tables.Count > 0 && dsSolicitudes.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsSolicitudes.Tables[0].Rows)
                    {
                        string subTipoSol = dr.ItemArray[4].ToString();
                        string estadoSol = dr.ItemArray[8].ToString();

                        if (subTipoSol == "007" && estadoSol == "001")
                        {
                            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_ACEPTACION_DE_PRESUPUESTO, -1);
                            //break;
                        }
                        if (subTipoSol == "008" && estadoSol == "035")
                        {
                            btnAltaGC.Visible = false;
                        }
                    }
                }
            }

            if (Usuarios.EsAdministrador(this.CodPerfil))
            {
                // 29/10/2015 Comentado porke la extracción normal ya saca las de gas confort.
                // btnExportarExcelPreciosCalderas.Visible = true;
            }

            try
            {
                //20200629 BGN MOD BEG [R#24254] Permitir modificar carencia (Pack Mantenimiento Gas y Asistencia Gas Iberdrola)
                if (!String.IsNullOrEmpty(lblCodContratoInfo.Text))
                {
                    MantenimientoDB mtoDB = new MantenimientoDB();
                    Boolean carencia = mtoDB.GetCarenciaPorContrato(lblCodContratoInfo.Text);
                    if (carencia && (lblFecBajaServInfo.Text == ""))
                    {
                        btnNuevaSolicitud.Enabled = false;
                        btnNuevaReclamacion.Visible = false;
                        btnAbrirSolicitudProveedor.Enabled = false;
                        btnAbrirSolicitudInspeccionGas.Enabled = false;
                        MostrarMensaje("Contrato en periodo de carencia.");
                    }
                }
                /*
                // Kintell carencia PMG y AG, Redmine #17446 26/06/2019.
                if ((lblServicioInfo.Text.ToUpper().IndexOf("PACK") >= 0 ) && (lblFecBajaServInfo.Text == ""))
                {
                    //Pack Mantenimiento Gas
                    //Asistencia Gas Iberdrola

                    DateTime fecAltaServicio = DateTime.Parse(lblFecAltaServInfo.Text);
                    if (fecAltaServicio.AddDays(15) > DateTime.Now && fecAltaServicio > DateTime.Parse("2019/08/15 00:00:00.000"))
                    {
                        btnNuevaSolicitud.Enabled = false;
                        btnNuevaReclamacion.Visible = false;
						btnAbrirSolicitudProveedor.Enabled = false;
                        btnAbrirSolicitudInspeccionGas.Enabled = false;
                        MostrarMensaje("Contrato en periodo de carencia.");
                    }
                }
                */
                //20200629 BGN MOD END [R#24254] Permitir modificar carencia (Pack Mantenimiento Gas y Asistencia Gas Iberdrola)
            }
            catch (Exception ex)
            { 

            }
        }

        protected void OnBtnPaginacion_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                if (hdnBusquedaColores.Value == "0")
                {
                    CargaSolicitudes(intPageIndex);
                }
                else
                {
                    CargaSolicitudesPorColores(hdnBusquedaColores.Value, intPageIndex);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void gvSolicitudes_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Id_SOlicitud = gvSolicitudes.SelectedRow.Cells[1].Text.ToString();
            hdnIdSolicitud.Value = Id_SOlicitud;
            InicializarControlesDatosContrato();
            hdnSeleccionadoAlta.Value = "0";
            // Select the process to do.
            btnCobertura.Enabled = true;

            if (ddlSubtipo.SelectedValue == "009")
            {
                // RECLAMACION.
                string Script = "NuevaReclamacion('" + Id_SOlicitud + "','','" + gvSolicitudes.SelectedRow.Cells[5].Text.ToString() + "')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANARECLAMACION", Script, false);
                CargaHistorico(Id_SOlicitud);
            }
            else
            {
                if (Usuarios.EsProveedor(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil) || ddlSubtipo.SelectedValue == "008")
                {
                    String Script = "";
                    if ((ddlSubtipo.SelectedValue.Equals("001") || ddlSubtipo.SelectedValue.Equals("004")) && lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") > 0)
                    {
                        Script = "<SCRIPT>AbrirVentana_SolAveGasConfort('" + lblCodContratoInfo.Text + "');</SCRIPT>";
                    }
                    else
                    {
                        //Script = "<Script>AbrirVentana('" + hdnIdSolicitud.Value + "','" + lblCodContratoInfo.Text + "');</Script>";//CerrarVentanaModal()
                        // Kintell 10/11/2020
                        Script = "<Script>AbrirVentana();</Script>";//CerrarVentanaModal()
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANAMODAL1", Script, false);
                    //CargaHistorico(Id_SOlicitud);
                }
                else
                {
                    CargaHistorico(Id_SOlicitud);
                    lblProceso.Text = Resources.TextosJavaScript.MODIF_SOLICITUD;//"MODIF. SOLICITUD";
                    this.ExecuteScript("VerEditar()");

                    MostrarAviso(Id_SOlicitud);
                }
            }

            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por Reclamación"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por reclamación"));
            //RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_INFORMACION_POR_RECLAMACION, -1);
            ////ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Aceptacion de presupuesto"));
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Gas Confort"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_GAS_CONFORT, -1);

        }


        protected void gvVisitas_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarAviso("0");
        }

        protected void MostrarAviso(string id_solicitud)
        {
            IDataReader AvisoVisita = null;
            try
            {
                // We catch the aviso_solicitudes from the solicitudes table.
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                string Aviso = "";
                if (id_solicitud == "0")
                {
                    AvisoVisita = Mantenimiento.ObtenerAviso(this.lblCodContratoInfo.Text, Int16.Parse(hdnCodVisita.Value.ToString()));
                    while (AvisoVisita.Read())
                    {
                        String Aviso1 = (String)DataBaseUtils.GetDataReaderColumnValue(AvisoVisita, "Aviso");
                        if (!String.IsNullOrEmpty(Aviso1))
                        {
                            Aviso1 = Aviso1.Replace(";#", @"<br /><br />");//Convert.ToChar(13).ToString());
                            // If we have something in the field, we should show the "aviso".
                            if (Aviso1 != "" && Aviso1 != "0")
                            {
                                // Here we must to show the field.
                                //Aviso1 = "<br /><br />[05/11/2013 16:53:07] Cliente llama pidiendo la visita anual ya que le urge porque quiere revisar la instalación. Pregunta si s epuede hacer antes.699907149 [SAMARA GONGORA MONTADA]";
                                string alerta = "<script type='text/javascript'>MostrarDatos('" + Aviso1 + "','" + hdnCodVisita.Value.ToString() + "');</script>";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_AVISO, alerta, false);
                                //MostrarMensaje("AVISO:" + Aviso);
                            }
                            else
                            {
                                // Here we must to hide the field.
                            }
                        }
                    }
                }
                else
                {
                    Aviso = objSolicitudesDB.ObtenerAvisoSolicitud(id_solicitud);
                    if (!String.IsNullOrEmpty(Aviso))
                    {
                        Aviso = Aviso.Replace(";#", @"<br /><br />");//Convert.ToChar(13).ToString());
                        // If we have something in the field, we should show the "aviso".
                        if (Aviso != "" && Aviso != "0")
                        {
                            // Here we must to show the field.
                            string alerta = "<script type='text/javascript'>MostrarDatos('" + Aviso + "','" + id_solicitud + "');</script>";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), Resources.TextosJavaScript.TEXTO_AVISO, alerta, false);
                            //MostrarMensaje("AVISO:" + Aviso);
                        }
                        else
                        {
                            // Here we must to hide the field.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_ERROR_MOSTRAR_AVISO);//"Error al mostrar el AVISO");
            }
            finally
            {
                if (AvisoVisita != null)
                {
                    AvisoVisita.Close();
                }
            }
        }

        protected void ddl_Subtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CodPerfil == 3)//if (Usuarios.EsTelefono(this.CodPerfil))
            {
                CargaComboEstadoSolicitud("T08", ddl_Subtipo.SelectedValue);
            }
            else
            {
                CargaComboEstadoSolicitud("001", ddl_Subtipo.SelectedValue);
            }
        }

        protected void gvSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //' ASIGNA EVENTOS
                int IdSolicitud = int.Parse(e.Row.Cells[1].Text);
                e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Attributes.Add("OnClick", "CargarDatos('" + e.Row.Cells[1].Text.ToString() + "');");
                    e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                    

                    Int32 Perfil = 0;
                    if (i == 1)
                    {
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                        Perfil = objSolicitudesDB.ObtenerPerfilPorSolicitud(IdSolicitud);

                        if (Usuarios.EsTelefono(Perfil))
                        {
                            e.Row.BackColor = System.Drawing.Color.GreenYellow;
                        }
                        else if (Usuarios.EsAdministrador(Perfil))
                        {
                            e.Row.BackColor = System.Drawing.Color.Red;
                        }
                        else if (Usuarios.EsProveedor(Perfil))
                        {
                            e.Row.BackColor = System.Drawing.Color.RoyalBlue;
                        }
                    }

                    if (i == 18)
                    {
                        SolicitudesDB objSolicitudesDB = new SolicitudesDB();

                        e.Row.Cells[i].Text = objSolicitudesDB.ObtenerProveedorUltimoMovimientoSolicitud(int.Parse(e.Row.Cells[1].Text.ToString()));
                        if (e.Row.Cells[i].Text == "d")
                        {
                            e.Row.Cells[i].Text = Resources.TextosJavaScript.TEXTO_ADMINISTRACION;//"ADMINISTRACION";
                        }
                    }

                    if (i == 19)
                    {
                        if (e.Row.Cells[i].Text.IndexOf("Gas Confort") > 0)
                        {
                            e.Row.Cells[i].ForeColor = Color.Blue;
                            e.Row.Cells[i].Font.Bold = true;
                        }

                    }
                    if (i == 8)
                    {
                        if (e.Row.Cells[i].Text.IndexOf("Reclamaci&#243;n") > 0)
                        {
                            e.Row.Cells[i].ForeColor = Color.Blue;
                            e.Row.Cells[i].Font.Bold = true;
                            if (Usuarios.EsProveedor(this.CodPerfil) || Usuarios.EsAdministrador(this.CodPerfil))
                            {
                                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                                string subtipo = objSolicitudesDB.ObtenerSubtipoPorIdSolicitud(int.Parse(e.Row.Cells[1].Text));
                                if (subtipo == "009")
                                {
                                    // RECLAMACION.
                                    string Script = "<Script>NuevaReclamacion('" + e.Row.Cells[1].Text + "','" + e.Row.Cells[9].Text + "','" + e.Row.Cells[17].Text + "');</Script>";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANARECLAMACION", Script, false);
                                }
                            }
                        }
                    }
                    if (i == 10)
                    {
                        DateTime fechaCreacion = DateTime.Parse(e.Row.Cells[10].Text);
                        if (e.Row.Cells[8].Text.IndexOf("Reclamaci&#243;n") > 0)
                        {
                            if (fechaCreacion.AddDays(5) < DateTime.Now)
                            {
                                e.Row.BackColor = System.Drawing.Color.Pink;
                            }
                        }
                    }
                }
            }
        }

        protected void gv_HistoricoSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // ' FORMATEA ROWS
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //' ASIGNA EVENTOS
                e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");

                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Attributes.Add("OnClick", "CargarDatosHistorico('" + e.Row.Cells[0].Text.ToString() + "');");
                    e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                }
            }
        }

        protected void gvVisitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web
            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                Boolean prl = false;
                if (Boolean.TryParse(e.Row.Cells[8].Text, out prl))
                {
                    if (prl)
                    {
                        e.Row.Cells[8].Text = Resources.TextosJavaScript.TEXTO_SI;
                    }
                    else
                    {
                        e.Row.Cells[8].Text = Resources.TextosJavaScript.TEXTO_NO;
                    }
                }
                else
                {
                   e.Row.Cells[8].Text = Resources.TextosJavaScript.TEXTO_NO;
                }
            }
            //BGN ADD INI R#25266: Visualizar si una visita tiene PRL por Web 

            if (lblProveedorInfo.Text != "")
            {
                if (Usuarios.EsReclamacion(this.CodPerfil) || lblProveedorInfo.Text.Substring(0, 3).ToString() == this.CodProveedor || this.CodProveedor == "TEL" || this.CodProveedor == "d")
                {
                    //' FORMATEA ROWS
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //' ASIGNA EVENTOS
                        e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);");
                        e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);");
                        //'e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)

                        //'btnTiempo.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Solicitudes, "Select$" + e.Row.RowIndex.ToString)

                        for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                        {
                            //if (Usuarios.EsProveedor(this.CodPerfil) && e.Row.RowIndex.ToString() == "0")
                            //{
                            //    e.Row.Attributes.Add("OnClick", "CargarDatosVisitaProveedor('" + lblCodContratoInfo.Text + "','" + e.Row.Cells[1].Text.ToString() + "','" + e.Row.Cells[1].Text.ToString() + "');");
                            //}
                            //else
                            //{
                            e.Row.Attributes.Add("OnClick", "CargarDatosVisita('" + e.Row.Cells[1].Text.ToString() + "');");
                            //}
                            if (hdnCodVisita.Value == "") { hdnCodVisita.Value = e.Row.Cells[1].Text.ToString(); }
                            //e.Row.Cells[i].ToolTip = Tildes(DirectCast(DirectCast(DirectCast(e.Row.Cells(15), System.Web.UI.WebControls.TableCell).Controls(1), System.Web.UI.Control), System.Web.UI.WebControls.Label).ToolTip); //'e.Row.Cells(15).Text
                            e.Row.Cells[i].Attributes["style"] += "cursor:pointer;";
                            if (i == 6)
                            {
                                e.Row.Cells[i].Text = Visitas.ObtenerProveedorUltimoMovimiento(e.Row.Cells[2].Text, int.Parse(e.Row.Cells[1].Text.ToString()));
                                if (e.Row.Cells[i].Text == "d")
                                {
                                    e.Row.Cells[i].Text = Resources.TextosJavaScript.TEXTO_ADMINISTRACION;//"ADMINISTRACION";
                                }
                            }
                        }
                    }
                }
                else
                {
                    e.Row.Visible = false;
                }
            }
        }
        #endregion

        #region Cargar Combos
        protected void CargarComboProvincias()
        {
            ddl_Provincia.Items.Clear();
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();

            //ProveedoresDB objProveedoresDB = new ProveedoresDB();
            //SqlDataReader dr = objProveedoresDB.GetCodigoProveedorPorNombre(txt_Proveedor.Text);
            //dr.Read();
            //hidden_proveedor.Value = dr["proveedor"].ToString();

            //String cod_proveedor = hidden_proveedor.Value;

            ddl_Provincia.DataSource = objTipoSolicitudesDB.GetProvincias(this.CodProveedor,usuarioDTO.IdIdioma);
            ddl_Provincia.DataTextField = "NOM_PROVINCIA";
            ddl_Provincia.DataValueField = "COD_PROVINCIA";
            ddl_Provincia.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "";
            defaultItem.Text = "TODAS";//ObtenerValorPropiedadTextoJavascript("TEXTO_TODAS;// "TODAS";// '"Seleccione un tipo de solicitud"
            ddl_Provincia.Items.Insert(0, defaultItem);
        }

        protected void CargaComboSubtipoSolicitud(String cod_tipo_solicitud)
        {
            ddl_Subtipo.Items.Clear();
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();
            DataSet dtsSubtipo = objTipoSolicitudesDB.GetSubtipoSolicitudesPorTipo(cod_tipo_solicitud,usuarioDTO.IdIdioma);

            ddl_Subtipo.DataSource = dtsSubtipo;
            ddl_Subtipo.DataTextField = "descripcion";
            ddl_Subtipo.DataValueField = "codigo";
            ddl_Subtipo.DataBind();

            //'Kintell 21/06/2011
            //'revisión por precinte
            if (!Usuarios.EsTelefono(this.CodPerfil))
            {
                ListItem RevisionPrecinte = new ListItem();
                RevisionPrecinte.Value = "005";
                RevisionPrecinte.Text = Resources.TextosJavaScript.TEXTO_REVISION_POR_PRECINTE;// "Revisión por Precinte";
                ddl_Subtipo.Items.Add(RevisionPrecinte);

                ListItem VisitaSupervision = new ListItem();
                VisitaSupervision.Value = "006";
                VisitaSupervision.Text = Resources.TextosJavaScript.TEXTO_VISITA_SUPERVISION;// "Visita supervision";
                ddl_Subtipo.Items.Add(VisitaSupervision);

                //BGN ADD BEG 11/02/2020: [Redmine:#21965] Servicio Supervision
                ListItem AveriaSupervision = new ListItem();
                AveriaSupervision.Value = "018";
                AveriaSupervision.Text = Resources.TextosJavaScript.TEXTO_AVERIA_SUPERVISION;// "Avería supervision";
                ddl_Subtipo.Items.Add(AveriaSupervision);
                //BGN ADD END 11/02/2020: [Redmine:#21965] Servicio Supervision
            }
            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = "TODOS";//ObtenerValorPropiedadTextoJavascript("TEXTO_TODOS;//"TODOS";// '"Seleccione un subtipo de solicitud"
            ddl_Subtipo.Items.Insert(0, defaultItem);

        }

        protected void CargaComboSubtipoNuevaSolicitud()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            //20200224 BGN INTE SP_GET_TIPO_SOLICITUD_POR_PERFIL -> Hay que cambiar la llamada a este SP
            TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();
            DataSet dtsSubtipo = objTipoSolicitudesDB.GetSubtipoSolicitudesPorEFV(hdnCodEFV.Value, usuarioDTO.IdIdioma);
            ddlSubtipo.DataSource = dtsSubtipo;
            ddlSubtipo.DataTextField = "descripcion";
            ddlSubtipo.DataValueField = "codigo";
            ddlSubtipo.DataBind();

            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por Reclamación"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_INFORMACION_POR_RECLAMACION, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Información por reclamación"));
            //RemoveElement(ddlSubtipo, ObtenerValorPropiedadTextoJavascript("TEXTO_INFORMACION_POR_RECLAMACION, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Solicitud de avería no resuelta tras llamada de cortesía"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_SOLICITUD_NO_RESUELTA_LLAMADA_CORTESIA, -1);
            //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Gas Confort"));
            RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_GAS_CONFORT, -1);
            // 26/01/2016 Quitamos la avería, y averóa incorrecta, sobre instalación de GC, porque las averías se darán de alta a través de averías normales, pero dejamos el subtipo en la
            // BB.DD. para poder cargar las características para este subtipo.
            
            // Renovación calderas (Kintell 25/09/2019 Redmine: #15290).
            if (!Usuarios.EsAdministrador(this.CodPerfil))
            {
                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_RENOVACION_CALDERA, -1);
                RemoveElement(ddlSubtipo, "Renovación Caldera", -1);
                RemoveElement(ddlSubtipo, "Renovação Caldera", -1);
            }
            
            if (Usuarios.EsTelefono(this.CodPerfil))
            {
                if (ComprobarAveriasAbiertas())
                {
                    //for (int i = 0; i < ddlSubtipo.Items.Count; i++)
                    //{
                        //if (ddlSubtipo.Items[i].Text.IndexOf("Aceptacion de presupuesto") < 0 || ddlSubtipo.Items[i].Text.IndexOf("Aceitação de orçamento") < 0)
                        //{
                        //    RemoveElement(ddlSubtipo, "", i);
                        //}
                    //}
                    ddlSubtipo.Items.Clear();
                    ListItem itemAcepPresupuesto = new ListItem();
                    itemAcepPresupuesto.Value = "007";
                    itemAcepPresupuesto.Text = Resources.TextosJavaScript.TEXTO_ACEPTACION_DE_PRESUPUESTO;//"Aceptacion de presupuesto" - "Aceitação de orçamento"
                    ddlSubtipo.Items.Insert(0, itemAcepPresupuesto);
                                        
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ACEPTACION_PRESUPUESTO);//"ATENCIÓN!!!\\nCONTRATO DADO DE BAJA POR RECLAMACION\\nNo se permite dar de alta solicitudes."); //AveriaAbierta()
                }

                //20180321 BGN Peticion Alex, telefono solo puede abrir solicitudes de visita para Proteccion Gas
                Mantenimiento mantenimiento = new Mantenimiento();
                MantenimientoDTO mantenimientoDTO2 = mantenimiento.DatosMantenimiento(txt_contrato.Text, usuarioDTO.Pais);
                if (!String.IsNullOrEmpty(mantenimientoDTO2.CODEFV))
                {
                    SolicitudesDB dbSol = new SolicitudesDB();
                    DataSet datosEFV = dbSol.ObtenerDatosDesdeSentencia("SELECT SMGPROTECCIONGAS,SMGPORTUGALMANTENIMIENTO,SMGPORTUGALMANTENIMIENTOINDEPENDIENTE,COD_COBERTURA from TIPO_EFV where COD_EFV='" + mantenimientoDTO2.CODEFV.ToString() + "'");
                    Boolean esProteccionGas = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[0].ToString());
                    Boolean esMGI = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[1].ToString());
                    Boolean esMGIIndependiente = Boolean.Parse(datosEFV.Tables[0].Rows[0].ItemArray[2].ToString());
                    string cobertura = datosEFV.Tables[0].Rows[0].ItemArray[3].ToString();
                    Boolean esAGI=false;
                    if (cobertura.ToUpper() == "AGI")
                    {
                        esAGI = true;
                    }
                    if (esProteccionGas)
                    {
                        ddlSubtipo.Items.Clear();
                        ddlSubtipo.DataSource = mantenimiento.GetSubtipoSolicitudesProteccionGasTelefono(usuarioDTO.IdIdioma);
                        ddlSubtipo.DataTextField = "descripcion";
                        ddlSubtipo.DataValueField = "codigo";
                        ddlSubtipo.DataBind();
                    }

                    //17012019 Kintell, Si es MGI con contrato en TA, solo puede abrir solicitudes de Inspección.
                    if ((esMGI || esMGIIndependiente) && mantenimientoDTO2.ESTADO_CONTRATO == "TA")
                    {
                        ddlSubtipo.Items.Clear();
                        ddlSubtipo.DataSource = mantenimiento.GetSubtipoSolicitudesInspeccion(usuarioDTO.IdIdioma);
                        ddlSubtipo.DataTextField = "descripcion";
                        ddlSubtipo.DataValueField = "codigo";
                        ddlSubtipo.DataBind();
                    }

                    //11022019 Kintell si es AGI solo puede abrir Averias o averia incorrecta.
                    if (esAGI)
                    {
                        ddlSubtipo.Items.Clear();
                        ddlSubtipo.DataSource = mantenimiento.GetSubtipoSolicitudesAGI(usuarioDTO.IdIdioma);
                        ddlSubtipo.DataTextField = "descripcion";
                        ddlSubtipo.DataValueField = "codigo";
                        ddlSubtipo.DataBind();
                    }
                }
            }
            
            ListItem defaultItemEdit = new ListItem();
            defaultItemEdit.Value = "-1";
            defaultItemEdit.Text = Resources.TextosJavaScript.SELECCIONE_SUBTIPO_SOLICITUD;//"Selecciones subtipo de solicitud...";// '"Seleccione un subtipo de solicitud"
            ddlSubtipo.Items.Insert(0, defaultItemEdit);
            ddlSubtipo.SelectedIndex = 0;
            
        }

        protected void CargaComboSubtipoSolicitudTodas()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            ddl_Subtipo.Items.Clear();

            TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();

            ddl_Subtipo.DataSource = objTipoSolicitudesDB.GetSubtipoSolicitudes(usuarioDTO.IdIdioma);
            ddl_Subtipo.DataTextField = "descripcion";
            ddl_Subtipo.DataValueField = "codigo";
            ddl_Subtipo.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = "TODOS";//ObtenerValorPropiedadTextoJavascript("TEXTO_TODOS;//"TODOS";// '"Seleccione un subtipo de solicitud"
            ddl_Subtipo.Items.Insert(0, defaultItem);
        }

        protected void CargaComboEstadoSolicitud(String cod_tipo, String cod_subtipo)
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            ddl_Estado.Items.Clear();
            ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipo(cod_tipo, cod_subtipo,usuarioDTO.IdIdioma);
            ddl_Estado.DataTextField = "descripcion";
            ddl_Estado.DataValueField = "codigo";
            ddl_Estado.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = "TODOS";//ObtenerValorPropiedadTextoJavascript("TEXTO_TODOS;//"TODOS";// '"Seleccione un estado de solicitud"
            ddl_Estado.Items.Insert(0, defaultItem);
        }


        protected void CargaComboEstadosFuturos(String cod_tipo, String cod_subtipo, String cod_estado, String des_estado)
        {
            ddl_Estado.Enabled = true;

            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            ddl_Estado.Items.Clear();
            UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
            if (Usuarios.EsTelefono(this.CodPerfil))
            {
                ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 1, user.IdIdioma);
            }
            else
            {
                ddl_Estado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudFuturos(cod_tipo, cod_subtipo, cod_estado, 2, user.IdIdioma);
            }

            ddl_Estado.DataTextField = "descripcion";
            ddl_Estado.DataValueField = "codigo";
            ddl_Estado.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = des_estado;
            ddl_Estado.Items.Insert(0, defaultItem);
        }
        #endregion

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


        protected void ExportarInformeBeretta()
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                ConsultasDB objConsultasDB = new ConsultasDB();

                DataTable dtReportGasConfort = objConsultasDB.GetReport_WEBGasConfort(usuarioDTO.Pais,usuarioDTO.CodProveedor);
                if (dtReportGasConfort != null)
                {
                    if (dtReportGasConfort.Rows.Count > 0)
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dtReportGasConfort);
                        ExcelHelper excel = new ExcelHelper(Resources.TextosJavaScript.TEXTO_LISTADO_BERETTA, HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                        ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
                        excel.TableName = "GasConfort";
                        excel.Attributtes.Add(headerAttrib);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_EXISTE_SOLICITUDES_EXPORTAR);//"No existen solicitudes a exportar");
            }
            finally
            {
                String Script = "<script language='javascript'>ExportarExcel();</script>";//OcultarCapaEspera();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPACALDERAS", Script, false);
            }
        }


        protected void ExportarExcel()
        {
            try
            {
                string cod_proveedor = "";
                if (Usuarios.EsTelefono(this.CodPerfil))
                {
                    cod_proveedor = "TEL";
                }
                else
                {
                    if (!Usuarios.EsAdministrador(this.CodPerfil))
                    {
                        cod_proveedor = this.CodProveedor;
                    }
                }

                if (Usuarios.EsReclamacion(this.CodPerfil))
                {
                    cod_proveedor = "REC";
                }

                string cod_contrato = txt_contrato.Text;
                bool consultaPendietes = chk_Pendientes.Checked;
                string cod_tipo = "001";
                string cod_subtipo = ddl_Subtipo.SelectedValue;
                string cod_estado = ddl_Estado.SelectedValue;
                //Dim caldera As String = ddl_MarcaCaldera.Text


                String fechaDesde = txtFechaDesde.Text;
                String fechaHasta = txtFechaHasta.Text;
                if (fechaDesde != "") { if (fechaDesde.Length == 9) { fechaDesde = "0" + fechaDesde; } fechaDesde = fechaDesde.Substring(6, 4) + "-" + fechaDesde.Substring(3, 2) + "-" + fechaDesde.Substring(0, 2); }
                if (fechaHasta != "") { if (fechaHasta.Length == 9) { fechaHasta = "0" + fechaHasta; } fechaHasta = fechaHasta.Substring(6, 4) + "-" + fechaHasta.Substring(3, 2) + "-" + fechaHasta.Substring(0, 2); }

                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                string Urgente = "False";
                if (this.chk_Urgente.Checked) { Urgente = "True"; }

                String IdPerfilColores = "0";
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                CultureInfo culture;
                //culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);//"FR-fr");
                culture = new CultureInfo(CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_CULTURA).ToString());

                //DataSet dsSolicitudes = objSolicitudesDB.GetSolicitudesPorProveedorExcell(cod_proveedor, cod_contrato, consultaPendietes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, IdPerfilColores, txt_solicitud.Text, usuarioDTO.IdIdioma.ToString(), culture.ToString()); //dsSolicitudes
                SolicitudDB objSol = new SolicitudDB();
                DataTable dtSolicitudes = objSol.ObtenerSolicitudesExcel(cod_proveedor, cod_contrato, consultaPendietes.ToString(), cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue.ToString(), Urgente, fechaDesde.ToString(), fechaHasta.ToString(), this.CodPerfil.ToString(), txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, "0", usuarioDTO.IdIdioma.ToString(), usuarioDTO.Pais.ToString(),txtDNI.Text,txtCUI.Text);//culture.ToString());
                
                if (dtSolicitudes!=null)//dsSolicitudes != null)
                {
                    if (dtSolicitudes.Rows.Count >0)//dsSolicitudes.Tables.Count > 0)
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL,dtSolicitudes);// dsSolicitudes.Tables[0]);
                        ExcelHelper excel = new ExcelHelper(Resources.TextosJavaScript.TEXTO_LISTADO_SOLICITUDES, HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                        ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
                        excel.TableName = Resources.TextosJavaScript.TEXTO_SOLICITUDES;//"solicitudes";
                        excel.Attributtes.Add(headerAttrib);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_EXISTE_SOLICITUDES_EXPORTAR);//"No existen solicitudes a exportar");
            }
            finally
            {
                String Script = "<script language='javascript'>ExportarExcel();</script>";//OcultarCapaEspera();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPACALDERAS", Script, false);
            }
        }
        protected void btnBuscarCalderas_Click(object sender, EventArgs e)
        {
            // Seleccionamos el subtipo de calderas.
            ddl_Subtipo.SelectedValue = "008";
            // Borramos los datos que haya cargados de una búsqueda anterior.
            InicializarControles();
            btnNuevaSolicitud.Enabled = false;
            ////btnNuevaReclamacion.Visible = false;
            ////if (Usuarios.EsTelefono(this.CodPerfil) && txt_contrato.Text == "" && txtApellido1.Text == "" && txtApellido2.Text == "" && txtNombre.Text == "")
            ////{
            ////    this.ExecuteScript("alert('Debe de indicar algún parametro de busqueda');");
            ////}
            ////else
            ////{
            //    int NumeroClientes = 0;
            //    if (Usuarios.EsTelefono(this.CodPerfil) && (txt_contrato.Text == "" || txtApellido1.Text != "" || txtApellido2.Text != "" || txtNombre.Text != ""))
            //    {
            //        if (txt_contrato.Text == "")
            //        {
            //            //Buscamos si hay mas de un resultado para estos parametros.
            //            NumeroClientes = BuscarNumeroRegistrosCalderasPorDatosCliente();
            //        }
            //    }
            //    if (NumeroClientes == 0 || NumeroClientes == 1)
            //    {
            //        //// Realizamos la búsqueda.
            //        CargaSolicitudes(1);
            //        CargarDatos();
            //        btnNuevaSolicitud.Enabled = true;
            //        if (Usuarios.EsReclamacion(this.CodPerfil)) { btnNuevaReclamacion.Visible = true; }
            //        CargarVisitas(lblCodContratoInfo.Text);
            //    }
            //    else
            //    {
            //        if (NumeroClientes > 50 || NumeroClientes < 0)
            //        {
            //            MostrarMensaje ("DEMASIADAS COINCIDENCIAS.\\n ACOTE LOS RESULTADOS INFORMANDO MAS FILTROS DE LA BUSQUEDA");
            //        }
            //        else
            //        {
            //            CargaVariosClientes();
            //        }
            //    }
            ////}


            //Mostramos todo para todos los perfiles, ya no tenemos en cuenta la limitación que existia antes 
            //si venía de Telefono.
            //// Realizamos la búsqueda.
            CargaSolicitudes(1);
            //No nos hace falta cargar los datos del contrato -- CargarDatos();
            btnNuevaSolicitud.Enabled = true;
            if (Usuarios.EsReclamacion(this.CodPerfil))
            {
                btnNuevaReclamacion.Visible = true;
            }
            CargarVisitas(lblCodContratoInfo.Text);
        }

        protected int BuscarNumeroRegistrosPorDatosCliente()
        {
            try
            {
                String CodContrato = txt_contrato.Text + "%";
                String Apellido1 = txtApellido1.Text + "%";
                String Apellido2 = txtApellido2.Text + "%";
                String Nombre = txtNombre.Text + "%";
                String Subtipo = ddl_Subtipo.SelectedValue;
                String DNI = txtDNI.Text + "%";
                String CUI = txtCUI.Text + "%";
                if (Subtipo == "-1") { Subtipo = "%"; }
                
                return Usuarios.ObtenerCantidadUsuariosPorNombre(Nombre, Apellido1, Apellido2, CodContrato, Subtipo,DNI,CUI);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        protected int BuscarNumeroRegistrosCalderasPorDatosCliente()
        {
            try
            {
                String CodContrato = txt_contrato.Text + "%";
                String Apellido1 = txtApellido1.Text + "%";
                String Apellido2 = txtApellido2.Text + "%";
                String Nombre = txtNombre.Text + "%";
                String Subtipo = ddl_Subtipo.SelectedValue;// +"%";
                if (Subtipo == "-1") { Subtipo = "%"; }
                Usuarios Usuarios = new Usuarios();
                return Usuarios.ObtenerCantidadUsuariosCalderaPorNombre(Nombre, Apellido1, Apellido2, CodContrato, Subtipo);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        protected void CargaVariosClientes()
        {
            IDataReader Datos = null;
            try
            {
                hdnNumeroEquivalencias.Value = "2";
                String Apellido1 = txtApellido1.Text + "%";
                String Apellido2 = txtApellido2.Text + "%";
                String Nombre = txtNombre.Text + "%";
                String Subtipo = ddl_Subtipo.SelectedValue;
                if (Subtipo == "-1") { Subtipo = "%"; }

                Usuarios Usuarios = new Usuarios();
                Datos = Usuarios.ObtenerUsuariosPorNombre(Nombre, Apellido1, Apellido2, Subtipo);

                gridContratosCalderas.DataSource = Datos;
                gridContratosCalderas.DataBind();
                //Datos.Close();

                ////MostrarMensaje ("Se han encontrado mas de una coincidencia con los criterios de búsqueda.");
                ////string Script = "<script>Prueba();</script>";

                ////Page.RegisterStartupScript("MasDeUno", Script);
            }
            finally
            {
                if (Datos != null)
                {
                    Datos.Close();
                }
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                hdnNumeroEquivalencias.Value = "1";
                //MostrarMensaje ("Se han encontrado mas de una coincidencia con los criterios de búsqueda.");
                //string Script = "<script>Prueba();</script>";

                //Page.RegisterStartupScript("MasDeUno", Script);
            }
            catch (Exception ex)
            {

            }
        }
        protected void onBtnBajaServicio_Click(object sender, EventArgs e)
        {
            try
            {
                hdnNumeroEquivalencias.Value = "1";
                //MostrarMensaje ("Se han encontrado mas de una coincidencia con los criterios de búsqueda.");
                //string Script = "<script>Prueba();</script>";

                //Page.RegisterStartupScript("MasDeUno", Script);
                //11/08/2010 Kintell.
                //**************************************
                //Botón para dar de baja en contrato de facturación.
                string Contrato = lblCodContratoInfo.Text;
                //Hay k mirar si tenemos k actualizar la fecha de baja o la fecha_hasta_facturacion.....
                //Mantenimiento.ActualizarFechaHastaFacturacion(Contrato)
                UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                string usuario = user.Login;
                Mantenimiento.ActualizarFechaBajaServicio(Contrato, usuario);

                //FACTURACION BAJAS 11/12/2012.
                Mantenimiento.InsertarFacturacionBajas(Contrato, Int32.Parse(gvVisitas.Rows[0].Cells[1].Text.ToString()));

                //BAJAS DONDE MIGUEL FERRER 13/05/2011
                string CONEXION; // = [ª0000ª]
                CONEXION = WebConfigurationManager.ConnectionStrings["OPCOMCMDMIGUELFERRER"].ConnectionString;

                SqlConnection myConnection = new SqlConnection(CONEXION);
                string Sql = "";

                try
                {
                    myConnection.Open();
                    Sql = "INSERT INTO GFI_OPCOM.ALARMAS.BAJAS_ALARMAS (COD_CONTRATO,TIPO,SUBTIPO) VALUES('" + Contrato + "','01','33')";
                    SqlCommand myCommand = new SqlCommand(Sql, myConnection);
                    myCommand.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if ((myConnection.State == ConnectionState.Open))
                    {
                        myConnection.Close();
                    }
                }
                //**********************************************
                this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_CONTRATO_DADO_DE_BAJA);//"Contrato dado de baja");
                Buscar();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                hdnNumeroEquivalencias.Value = "1";
                lblProceso.Text = "";

                //ddlSubtipo.Items.Remove(ddlSubtipo.Items.FindByText("Revisión por Precinte"));
                RemoveElement(ddlSubtipo, Resources.TextosJavaScript.TEXTO_REVISION_POR_PRECINTE, -1);

                //MostrarMensaje ("Se han encontrado mas de una coincidencia con los criterios de búsqueda.");
                //string Script = "<script>Prueba();</script>";

                //Page.RegisterStartupScript("MasDeUno", Script);
            }
            catch (Exception ex)
            {

            }
        }

        private void CargaComboTiposVisitaIncorrecta()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            ddlVisita.Items.Clear();

            TiposVisitaDB objTiposVisitaDB = new TiposVisitaDB();

            ddlVisita.DataSource = objTiposVisitaDB.GetTiposVisitaIncorrecta(usuarioDTO.IdIdioma);
            ddlVisita.DataTextField = "descripcion_averia";
            ddlVisita.DataValueField = "cod_averia";
            ddlVisita.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = Resources.TextosJavaScript.TEXTO_SELECCIONE_TIPO_VISITA;// "Seleccione un tipo de visita";
            ddlVisita.Items.Insert(0, defaultItem);
        }

        protected void ddlSubtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            NuevoSubtipoSeleccionado();
        }

        private void NuevoSubtipoSeleccionado()
        {
            //// 26/01/2016 Tema solicitudes sobre instalación de GC.
            // 11/04/2016 comentado, al dar de alta tiene k darse de alta sin caracteristicas.
            //if ((ddlSubtipo.SelectedValue.Equals("001") || ddlSubtipo.SelectedValue.Equals("004")) && lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") > 0)
            //{
            //    // 26/01/2016 Si es avería ponemos, en el hidden que miraremos en la modal, avería sobre instalación de GC.
            //    //hdnSubtipoAveriaSobreGC.Value = "012";
            //    //if (ddlSubtipo.SelectedValue.Equals("004"))
            //    //{
            //    //    // 26/01/2016 Si es avería incorrecta ponemos, en el hidden que miraremos en la modal, avería incorrecta sobre instalación de GC.
            //    //    hdnSubtipoAveriaSobreGC.Value = "013";
            //    //}
            //    hdnSubtipoAveriaSobreGC.Value = ddlSubtipo.SelectedValue;
            //    string Script = "<SCRIPT>AbrirVentana_SolAveGasConfort('" + lblCodContratoInfo.Text + "');</SCRIPT>";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "VENTANAAVERIAGC", Script, false);
            //}
            //else
            //{
            if (String.IsNullOrEmpty(lblFecBajaServInfo.Text))
            {
                ddlMotivoCancelacion.Enabled = false;
                txtEditarObservacionesAnteriores.Text = "";
                if (ddlEstado.Items.Count > 0) { ddlEstado.SelectedIndex = 0; }

                if (ddlMotivoCancelacion.Items.Count > 0) { ddlMotivoCancelacion.SelectedIndex = 0; }
                txtEditarTelefono.Text = "";
                txtPersonaContacto.Text = "";
                cmbHorarioContacto.SelectedIndex = 0;
                ddlAveria.SelectedIndex = 0;
                chkUrgente.Checked = false;
                txtEditarObservacionesAnteriores.Text = "";
                txtEditarObservaciones.Text = "";
                //txtMarcaCaldera.Text = "";
                txtModelocaldera.Text = "";
                //InicializarControlesNuevaSolicitud();
                hdnDesEstadoSol.Value = "";
                if (lblProceso.Text == "ALTA SOLICITUD" || lblProceso.Text == "PEDIDO DE ADESÃO")
                {
                    hdnIdSolicitud.Value = "";
                    CargaComboEstadoSolicitudNueva("001", ddlSubtipo.SelectedValue);
                }
                ddlSubtipo.Enabled = true;
                ddlEstado.Enabled = true;
                ddlAveria.Enabled = true;
                ddlVisita.Enabled = true;
                btnAceptarEditarSolicitud.Enabled = true;
                ChkRetencion.Checked = false;
                //*****************************************************
                if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("AVER") < 0
                    && ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("AVAR") < 0)
                {
                    if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("ACEPTACI") < 0
                        && ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("ACEITA") < 0)
                    {
                        if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("TERMOSTATO") < 0
                            && ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("TERMóSTATO") < 0)
                        {
							if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("INSPEÇÃO") < 0)
                            {
	                            ddlAveria.Visible = false;
	                            lblDescrAveria.Text = Resources.TextosJavaScript.TEXTO_VISITA;//"Visita:";
	                            //Kintell 07/06/2011
	                            ddlVisita.Visible = true;
	
	
	
	                            if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("INCORR") > 0)
	                            {
	                                CargaComboTiposVisitaIncorrecta();
	
	
	
	
	
	                            }
	                            else
	                            {
	                                CargaComboTiposVisita();
	
	
	
	                            }
							}
							else
                            {
                                ddlAveria.Visible = false;
                                lblDescrAveria.Text = "";
                                //Kintell 07/06/2011
                                ddlVisita.Visible = false;
                            }
                        }
                        else
                        {
                            ddlAveria.Visible = false;
                            lblDescrAveria.Text = "";
                            //Kintell 07/06/2011
                            ddlVisita.Visible = false;
                        }
                    }
                    else
                    {
                        ddlAveria.Visible = false;
                        lblDescrAveria.Text = "";
                        //Kintell 07/06/2011
                        ddlVisita.Visible = false;
                    }
                }
                else
                {
                    ddlAveria.Visible = true;
                    lblDescrAveria.Visible = true;
                    lblDescrAveria.Text = Resources.TextosJavaScript.TEXTO_AVERIA;//"Averia:";
                    //Kintell 07/06/2011
                    ddlVisita.Visible = false;
                }
                CargaComboEstadoSolicitud("001", ddlSubtipo.SelectedValue);
                ValidaComboMotivoCancelacion(ddlEstado.SelectedValue);
                if (ddlSubtipo.SelectedItem != null)
                {
                    if (ddlSubtipo.SelectedItem.Value == "004")
                    {
                        chkUrgente.Checked = true;
                    }
                    else
                    {
                        chkUrgente.Checked = false;
                    }
                }
                chkUrgente.Enabled = false;
                //*****************************************************
                // TERMOSTATO INTELIGENTE 13/08/2015
                if ((ddlSubtipo.SelectedItem.Value != "011") && (ddlSubtipo.SelectedItem.Value != "008") && (ddlSubtipo.SelectedItem.Value != "017") && (ddlSubtipo.SelectedItem.Value != "012") && (ddlSubtipo.SelectedItem.Value != "016") && (ddlSubtipo.SelectedItem.Value != "009"))
                {
                    ddlEstado.SelectedValue = "001";
                }
            }
                this.ExecuteScript("VerEditar()");
            //}
        }

        protected void CargaComboEstadoSolicitudCalderas()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            ddlEstado.Items.Clear();

            ddlEstado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipoCanal("001", "008", "000",usuarioDTO.IdIdioma);
            ddlEstado.DataTextField = "descripcion";
            ddlEstado.DataValueField = "codigo";
            ddlEstado.DataBind();

            if ((ddlEstado.Items.Count == 0))
            {

                ListItem defaultItem = new ListItem();
                defaultItem.Value = "-1";
                defaultItem.Text = Resources.TextosJavaScript.TEXTO_NINGUNO;// "Ninguno";
                ddlEstado.Items.Insert(0, defaultItem);

                //Else
                //    ddl_Estado.Items.RemoveAt(0)
            }
        }

        protected void CargaComboEstadoSolicitudNueva(String cod_tipo, String cod_subtipo)
        {
            //EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            //ddlEstado.Items.Clear();
            //ddlEstado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipo(cod_tipo, cod_subtipo);
            //ddlEstado.DataTextField = "descripcion";
            //ddlEstado.DataValueField = "codigo";
            //ddlEstado.DataBind();

            //ListItem defaultItem = new ListItem();
            //defaultItem.Value = "-1";
            //defaultItem.Text = "TODOS";// '"Seleccione un estado de solicitud"
            //ddlEstado.Items.Insert(0, defaultItem);
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

            EstadosSolicitudDB objEstadoSolicitudesDB = new EstadosSolicitudDB();

            ddlEstado.Items.Clear();
            ddlEstado.SelectedValue = null;
            ddlEstado.DataSource = objEstadoSolicitudesDB.GetEstadosSolicitudPorTipoSubtipoCanal(cod_tipo, cod_subtipo, "000",usuarioDTO.IdIdioma);
            ddlEstado.DataTextField = "descripcion";
            ddlEstado.DataValueField = "codigo";
            ddlEstado.DataBind();

            if (ddlSubtipo.SelectedValue != "008")
            {
                if (!(ddlEstado.Items.Count == 0) & (ddlEstado.Items.Count > 2))
                {
                    //ddl_Estado.Items.RemoveAt(0)
                    //EN PRO
                    //ddlEstado.Items.RemoveAt(2);
                }
            }

            if ((ddlEstado.Items.Count == 0))
            {

                ListItem defaultItem = new ListItem();
                defaultItem.Value = "-1";
                defaultItem.Text = Resources.TextosJavaScript.TEXTO_NINGUNO;//"Ninguno";
                ddlEstado.Items.Insert(0, defaultItem);

                //Else
                //    ddl_Estado.Items.RemoveAt(0)
            }

            if (Usuarios.EsTelefono(this.CodPerfil))
            {
                for (int i = 0; i < ddlEstado.Items.Count; i++)
                {
                    //if (ddlEstado.Items[i].Text.IndexOf("Visitado") < 0 && ddlEstado.Items[i].Text.IndexOf("aplaz. o ausencia") < 0 && ddlEstado.Items[i].Text.IndexOf(" SAT") < 0 && ddlEstado.Items[i].Text.IndexOf("ontactar") < 0 && ddlEstado.Items[i].Text.IndexOf("Ninguno") < 0)
                    if (ddlEstado.Items[i].Text.IndexOf(" SAT") < 0 && ddlEstado.Items[i].Text.IndexOf("de contactar") < 0 && ddlEstado.Items[i].Text.IndexOf("Ninguno") < 0 && ddlEstado.Items[i].Text.IndexOf("ACEPTADO") < 0
                        && ddlEstado.Items[i].Text.IndexOf("Rechazo presupuesto temporal") < 0
                        && ddlEstado.Items[i].Text.IndexOf("Inspeção pendente") < 0
                        && cod_subtipo != "016"
                        )
                    {
                        // TERMOSTATO INTELIGENTE 13/08/2015
                        if (cod_subtipo != "011" )
                        {
                            ddlEstado.Items.RemoveAt(i);
                            i = i - 1;
                        }
                        else
                        {
                            if (ddlEstado.Items[i].Text.ToUpper().IndexOf(" OFERTA") < 0 && ddlEstado.Items[i].Text.ToUpper().IndexOf("CANCELADA") < 0)
                            {
                                ddlEstado.Items.RemoveAt(i);
                                i = i - 1;
                            }
                        }
                        //break;
                    }
                }
                //if (hdnDesEstadoSol.Value != "")
                //{
                //    ListItem defaultItem = new ListItem();
                //    defaultItem.Value = hdnEstadoSol.Value;
                //    defaultItem.Text = hdnDesEstadoSol.Value;
                //    ddlEstado.Items.Insert(0, defaultItem);
                //    ddlEstado.SelectedValue = hdnEstadoSol.Value;
                //}
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string cod_estado = ddlEstado.SelectedValue;

                CargaComboMotivosCancelacion(ddlSubtipo.SelectedValue);
                ValidaComboMotivoCancelacion(cod_estado);
                if (cod_estado == "001")
                {
                    lblProveedorEditar.Visible = true;
                    cmbProveedor.Visible = true;
                    cmbProveedor.Enabled = true;
                }
                else
                {
                    lblProveedorEditar.Visible = false;
                    cmbProveedor.Visible = false;
                    cmbProveedor.Enabled = false;
                }

                this.ExecuteScript("VerEditar()");

            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_VUELVA_A_SELECCIONAR_LA_SOLICITUD);//"Vuelva a seleccionar la solicitud en la busqueda");
            }
        }

        private void ValidaComboMotivoCancelacion(string estadoSolicitud)
        {
            try
            {
                if ("012".Equals(estadoSolicitud) || "025".Equals(estadoSolicitud) || "056".Equals(estadoSolicitud))
                {
                    ddlMotivoCancelacion.Enabled = true;
                }
                else
                {
                    ddlMotivoCancelacion.Enabled = false;
                }

                ListItem li = null;
                li = ddlMotivoCancelacion.Items.FindByText(Resources.TextosJavaScript.TEXTO_SELECCIONE_MOTIVO_CANCELACION);//"Seleccione el motivo de la cancelación");
                if ((li == null))
                {
                    li = new ListItem();
                    li.Text = Resources.TextosJavaScript.TEXTO_SELECCIONE_MOTIVO_CANCELACION;// "Seleccione el motivo de la cancelación";
                    li.Value = "-1";
                    ddlMotivoCancelacion.Items.Insert(0, li);
                }

                if ((ddlMotivoCancelacion.SelectedItem == null))
                {
                    ddlMotivoCancelacion.Items[0].Selected = true;
                }

                //PROVEEDOR.
                if (estadoSolicitud == "001")
                {
                    //Estado Inicial.
                    //Habilitar asignación de proveedor manualmente.
                    lblProveedorEditar.Visible = true;
                    txtProveedor.Visible = true;
                    ProveedoresDB objProveedoresDB = new ProveedoresDB();

                    string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo("001", ddlSubtipo.SelectedValue.ToString(), lblCodContratoInfo.Text).Split(';');

                    //'hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato)

                    string proveedor = Proveedores[0].ToString().Substring(0, 3);
                    txtProveedor.Text = proveedor.ToUpper();
                    cmbProveedor.SelectedValue = proveedor.ToUpper();
                    cmbProveedor.Visible = true;
                    txtProveedor.Visible = false;
                }
                else
                {
                    cmbProveedor.Visible = false;
                    lblProveedorEditar.Visible = false;
                    txtProveedor.Visible = false;
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

            ddlMotivoCancelacion.Items.Clear();

            MotivosCancelacionDB objMotivosCancelacionDB = new MotivosCancelacionDB();
            //Kintell 12/12/2011
            ddlMotivoCancelacion.DataSource = objMotivosCancelacionDB.GetMotivosCancelacion(Subtipo,usuarioDTO.IdIdioma);
            ddlMotivoCancelacion.DataTextField = "descripcion";
            ddlMotivoCancelacion.DataValueField = "cod_mot";
            ddlMotivoCancelacion.DataBind();

            //' por defecto esta desactivado
            ddlMotivoCancelacion.Enabled = false;
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

        private void CargaDatosSolicitud(string id_solicitud)
        {
            try
            {
                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                SqlDataReader dr = null;
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = null;
                usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                dr = objSolicitudesDB.GetSingleSolicitudes(id_solicitud, usuarioDTO.IdIdioma);
                dr.Read();

                //20180219 Recargamos el combo de subtipo solicitud para que se carguen correctamente los estados
                //if (lblProceso.Text == "MODIF. SOLICITUD")
                if (lblProceso.Text == "MODIF. SOLICITUD" || lblProceso.Text == "MODIF. PEDIDO")
                {
                    TiposSolicitudDB objTipoSolicitudesDB = new TiposSolicitudDB();
                    //DataSet dtsSubtipo = objTipoSolicitudesDB.GetSubtipoSolicitudesPorTipo("001", usuarioDTO.IdIdioma);

                    DataSet dtsSubtipo = objTipoSolicitudesDB.GetSubtipoSolicitudesPorEFV(hdnCodEFV.Value, usuarioDTO.IdIdioma);
                    ddlSubtipo.DataSource = dtsSubtipo;
                    ddlSubtipo.DataTextField = "descripcion";
                    ddlSubtipo.DataValueField = "codigo";
                    ddlSubtipo.DataBind();
                }

                //Kintell. 14/04/2009
                //Ponermos si es urgente o no la solicitud.
                this.chkUrgente.Checked = Boolean.Parse(DataBaseUtils.GetDataReaderColumnValue(dr, "URGENTE").ToString());
                if (DataBaseUtils.GetDataReaderColumnValue(dr, "Observaciones") != null) { txtEditarObservacionesAnteriores.Text = DataBaseUtils.GetDataReaderColumnValue(dr, "Observaciones").ToString(); }
                try
                {
                    ddlSubtipo.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "SUBTIPO_SOLICITUD").ToString();
                }
                catch (Exception ex)
                {
                    string nombre = DataBaseUtils.GetDataReaderColumnValue(dr, "Des_subtipo_solicitud").ToString();
                    string id = DataBaseUtils.GetDataReaderColumnValue(dr, "SUBTIPO_SOLICITUD").ToString();
                    ListItem Nuevo = new ListItem();
                    Nuevo.Value = id;
                    Nuevo.Text = nombre;
                    ddlSubtipo.Items.Add(Nuevo);

                    ddlSubtipo.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "SUBTIPO_SOLICITUD").ToString();
                    ddlSubtipo.Enabled = false;
                }
                //' Estado solicitud
                //CargaComboEstadoSolicitud("001", ddlSubtipo.SelectedValue);
                string estadoSolicitud = DataBaseUtils.GetDataReaderColumnValue(dr, "ESTADO_SOLICITUD").ToString();
                hdnEstadoSol.Value = estadoSolicitud;
                string desEstadoSolicitud = DataBaseUtils.GetDataReaderColumnValue(dr, "Des_Estado_solicitud").ToString();
                hdnDesEstadoSol.Value = desEstadoSolicitud;
                ddlEstado.SelectedValue = null;
                try
                {
                    ddlEstado.Items.FindByValue(estadoSolicitud).Selected = true;
                }
                catch (Exception ex)
                {
                    // Cargamos el  estado si por defecto no es seleccionable.
                    // Load the state if has not been loaded yet.
                    string Sql1;
                    Sql1 = "Select Codigo,Descripcion from Estado_solicitud where Codigo='" + estadoSolicitud + "' and id_idioma='" + usuarioDTO.IdIdioma + "'";
                    SolicitudesDB Db = new SolicitudesDB();
                    DataSet myDataSet1 = Db.ObtenerDatosDesdeSentencia(Sql1);

                    ListItem Nuevo = new ListItem();
                    Nuevo.Value = myDataSet1.Tables[0].Rows[0][0].ToString();
                    Nuevo.Text = myDataSet1.Tables[0].Rows[0][1].ToString();
                    ddlEstado.Items.Add(Nuevo);
                    ddlEstado.Items.FindByValue(myDataSet1.Tables[0].Rows[0][0].ToString()).Selected = true;

                }
                //ddlEstado.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "ESTADO_SOLICITUD").ToString();
                ////ddlAveria.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "COD_AVERIA").ToString();

                if (DataBaseUtils.GetDataReaderColumnValue(dr, "TELEFONO_CONTACTO") != null)
                {
                    txtEditarTelefono.Text = DataBaseUtils.GetDataReaderColumnValue(dr, "TELEFONO_CONTACTO").ToString();
                }

                if (DataBaseUtils.GetDataReaderColumnValue(dr, "NUM_MOVIL") != null)
                {
                    if (!string.IsNullOrEmpty(txtEditarTelefono.Text))
                    {
                        txtEditarTelefono.Text += "/";
                    }
                    txtEditarTelefono.Text += DataBaseUtils.GetDataReaderColumnValue(dr, "NUM_MOVIL").ToString();
                }

                if (DataBaseUtils.GetDataReaderColumnValue(dr, "PERSONA_CONTACTO") != null) { txtPersonaContacto.Text = DataBaseUtils.GetDataReaderColumnValue(dr, "PERSONA_CONTACTO").ToString(); }

                ddlEstado.Enabled = true;
                String[] solicitudCerrada = { "016", "017" };
                if (ddlEstado.SelectedValue == "010" | ddlEstado.SelectedValue == "011" |
                    ddlEstado.SelectedValue == "012" | ddlEstado.SelectedValue == "014" |
                    ddlEstado.SelectedValue == "018" | ddlEstado.SelectedValue == "020" |
                    ddlEstado.SelectedValue == "021" | ddlEstado.SelectedValue == "022" |
                    ddlEstado.SelectedValue == "023" | ddlEstado.SelectedValue == "024" |
                    ddlEstado.SelectedValue == "025" | ddlEstado.SelectedValue == "026" |
                    ddlEstado.SelectedValue == "027" | ddlEstado.SelectedValue == "028" |
                    ddlEstado.SelectedValue == "029" | ddlEstado.SelectedValue == "030" |
                    ddlEstado.SelectedValue == "032" | ddlEstado.SelectedValue == "036" |
                    ddlEstado.SelectedValue == "037" | ddlEstado.SelectedValue == "043" )//|
                    //ddlEstado.SelectedValue == "044")
                {
                    // // si la solicitud esta cerrada no se puede modificar
                    btnAceptarEditarSolicitud.Enabled = false;
                    //btnAviso.Enabled = false;
                    ddlEstado.Enabled = false;
                    //Kintell 14/04/2009
                    this.btnLLAMADA.Enabled = false;
                }
                else
                {
                    btnAviso.Enabled = true;
                }

                if (DataBaseUtils.GetDataReaderColumnValue(dr, "Mot_cancel") != null)
                {
                    string cod_estado = ddlEstado.SelectedValue;

                    CargaComboMotivosCancelacion(ddlSubtipo.SelectedValue);
                    ValidaComboMotivoCancelacion(cod_estado);

                    ddlMotivoCancelacion.Enabled = true;
                    ddlMotivoCancelacion.SelectedValue = null;
                    ddlMotivoCancelacion.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "Mot_cancel").ToString();

                    ddlMotivoCancelacion.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlSubtipo.Enabled = false;
                    ddlAveria.Enabled = false;
                    ddlVisita.Enabled = false;
                    btnAceptarEditarSolicitud.Enabled = false;
                }
                else
                {
                    ddlMotivoCancelacion.Items.Clear();
                    ddlMotivoCancelacion.Enabled = false;

                    //ddlEstado.Enabled = true;
                    ddlAveria.Enabled = true;
                    //ddlSubtipo.Enabled = true;
                    if (!String.IsNullOrEmpty(lblFecBajaServInfo.Text) && lblFecBajaServInfo.Text != "01/01/1900")
                    {
                        ddlSubtipo.Enabled = false;
                    }
                    else { ddlSubtipo.Enabled = true; }
                    ddlVisita.Enabled = true;
                    //btnAceptarEditarSolicitud.Enabled = true;
                }

                ////Mostrar aviso 24/05/2012. Kintell
                //lblAviso.Text = dr("visita_aviso").ToString().Replace(";#", (char)(13));
                //if( ! lblAviso.Text == "" ){
                //    string Mostrar = "<script type='text/javascript'>Mostrar();</script>";

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AVISO", Mostrar, false);
                //    ImageButton4.Visible = true;
                //}else{
                //    ImageButton4.Visible = false;
                //}
                //*****************
                CalderasDB objCalderasDB = new CalderasDB();
                ListItem defaultItem = new ListItem();
                DataSet ds = new DataSet();
                string cod_contrato = lblCodContratoInfo.Text; //gv_Solicitudes.SelectedRow.Cells(2).Text.ToString()

                ds = objCalderasDB.GetCalderasPorContrato(cod_contrato);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows[0][3] != null)
                    {
                        txtMarcaCaldera.Text = ds.Tables[0].Rows[0][3].ToString();
                    }
                    txtModelocaldera.Text = ds.Tables[0].Rows[0][4].ToString();
                    codMarcaCaldera = decimal.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString());
                    ComprobarCalderaINMERGAS();
                }
                else
                { codMarcaCaldera = 5834; }

                // // Tipo averia
                if ((ddlAveria.SelectedItem != null))
                {
                    ddlAveria.SelectedItem.Selected = false;
                }
                try
                {
                    ddlAveria.SelectedValue = null;
                    ddlAveria.Items.FindByValue(DataBaseUtils.GetDataReaderColumnValue(dr, "cod_averia").ToString()).Selected = true;
                }
                catch (Exception ex)
                {

                }
                //Tipo visita

                if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("INCORR") > 0)
                {
                    CargaComboTiposVisitaIncorrecta();
                }
                else
                {
                    CargaComboTiposVisita();
                }
                ddlVisita.SelectedIndex = -1; //.Items(0).Selected = false
                //ddl_Visita.ClearSelection()


                if ((ddlVisita.SelectedItem != null))
                {
                    ddlVisita.SelectedItem.Selected = false;
                }
                try
                {
                    ddlVisita.SelectedValue = null;
                    ddlVisita.SelectedValue = DataBaseUtils.GetDataReaderColumnValue(dr, "cod_averia").ToString(); //.Selected = true;
                }
                catch (Exception ex)
                {

                }

                //Kintell 08/06/2011
                //Seleccionamos tipo averia o tipo visita.
                if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("AVER") < 0 && ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("CONFORT") < 0)
                {
                    if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("ACEPTACI") < 0)
                    {
                        ddlAveria.Visible = false;
                        lblDescrAveria.Text = Resources.TextosJavaScript.TEXTO_VISITA;// "Visita:";
                        //Kintell 07/06/2011
                        ddlVisita.Visible = true;

                        if (ddlSubtipo.SelectedItem.Text.ToUpper().ToString().IndexOf("INCORR") > 0)
                        {
                            CargaComboTiposVisitaIncorrecta();
                        }
                        else
                        {
                            CargaComboTiposVisita();
                        }
                    }
                    else
                    {
                        ddlAveria.Visible = false;
                        lblDescrAveria.Text = "";
                        //Kintell 07/06/2011
                        ddlVisita.Visible = false;
                    }
                }
                else
                {
                    ddlAveria.Visible = true;
                    lblDescrAveria.Visible = true;
                    lblDescrAveria.Text = Resources.TextosJavaScript.TEXTO_AVERIA;//"Averia:";
                    //Kintell 07/06/2011
                    ddlVisita.Visible = false;
                }

                //Kintell 22/11/2011 Tema HORARIO CONTACTO.

                SolicitudesDB db = new SolicitudesDB();

                DataSet datos = db.ObtenerDatosDesdeSentencia("SELECT top 1 HORARIO_CONTACTO FROM MANTENIMIENTO WHERE COD_CONTRATO_SIC='" + lblCodContratoInfo.Text + "'");
                if (datos.Tables[0].Rows.Count > 0)
                {
                    cmbHorarioContacto.SelectedValue = datos.Tables[0].Rows[0][0].ToString();
                }

                //Kintell 14/04/2009
                //SqlConnection myConnection = new SqlConnection(ConfigurationManager.AppSettings("connectionString"));
                //string Sql;
                //Sql = "Select id_solicitud, FechaLlamada from LlamadasClientes where id_solicitud=" + id_solicitud + " order by FechaLlamada desc";
                //DataSet myDataSet = new DataSet();
                //SqlDataAdapter myCommand = new SqlDataAdapter(Sql, myConnection);

                //myCommand.Fill(myDataSet);
                //gv_LlamadasClientes.DataSource = myDataSet;
                //gv_LlamadasClientes.DataBind();

                //lblLlamadas.Text = "Histórico de Llamadas: " + myDataSet.Tables[0].Rows.Count;
                //Seleccionamos el proveedor.
                //Kintell 31/05/2011
                txtProveedor.Text = DataBaseUtils.GetDataReaderColumnValue(dr, "proveedor").ToString();

                // Comprobamos si tiene otra solcitud abierta.
                if (Usuarios.EsTelefono(this.CodPerfil)) { if (ComprobarAveriasAbiertas()) { this.ExecuteScript("alert('" + Resources.TextosJavaScript.TEXTO_ATENCION_AVERIA_ABIERTA + "');"); } }
          
            }
            catch (Exception ex)
            {

            }
        }

        private void CargaComboTiposVisita()
        {
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            ddlVisita.Items.Clear();

            TiposVisitaDB objTiposVisitaDB = new TiposVisitaDB();

            ddlVisita.DataSource = objTiposVisitaDB.GetTiposVisita(usuarioDTO.IdIdioma);
            ddlVisita.DataTextField = "descripcion_averia";
            ddlVisita.DataValueField = "cod_averia";
            ddlVisita.DataBind();

            ListItem defaultItem = new ListItem();
            defaultItem.Value = "-1";
            defaultItem.Text = Resources.TextosJavaScript.TEXTO_SELECCIONE_TIPO_VISITA;// "Seleccione un tipo de visita";
            ddlVisita.Items.Insert(0, defaultItem);
        }

        private void ComprobarCalderaINMERGAS()
        {
            try
            {
                int i = 0;
                if (txtMarcaCaldera.Text == "INMEGAS")
                {
                    if (ddl_Estado.Items.Count > 0)
                    {
                        for (i = 0; i < ddl_Estado.Items.Count; i++)
                        {
                            if (ddlEstado.Items[i].Text.IndexOf(" SAT") > 0)
                            {
                                //ddlEstado.Items.RemoveAt(i);
                                RemoveElement(ddlEstado, "", i);
                                i = i - 1;
                                if (ddl_Estado.Items.Count <= 0) break;
                                if (i < 0) i = 0;

                            }
                        }
                    }
                    MostrarMensaje(Resources.TextosJavaScript.TEXTO_ATENCION_SOLICITUD_NO_PUEDE_DERIVARSE);//"ATENCIÓN!!!\nESTA SOLICITUD NO PUEDE DERIVARSE AL SAT AL SER CALDERA INMERGAS.");
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlAveria_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkUrgente.Enabled = true;
            if (ddlSubtipo.SelectedItem.Value == "004")
            {
                chkUrgente.Checked = true;
            }
            else
            {
                if (ddlSubtipo.SelectedItem.Value == "001")
                {
                    chkUrgente.Checked = false;
                    if (ddlAveria.SelectedItem.Value == "16 " | ddlAveria.SelectedItem.Value == "1  " | ddlAveria.SelectedItem.Value == "2  " | ddlAveria.SelectedItem.Value == "3  " | ddlAveria.SelectedItem.Value == "4  ")
                    {
                        chkUrgente.Checked = true;
                    }
                }
                else
                {
                    chkUrgente.Checked = false;
                }
            }
            chkUrgente.Enabled = false;
            //----------------------------------------------------------
            this.ExecuteScript("VerEditar()");
        }
        protected void ddlVisita_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkUrgente.Enabled = true;
            if (ddlSubtipo.SelectedItem.Value == "004")
            {
                chkUrgente.Checked = true;
            }
            else
            {
                if (ddlSubtipo.SelectedItem.Value == "001")
                {
                    chkUrgente.Checked = false;
                    if (ddlVisita.SelectedItem.Value == "20 " | ddlVisita.SelectedItem.Value == "18 " | ddlVisita.SelectedItem.Value == "22 " | ddlVisita.SelectedItem.Value == "23 " | ddlVisita.SelectedItem.Value == "19 " | ddlVisita.SelectedItem.Value == "21 ")
                    {
                        chkUrgente.Checked = true;
                    }
                }
                else { chkUrgente.Checked = true; }
            }
            chkUrgente.Enabled = false;
            //----------------------------------------------------------
            this.ExecuteScript("VerEditar()");
        }

        private bool ComprobarAveriasAbiertas()
        {
            try
            {
                int i = 0;
                bool Abierta = false;

                for (i = 0; i < this.gvSolicitudes.Rows.Count; i++)
                {
                     if (this.gvSolicitudes.Rows[i].Cells[8].Text.ToString().ToUpper().IndexOf("Aver".ToUpper(), 0) >= 0 &&(this.gvSolicitudes.Rows[i].Cells[9].Text.ToString().ToUpper().IndexOf("Pendiente".ToUpper(), 0) >= 0 || this.gvSolicitudes.Rows[i].Cells[9].Text.ToString().ToUpper().IndexOf("Pendente".ToUpper(), 0) >= 0))
                    {
                        Abierta = true;
                        break;
                    }
                }
                return Abierta;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void btnAceptarEditarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                //if (lblProceso.Text != ObtenerValorPropiedadTextoJavascript("MODIF_SOLICITUD || hdnSeleccionadoAlta.Value == "1")
                if ((lblProceso.Text != "MODIF. SOLICITUD" && lblProceso.Text != "MODIF. PEDIDO") || hdnSeleccionadoAlta.Value == "1")
                {
                    // Nueva.
                    // New.
                    lblProceso.Text = Resources.TextosJavaScript.ALTA_SOLICITUD;//"ALTA SOLICITUD";
                    AltaSolicitud();
                }
                else
                {
                    // Modificar.
                    // Modify.
                    Modificarsolicitud();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Modificarsolicitud()
        {
            try
            {
                lblMensajeEditar.Text = "";

                if (ddlEstado.SelectedValue != "-1")
                {
                    //Comprobamos k si es cancelada haya seleccionado un motivo de cancelación.
                    if (this.ddlEstado.SelectedItem.Text == Resources.TextosJavaScript.TEXTO_CANCELADA)//"Cancelada")
                    {
                        if (ddlMotivoCancelacion.SelectedValue == "-1")
                        {
                            this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_MOTIVO_CANCELACION);//"Debe seleccionar un motivo de cancelación.");
                            return;
                        }
                    }

                    string id_solicitud = hdnIdSolicitud.Value;

                    hdnIdSolicitud.Value = id_solicitud;
                    string contrato = lblCodContratoInfo.Text;
                    string cod_tipo_solicitud = "001";
                    string cod_subtipo_solicitud = ddlSubtipo.SelectedValue;
                    string cod_estado = ddlEstado.SelectedValue;
                    string telef_contacto = txtEditarTelefono.Text;
                    string pers_contacto = txtPersonaContacto.Text;
                    string cod_averia = ddlAveria.SelectedValue;
                    string cod_visita = ddlVisita.SelectedValue;
                    string observaciones = "";//txt_ObservacionesAnteriores.Text & (char)(13) & txt_Observaciones.Text
                    SolicitudesDB objSolicitudesDB = new SolicitudesDB();

                    //ProveedoresDB objProveedoresDB = new ProveedoresDB();
                    //string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo(cod_tipo_solicitud, cod_subtipo_solicitud, contrato).Split(';');

                    ////hidden_proveedor.Value = Proveedores[0];
                    ////'hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato)

                    string proveedor = "";// Proveedores[0].ToString().Substring(0, 3); //Mid(hidden_proveedor.Value, 1, 3);

                    if (cmbProveedor.Visible == true)
                    {
                        proveedor = cmbProveedor.SelectedValue;
                        if (proveedor.ToUpper() == "D  " || proveedor.ToUpper() == "D") { return; }
                    }
                    else
                    {
                        proveedor = txtProveedor.Text;
                    }

                    string mot_cancel = ddlMotivoCancelacion.SelectedValue;

                    telef_contacto = telef_contacto.Replace(" ", "");
                    if (!String.IsNullOrEmpty(telef_contacto))
                    {
                        //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                        PhoneValidator telefVal = new PhoneValidator();
                        if (!telefVal.Validate(telef_contacto))
                        {
                            this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                        }

                        //if (telef_contacto.Length != 12)
                        //{
                        //    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_TENER_12_DIGITOS);
                        //}
                        //else
                        //{
                        //    if (telef_contacto.Substring(0, 3) != "+34" && telef_contacto.Substring(0, 4) != "+351")
                        //    {
                        //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                        //    }
                        //    if (telef_contacto.Substring(3, 1) == "7" || telef_contacto.Substring(3, 1) == "6" || telef_contacto.Substring(3, 1) == "9" || telef_contacto.Substring(3, 1) == "8" || telef_contacto.Substring(3, 1) == "2" || telef_contacto.Substring(3, 1) == "3")
                        //    {
                        //    }
                        //    else
                        //    {
                        //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_COMENZAR_6789);//"El telefono debe de comenzar por el digito 6, 7, 9 o 8");
                        //    }
                        //}
                        //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    }

                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    string usuario = user.Login;

                    string Horas = DateTime.Now.Hour.ToString();
                    if (Horas.Length == 1) Horas = "0" + Horas;
                    string Minutos = DateTime.Now.Minute.ToString();
                    if (Minutos.Length == 1) Minutos = "0" + Minutos;

                    string observ_finales = string.Empty;
                    if (txtEditarObservaciones.Text != "")
                    {
                        observ_finales = "[" + DateTime.Now.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + txtEditarObservaciones.Text;
                        observaciones = observ_finales + (char)(13) + txtEditarObservacionesAnteriores.Text;

                        objSolicitudesDB.UpdateObservacionesSolicitud(id_solicitud, observaciones);
                    }
                    else
                    {
                        observaciones = txtEditarObservacionesAnteriores.Text;
                    }

                    if ((cod_subtipo_solicitud == "1" | cod_subtipo_solicitud == "001") & cod_averia == "-1")
                    {
                        lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_CODIGO_AVERIA_OBLIGATORIO;//"El código de averia es obligatorio.";
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    if ((cod_subtipo_solicitud == "4" | cod_subtipo_solicitud == "004") & cod_averia == "-1")
                    {
                        lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_CODIGO_AVERIA_OBLIGATORIO;//"El código de averia es obligatorio.";
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    if ((cod_subtipo_solicitud == "2" | cod_subtipo_solicitud == "002" | cod_subtipo_solicitud == "3" | cod_subtipo_solicitud == "003") & cod_visita == "-1")
                    {
                        lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_CODIGO_VISITA_OBLIGATORIO;//"El código de visita es obligatorio.";
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    //Kintell 14/04/2009.
                    //Actualizamos el estado de Urgente.
                    //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    if (ddlAveria.Visible || !ddlVisita.Visible)
                    {
                        objSolicitudesDB.UpdateSolicitud(id_solicitud, contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, mot_cancel, this.chkUrgente.Checked.ToString(), ChkRetencion.Checked.ToString());
                    }
                    else
                    {
                        objSolicitudesDB.UpdateSolicitud(id_solicitud, contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_visita, observaciones, proveedor, mot_cancel, this.chkUrgente.Checked.ToString(), ChkRetencion.Checked.ToString());
                    }
                    //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente

                    //Kintell 22/11/2011 Tema HORARIO CONTACTO.
                    ActualizarHorariocontacto("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" + cmbHorarioContacto.SelectedValue + "' WHERE COD_CONTRATO_SIC='" + contrato + "'");

                    //Kintell 08/04/2009
                    //Comprobamos k existe o no una caldera para este contrato, para insertar una nueva o modificar la existente.
                    CalderasDB objCalderasDB = new CalderasDB();

                    DataSet ds = new DataSet();
                    ds = objCalderasDB.GetCalderasPorContrato(contrato);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        objSolicitudesDB.InsertarCalderaContrato(contrato, int.Parse(codMarcaCaldera.ToString()), txtModelocaldera.Text);
                    }
                    else
                    {
                        objSolicitudesDB.ActualizarCalderaContrato(contrato, int.Parse(codMarcaCaldera.ToString()), txtModelocaldera.Text, "", "", 0, 0);
                    }

                    //18/12/2017 BGN Capa Negocio actualizar historico para incluir llamada WS Alta Interaccion
                    //HistoricoDB objHistoricoDB = new HistoricoDB();
                    //objHistoricoDB.AddHistoricoSolicitud(id_solicitud.ToString(), "002", usuario, cod_estado, observaciones, proveedor);
                    Solicitud soli = new Solicitud();
                    soli.ActualizarHistoricoSolicitud(contrato, id_solicitud.ToString(), "002", usuario, cod_estado, observ_finales, proveedor, cod_subtipo_solicitud, cod_averia, cod_visita, "M");

                    //Twitter
                    //06/09/2013
                    if (chkTwitter.Checked)
                    {
                        EnviarAviso(id_solicitud.ToString());
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_MODIFICADA_CON_ENVIO_MAIL + id_solicitud);//"Se ha modificado la solicitud " + id_solicitud + " (y se ha enviado un mail al proveedor).");
                    }
                    else
                    {
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_MODIFICADA + id_solicitud);//"Se ha modificado la solicitud " + id_solicitud);
                    }

                    //Facebook
                    //10/03/2015
                    if (chkFacebook.Checked)
                    {
                        EnviarAvisoFacebook(id_solicitud.ToString());
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_MODIFICADA_CON_ENVIO_MAIL + id_solicitud);//"Se ha modificado la solicitud " + id_solicitud + " (y se ha enviado un mail al proveedor).");
                    }
                    else
                    {
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_MODIFICADA + id_solicitud);//"Se ha modificado la solicitud " + id_solicitud);
                    }

                    //----------------------------------------------------------
                    this.ExecuteScript("VerSolicitudes();");
                    hdnModificandoSolicitud.Value = "True";
                    Buscar();
                    hdnModificandoSolicitud.Value = "False";
                }
                else
                {
                    lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_ESTADO_SOLICITUD;// "Debe seleccionar un estado de solicitud.";
                }
            }
            catch (Exception ex)
            {
                lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_NO_SE_HA_PODIDO_MODIFICAR_SOLICITUD + ex.Message;// "No se ha podido modificar la solicitud. Codigo: " + ex.Message;
            }
        }

        private void CargaComboProveedores()
        {
            cmbProveedor.Items.Clear();
            UsuarioDB usuarioDB = new UsuarioDB();
            this.cmbProveedor.DataSource = usuarioDB.ObtenerProveedores();
            cmbProveedor.DataTextField = "PROVEEDOR";
            cmbProveedor.DataValueField  = "PROVEEDOR";
            this.cmbProveedor.DataBind();
            
            ListItem defaultItem = new ListItem();

            //'SIEL
            defaultItem = new ListItem();
            defaultItem.Value = "SIE";
            defaultItem.Text = "SIEL";
            cmbProveedor.Items.Insert(0, defaultItem);
            ////'MGA
            // 04/05/2017 QUITAMOS MGA DE OPERA.
            //defaultItem = new ListItem();
            //defaultItem.Value = "MGA";
            //defaultItem.Text = "MGA";
            //cmbProveedor.Items.Insert(0, defaultItem);
            //'MAPFRE
            defaultItem = new ListItem();
            defaultItem.Value = "MAP";
            defaultItem.Text = "MAPFRE";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'ICISA
            defaultItem = new ListItem();
            defaultItem.Value = "ICI";
            defaultItem.Text = "ICISA";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'ACTIVAIS
            defaultItem = new ListItem();
            defaultItem.Value = "ACT";
            defaultItem.Text = "ACTIVAIS";
            cmbProveedor.Items.Insert(0, defaultItem);

            // TRADIVEL
            defaultItem = new ListItem();
            defaultItem.Value = "TRA";
            defaultItem.Text = "TRADIVEL";
            cmbProveedor.Items.Insert(0, defaultItem);
            //'ACTIVAIS
            //defaultItem = new ListItem();
            //defaultItem.Value = "APP";
            //defaultItem.Text = "APPLUS";
            //cmbProveedor.Items.Insert(0, defaultItem);
            //''Vacio
            //'defaultItem = New ListItem
            //'defaultItem.Value = "-1"
            //'defaultItem.Text = "Seleccione un proveedor"
            //'cmbProveedor.Items.Insert(0, defaultItem)
        }

        private void AltaSolicitud()
        {
            try
            {
                string contrato = lblCodContratoInfo.Text;
                string cod_tipo_solicitud = "001";
                string cod_subtipo_solicitud = ddlSubtipo.SelectedValue;
                string cod_estado = ddlEstado.SelectedValue;
                string telef_contacto = txtEditarTelefono.Text;
                string pers_contacto = txtPersonaContacto.Text;
                string cod_averia = ddlAveria.SelectedValue;
                string cod_visita = ddlVisita.SelectedValue;
                string observaciones = ""; //txt_ObservacionesAnteriores.Text & (char)(13) & txt_Observaciones.Text
                bool sRetencion = ChkRetencion.Checked;

                string EFV = "";
                Int16 numerovisitaSMGAmpliado = 0;

                if (ddlSubtipo.SelectedValue == "-1")
                {
                    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_SUBTIPO);//"Debe seleccionar un subtipo de solicitud.");
                    this.ExecuteScript("VerEditar()");
                    return;
                }
                ProveedoresDB objProveedoresDB = new ProveedoresDB();
                string[] Proveedores = objProveedoresDB.GetProveedorPorTipoSubtipo(cod_tipo_solicitud, cod_subtipo_solicitud, contrato).Split(';');

                //hidden_proveedor.Value = Proveedores[0];
                //'hidden_proveedor.Value = objProveedoresDB.GetProveedorPorTipoSubtipo(ddl_Tipo.SelectedValue, ddl_Subtipo.SelectedValue, contrato)

                string proveedor = Proveedores[0].ToString().Substring(0, 3); //Mid(hidden_proveedor.Value, 1, 3);

                if (cmbProveedor.Visible == true)
                {
                    proveedor = cmbProveedor.SelectedValue;
                    if (proveedor.ToUpper() == "D  " || proveedor.ToUpper() == "D") { return; }
                }
                //if (cmbProveedor.Visible == true)
                //{
                //    proveedor = cmbProveedor.SelectedValue;
                //}

                string usuario = String.Empty;

                //Comprobamos k si es cancelada haya seleccionado un motivo de cancelación.
                //if (this.ddl_Estado.SelectedItem.Text == ObtenerValorPropiedadTextoJavascript("TEXTO_CANCELADA)//"Cancelada")
                if (this.ddl_Estado.SelectedItem.Text == "Cancelada")
                {
                    if (this.ddlMotivoCancelacion.SelectedValue == "-1")
                    {
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_DEBE_SELECCIONAR_MOTIVO_CANCELACION);//"Debe seleccionar un motivo de cancelación.");
                        this.ExecuteScript("VerEditar()");
                        return;
                    }
                }

                telef_contacto = telef_contacto.Replace(" ", "");

                if (!String.IsNullOrEmpty(telef_contacto))
                {
                    //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    PhoneValidator telefVal = new PhoneValidator();
                    if (!telefVal.Validate(telef_contacto))
                    {
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    //if (telef_contacto.Length != 12)
                    //{
                    //    this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_TENER_12_DIGITOS);
                    //    this.ExecuteScript("VerEditar()");
                    //    return;
                    //}
                    //else
                    //{
                    //    if (telef_contacto.Substring(0, 3) != "+34" && telef_contacto.Substring(0, 4) != "+351")
                    //    {
                    //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_INCLUIR_PREFIJO);
                    //        this.ExecuteScript("VerEditar()");
                    //        return;
                    //    }
                    //    if (telef_contacto.Substring(3, 1) == "7" || telef_contacto.Substring(3, 1) == "6" || telef_contacto.Substring(3, 1) == "9" || telef_contacto.Substring(3, 1) == "8" || telef_contacto.Substring(3, 1) == "2" || telef_contacto.Substring(3, 1) == "3")
                    //    {
                    //    }
                    //    else
                    //    {
                    //        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_TELEFONO_DEBE_COMENZAR_6789);//"El telefono debe de comenzar por el digito 6, 7, 9 o 8");
                    //        this.ExecuteScript("VerEditar()");
                    //        return;
                    //    }
                    //}
                    //20220215 BGN MOD END R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                }
                string observ_finales = string.Empty;
                try
                {
                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    usuario = user.Login;
                    string Horas = DateTime.Now.Hour.ToString();
                    if (Horas.Length == 1) Horas = "0" + Horas;
                    string Minutos = DateTime.Now.Minute.ToString();
                    if (Minutos.Length == 1) Minutos = "0" + Minutos;

                    observ_finales = "[" + DateTime.Now.ToString().Substring(0, 10) + "-" + Horas + ":" + Minutos + "] " + usuario + ": " + txtEditarObservaciones.Text;
                    observaciones = observ_finales + (char)(13) + txtEditarObservacionesAnteriores.Text;
                }
                catch (Exception ex)
                {

                }

                if (!String.IsNullOrEmpty(contrato) & cod_tipo_solicitud != "-1" & cod_subtipo_solicitud != "-1" & cod_estado != "-1" & !String.IsNullOrEmpty(telef_contacto) & !String.IsNullOrEmpty(pers_contacto) & !String.IsNullOrEmpty(proveedor))
                {
                    if ((cod_subtipo_solicitud == "1" | cod_subtipo_solicitud == "001") & cod_averia == "-1")
                    {
                        lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_CODIGO_AVERIA_OBLIGATORIO;// "El código de averia es obligatorio.";
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    if ((cod_subtipo_solicitud == "2" | cod_subtipo_solicitud == "002" | cod_subtipo_solicitud == "3" | cod_subtipo_solicitud == "003") & cod_visita == "-1")
                    {
                        lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_CODIGO_VISITA_OBLIGATORIO;//"El código de visita es obligatorio.";
                        this.ExecuteScript("VerEditar()");
                        return;
                    }

                    SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                    //If cod_averia = -1 Then cod_averia = System.DBNull.Value.ToString

                   
                    int id_solicitud;
                    //if ((ddlSubtipo.SelectedValue.Equals("001") || ddlSubtipo.SelectedValue.Equals("004")) && lblServicioInfo.Text.ToUpper().IndexOf("CONFORT") > 0)
                    //{
                    //    if(ddlSubtipo.SelectedValue.Equals("001") )
                    //    {
                    //        id_solicitud = objSolicitudesDB.AddSolicitudAveriaGasConfort(contrato, "012");
                    //    }
                    //    else
                    //    {
                    //        id_solicitud = objSolicitudesDB.AddSolicitudAveriaGasConfort(contrato, "013");
                    //    }
                    //    //id_movimiento = objHistoricoDB.AddHistoricoSolicitud(id_solicitud, "002", usuario, "001", Observaciones, "");
                    //}
                    //else
                    //{
                        if ((ddlAveria.Visible))
                        {
                            id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_averia, observaciones, proveedor, this.chkUrgente.Checked, ChkRetencion.Checked);
                        }
                        else
                        {
                            id_solicitud = objSolicitudesDB.AddSolicitud(contrato, cod_tipo_solicitud, cod_subtipo_solicitud, cod_estado, telef_contacto, pers_contacto, cod_visita, observaciones, proveedor, this.chkUrgente.Checked, ChkRetencion.Checked);
                        }
                   // }

                    //Kintell 22/11/2011 Tema HORARIO CONTACTO.
                    ActualizarHorariocontacto("UPDATE MANTENIMIENTO SET HORARIO_CONTACTO='" + cmbHorarioContacto.SelectedValue + "' WHERE COD_CONTRATO_SIC='" + contrato + "'");
                    
                    //// UPDATE THE NUM SOLICITUDES IF IT IS SMG AMPLIADO OR SMG AMPLIADO INDEPENDIENTE
                    //if (hdnEsSMGAmpliado.Value == "1" || hdnEsSMGAmpliadoIndependiente.Value == "1" || hdnEsSMGAmpliado.Value == "True" || hdnEsSMGAmpliadoIndependiente.Value == "True")
                    //{
                        // WE HAVE A TIGGER THAT DECREASE "1" THE VALUE IF THE ISSUE IS CANCELLED...
                        if (cod_subtipo_solicitud == "001")
                        {
                            ActualizarNumeroAveriasPorcontrato(contrato);
                        }
                    //}

                    //Kintell 08/04/2007
                    //Comprobamos k existe o no una caldera para este contrato, para insertar una nueva o modificar la existente.
                    CalderasDB objCalderasDB = new CalderasDB();

                    DataSet ds = new DataSet();
                    ds = objCalderasDB.GetCalderasPorContrato(contrato);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        objSolicitudesDB.InsertarCalderaContrato(contrato, int.Parse(codMarcaCaldera.ToString()), txtModelocaldera.Text);
                    }
                    else
                    {
                        objSolicitudesDB.ActualizarCalderaContrato(contrato, int.Parse(codMarcaCaldera.ToString()), txtModelocaldera.Text, "", "", 0, 0);
                    }

                    //18/12/2017 BGN Capa Negocio actualizar historico para incluir llamada WS Alta Interaccion
                    //HistoricoDB objHistoricoDB = new HistoricoDB();
                    //objHistoricoDB.AddHistoricoSolicitud(id_solicitud.ToString(), "001", usuario, cod_estado, observaciones, proveedor);
                    Solicitud soli = new Solicitud();
                    soli.ActualizarHistoricoSolicitud(contrato, id_solicitud.ToString(), "001", usuario, cod_estado, observ_finales, proveedor, cod_subtipo_solicitud, cod_averia, cod_visita, "A");


                    //Twitter
                    //06/09/2013

                    if (chkTwitter.Checked)
                    {
                        EnviarAviso(id_solicitud.ToString());
                        this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA_CON_ENVIO_MAIL + id_solicitud);//"Se ha dado de alta la solicitud " + id_solicitud + " (y se ha enviado un mail al proveedor).");
                    }
                    else
                    {
                        //Facebook
                        //10/03/2015

                        if (chkFacebook.Checked)
                        {
                            EnviarAvisoFacebook(id_solicitud.ToString());
                            this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA_CON_ENVIO_MAIL + id_solicitud);//"Se ha dado de alta la solicitud " + id_solicitud + " (y se ha enviado un mail al proveedor).");
                        }
                        else
                        {
                            // Si es solicitud de visita, sacamos datos del proveedor en el mensaje.
                            if (cod_subtipo_solicitud == "002")
                            {
                                this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + " " + id_solicitud + "\\n" + this.lblProveedorInfo.Text + " - " + lblProvTelInfo.Text + "\\n " + Resources.TextosJavaScript.HORARIOProveedor);//: De lunes a viernes de 8h a 20h. Sábados de 9h a 14h");//"Se ha dado de alta la solicitud " + id_solicitud);
                                //this.MostrarMensaje(this.lblProveedorInfo.Text + " - " + lblProvTelInfo.Text + " - " + cmbHorarioContacto.SelectedItem.ToString());
                            }
                            else
                            {
                                this.MostrarMensaje(Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + id_solicitud);//"Se ha dado de alta la solicitud " + id_solicitud);
                            }
                        }
                    }


                    //String Script = "<script language='javascript'>divEditar.style.visibility = 'hidden';</script>";//OcultarCapaEspera();</script>";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTAREDITAR", Script, false);

                    this.ExecuteScript("divEditar.style.visibility = 'hidden';");

                    lblMensajeEditar.Text = "";

                    Buscar();
                }
                else
                {
                    lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_NO_ALTA_FALTAN_CAMPOS_OBLIGATORIOS;//"No se ha dado de alta su solicitud. Todos los campos excepto 'Observaciones' son obligatorios.";
                    this.ExecuteScript("VerEditar()");
                }

            }
            catch (Exception ex)
            {
                lblMensajeEditar.Text = Resources.TextosJavaScript.TEXTO_NO_SE_HA_PODIDO_ALTA_SOLICITUD + ex.Message;// "No se ha podido dar de alta la solicitud. Codigo: " + ex.Message;
                this.ExecuteScript("VerEditar()");
            }
            finally
            {

            }
            //----------------------------------------------------------
            //this.ExecuteScript("VerEditar()");
            //Buscar();
        }

        private void ActualizarHorariocontacto(string Sql)
        {
            SolicitudesDB Db = new SolicitudesDB();
            Db.EjecutarSentencia(Sql);
        }

        private void ActualizarNumeroAveriasPorcontrato(string contrato)
        {
            SolicitudesDB Db = new SolicitudesDB();
            Db.ActualizarNumeroAveriasPorcontrato(contrato);
        }

        protected void btnTelefono_Click(object sender, EventArgs e)
        {
            // Seek the rows whom the last editor is the perfil "phone".
            hdnBusquedaColores.Value = "3";
            hdnIdSolicitud.Value = "";
            CargaSolicitudesPorColores("3", 1);
        }
        protected void btnProveedor_Click(object sender, EventArgs e)
        {
            // Seek the rows whom the last editor is the perfil "provider".
            hdnBusquedaColores.Value = "'1','2','5','6','7','9','10'";
            hdnIdSolicitud.Value = "";
            CargaSolicitudesPorColores("'1','2','5','6','7','9','10'", 1);
        }
        protected void btnAdministrador_Click(object sender, EventArgs e)
        {
            // Seek the rows whom the last editor is the perfil "administrator".
            hdnBusquedaColores.Value = "4";
            hdnIdSolicitud.Value = "";
            CargaSolicitudesPorColores("4", 1);
        }

        private void CargaSolicitudesPorColores(string IdPerfil, Int32 numPagina)
        {
            try
            {
                //Kintell 22/04/2009.
                InicializarControles();
                //Session("IdPerfilColores") = IdPerfil;

                String cod_proveedor = "";
                if (Usuarios.EsTelefono(this.CodPerfil))
                {
                    cod_proveedor = "TEL";
                }
                else
                {
                    if (!Usuarios.EsAdministrador(this.CodPerfil))
                    {
                        cod_proveedor = this.CodProveedor;
                    }
                }

                String cod_contrato = txt_contrato.Text;
                Boolean consultaPendientes = chk_Pendientes.Checked;
                String cod_tipo = "001";
                String cod_subtipo = ddl_Subtipo.SelectedValue;
                String cod_estado = ddl_Estado.SelectedValue;

                if (cod_subtipo == "") { cod_subtipo = "-1"; }
                if (cod_estado == "") { cod_estado = "-1"; }

                String fechaDesde = txtFechaDesde.Text;
                String fechaHasta = txtFechaHasta.Text;
                if (fechaDesde != "") { if (fechaDesde.Length == 9) { fechaDesde = "0" + fechaDesde; } fechaDesde = fechaDesde.Substring(6, 4) + "-" + fechaDesde.Substring(3, 2) + "-" + fechaDesde.Substring(0, 2); }
                if (fechaHasta != "") { if (fechaHasta.Length == 9) { fechaHasta = "0" + fechaHasta; } fechaHasta = fechaHasta.Substring(6, 4) + "-" + fechaHasta.Substring(3, 2) + "-" + fechaHasta.Substring(0, 2); }

                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                String Urgente = "False";
                if (chk_Urgente.Checked) { Urgente = "True"; }

                //*********************************************************

                Int32 intPageIndex = numPagina;
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                CultureInfo culture;
                //culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);//"FR-fr");
                culture = new CultureInfo(CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_CULTURA).ToString());

                DataSet dsSolicitudes = objSolicitudesDB.GetSolicitudesPorProveedorCalderas(cod_proveedor, cod_contrato, consultaPendientes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, Desde.ToString(), Hasta.ToString(), IdPerfil, txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, IdPerfil, usuarioDTO.IdIdioma.ToString(),culture.ToString(),txtDNI.Text,txtCUI.Text);

                gvSolicitudes.Columns[0].Visible = true;
                gvSolicitudes.Columns[1].Visible = true;
                gvSolicitudes.Columns[2].Visible = true;

                gvSolicitudes.DataSource = dsSolicitudes.Tables[0];
                gvSolicitudes.DataBind();

                if (gvSolicitudes.Rows.Count == 1 || Usuarios.EsTelefono(this.CodPerfil))
                {
                    hdnIdSolicitud.Value = dsSolicitudes.Tables[0].Rows[0].ItemArray[1].ToString();
                }

                if (Desde == 0 || Desde == 1)
                {
                    String contador = gvSolicitudes.Rows.Count.ToString();
                    if (gvSolicitudes.Rows.Count > PageSize - 1)
                    {
                        DataSet dsSolicitudesContador = objSolicitudesDB.GetSolicitudesPorProveedorCalderas(cod_proveedor, cod_contrato, consultaPendientes, cod_tipo, cod_subtipo, cod_estado, fechaDesde, fechaHasta, ddl_Provincia.SelectedValue, Urgente, "0", "999999999", IdPerfil, txt_solicitud.Text, txtNombre.Text, txtApellido1.Text, txtApellido2.Text, IdPerfil, usuarioDTO.IdIdioma.ToString(),culture.ToString());
                        contador = (dsSolicitudesContador.Tables[0].Rows.Count - 1).ToString();
                    }

                    lblNumEncontrados.Text = contador.ToString();
                    hdnPageCount.Value = ObtenerNumPaginas(Convert.ToInt32(contador)).ToString();
                }
                lblNumEncontrados1.Text = (Desde + 1).ToString();
                if (Hasta <= int.Parse(lblNumEncontrados.Text))
                {
                    lblNumRegistros.Text = Hasta.ToString();
                }
                else
                {
                    lblNumRegistros.Text = lblNumEncontrados.Text.ToString();
                }
                CargarPaginador(numPagina);
            }
            catch (Exception ex)
            {
                gvSolicitudes.DataSource = null;
                gvSolicitudes.DataBind();
            }
        }

        protected void btnExportarExcelBeretta_Click(object sender, EventArgs e)
        {
            ExportarInformeBeretta();
        }

        protected void btnAbrirSolicitudGC_Click(object sender, EventArgs e)
        {
            CrearSolitudGCSobreContratoFicticio();
        }

        private void CrearSolitudGCSobreContratoFicticio()
        {
            try
            {
                // KINTELL 30/12/2021 R#35436
                //R#35436
                UtilidadesWebServices uw = new UtilidadesWebServices();
                string resultado = uw.llamadaWSobtenerValcred(DNI, CP);
                XmlNode datos = uw.StringAXMLValcred(resultado);
                string valoracion = datos.InnerText;
                string contrato = this.lblCodContratoInfo.Text;

                String resultadoLlamadaObtenerDatosContratoDelta = uw.llamadaWSobtenerDatosCtoOperaSMG(contrato);
                XmlNode datosXML = uw.StringAXMLGCTemporal(resultadoLlamadaObtenerDatosContratoDelta);
                Mantenimiento mantenimiento = new Mantenimiento();
                MantenimientoDTO mantenimientoDTO = new MantenimientoDTO();

                if (datosXML != null)
                {
                    mantenimientoDTO = mantenimiento.DatosMantenimientoDesdeDeltaYAltaContrato(datosXML,false);
                    if (mantenimientoDTO.ESTADO_CONTRATO == "TA")
                    {
                        contrato = mantenimientoDTO.COD_CONTRATO_SIC;
                    }
                }

                SolicitudDB sol = new SolicitudDB();
                Int64 idSolicitud=sol.AltaSolicitudGCContratoFicticio(contrato, valoracion, DNI);

                string mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLO_PAGO_ANTICIPADO;
                
                if (valoracion== "SCM09")
                {
                    // Todas las formas de pago
                    mensajeValoracion = Resources.TextosJavaScript.TEXTO_TODOS_PAGOS;
                }
                mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + " " + idSolicitud + "  " + mensajeValoracion;

                MostrarMensaje(mensajeValoracion);

                ConfiguracionDTO confActivoAltaGC = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ActivoLlamadaWSProveedorAltaGC);
                Boolean ActivoLlamadaWSProveedorAltaGC = false;

                if (Boolean.Parse(confActivoAltaGC.Valor))
                {
                    ActivoLlamadaWSProveedorAltaGC = Boolean.Parse(confActivoAltaGC.Valor);
                }
                if (ActivoLlamadaWSProveedorAltaGC)
                {
                    if (String.IsNullOrEmpty(mantenimientoDTO.COD_CONTRATO_SIC))
                    {
                        mantenimientoDTO = mantenimiento.DatosMantenimientoSinPais(contrato);
                        llamarWsProveedorAperturaSolicitudGC(idSolicitud.ToString(), mantenimientoDTO);
                    }
                }

                Buscar();
            }
            catch (Exception ex)
            {

            }
        }

        protected void llamarWsProveedorAperturaSolicitudGC(string idSolicitud, MantenimientoDTO mantenimiento)
        {
            string proveedor = mantenimiento.PROVEEDOR_AVERIA;
            string cod_contrato = mantenimiento.COD_CONTRATO_SIC;
            string codSubtipoSolicitud = "008";
            string codEstadoSol = "035";
            string personaContacto = mantenimiento.NOM_TITULAR;
            string telefonoContacto = mantenimiento.NUM_MOVIL;
            string observaciones = "";
            string tipoOperacion = "A";
            string categoriaVisita = "2";
            string sociedad = mantenimiento.SOCIEDAD;
            string urgencia = "";
            string averiasRealizadas = "0";
            string codCategoriaVisita = "2";

            String sProv = proveedor.Substring(0, 3).ToUpper();
            string resultadoLlamadaWSAperturaSolic = "";
            UtilidadesWebServices uw = new UtilidadesWebServices();
            
            switch (sProv)
            {
                case "ICI":
                case "TRA":
                case "ACT":
                    {
                        resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudActivais(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, sProv, tipoOperacion, mantenimiento, codCategoriaVisita, sociedad);
                        break;
                    }
                case "SIE":
                    {
                        resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudSiel(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, tipoOperacion, mantenimiento, categoriaVisita, sociedad);
                        break;
                    }
                case "MAP":
                    {
                        resultadoLlamadaWSAperturaSolic = uw.llamadaWSAperturaSolicitudMapfre(cod_contrato, idSolicitud, codSubtipoSolicitud, codEstadoSol, personaContacto, telefonoContacto, observaciones, tipoOperacion, mantenimiento, urgencia, averiasRealizadas, sociedad);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (!String.IsNullOrEmpty(resultadoLlamadaWSAperturaSolic))
            {
                // MANDAR MAIL ERROR
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Se ha producido un error al llamar al WS AperturaSolicitud de " + proveedor + ". CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaWSAperturaSolic, "[SMG] Error llamada WS AperturaSolicitudProveedor");
                UtilidadesMail.EnviarMensajeError(" Error llamada WS AperturaSolicitudProveedor", "Se ha producido un error al llamar al WS AperturaSolicitud de " + proveedor + ". CodContrato: " + cod_contrato + " IdSolicitud: " + idSolicitud + ". Message: " + resultadoLlamadaWSAperturaSolic);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            else
            {
                Mantenimiento.InsertarSolicitudWSCreadaPorWebEnEnviados(cod_contrato, idSolicitud);
            }
        }

        protected void btnExportarExcelSolicitudes_Click(object sender, EventArgs e)
        {
            ExportarExcel();
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
                    //MandarMail(Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_TWITTER + lblCodContratoInfo.Text + " - " + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + lblTelefonoContacto.Text, Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + lblCodContratoInfo.Text.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(lblCodContratoInfo.Text.ToString()), Destino, true);
                    string asunto = Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + lblCodContratoInfo.Text.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(lblCodContratoInfo.Text.ToString());
                    string mensaje = Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_TWITTER + lblCodContratoInfo.Text + " - " + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + lblTelefonoContacto.Text;
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
                    //MandarMail(Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_FACEBOOK + lblCodContratoInfo.Text + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + lblTelefonoContacto.Text, Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + lblCodContratoInfo.Text.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(lblCodContratoInfo.Text.ToString()), Destino, true);
                    string asunto = Resources.TextosJavaScript.TEXTO_NUEVO_AVISO_CONTRATO + lblCodContratoInfo.Text.ToString() + " - " + Mantenimiento.ObtenerProvinciaPorContrato(lblCodContratoInfo.Text.ToString());
                    string mensaje = Resources.TextosJavaScript.TEXTO_CLIENTE_PREFERENTE_FACEBOOK + lblCodContratoInfo.Text + " - (" + Resources.TextosJavaScript.TEXTO_CODIGO_SOLICITUD + idSolicitud + ") - " + lblClienteInfo.Text + " - " + lblSuministroInfo.Text + " - " + lblTelefonoContacto.Text;
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
                if (Datos != null)
                {
                    Datos.Close();
                }
            }
        }

        protected void btnExportarExcelPreciosCalderas_Click(object sender, EventArgs e)
        {
            ExportarExcelPreciosCalderas();
        }
        protected void ExportarExcelPreciosCalderas()
        {
            try
            {
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;

                SolicitudesDB objSolicitudesDB = new SolicitudesDB();
                DataSet dsSolicitudes = objSolicitudesDB.GetPreciosCalderasExcel(usuarioDTO.IdIdioma);
                if (dsSolicitudes != null)
                {
                    if (dsSolicitudes.Tables.Count > 0)// && dsSolicitudes.Tables.Count <= Int32.Parse(Resources.SMGConfiguration.ExcelMaxRows))
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dsSolicitudes.Tables[0]);
                        ExcelHelper excel = new ExcelHelper(Resources.TextosJavaScript.TEXTO_LISTADO_CALDERAS, HttpContext.Current.Server.MapPath(Resources.SMGConfiguration.XsltConsultas));
                        ExcelHeaderAttribute headerAttrib = new ExcelHeaderAttribute("Fecha", DateTime.Now.ToShortDateString());
                        excel.TableName = Resources.TextosJavaScript.TEXTO_CALDERAS;//"Calderas";
                        excel.Attributtes.Add(headerAttrib);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_EXISTE_SOLICITUDES_EXPORTAR);//"No existen solicitudes a exportar");
            }
            finally
            {
                String Script = "<script language='javascript'>ExportarExcel();</script>";//OcultarCapaEspera();</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPACALDERAS", Script, false);
            }
        }
        
        protected void btnDetalleVisita_Click(object sender, EventArgs e)
        {
            SaveSessionData();
            this.SaveSessionData();
            NavigationController.Forward("./FrmDetalleVisita.aspx?idContrato=" + this.lblCodContratoInfo.Text + "&idVisita=" + lblNumeroVisita.Text + "&TotalVisitas=" + gvVisitas.Rows[0].Cells[1].Text.ToString());
        }

       
    }
}