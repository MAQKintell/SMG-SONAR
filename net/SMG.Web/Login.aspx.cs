using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Logging;
using Iberdrola.Commons.Messages;
using Iberdrola.Commons.Modules.Authentication.BLL;
using Iberdrola.Commons.Modules.Authentication.DAL.DTO;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Iberdrola.SMG.UI
{
    public partial class Login : System.Web.UI.Page
    {
        GlobalResourcesToJavaScript resourcesToJs = null;

        protected void Button2_Click(object sender, EventArgs e)
        {
            GenerarPDF("miguel-angel.quintela@inetum.world");
        }

        public void EscribirTXT(string texto)
        {
            using (ManejadorFicheros.GetImpersonator())
            {
                StreamWriter sw = new StreamWriter(@"\\CLBILANAS01\Gesdocor\Comercial\GD_SMG\INTEGRACION\EDATALIA\LogMacro" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                sw.WriteLine(texto);
                sw.Flush();
                sw.Close();
            }
        }

        private void GenerarPDF(string email)
        {
            string hdnContrato = "2009821008";
            string hdnIdSolicitud = "4870125";

            try
            {
                EscribirTXT("Empezando " + DateTime.Now.ToString("HH:mm:ss"));
                //AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                //UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                if (hdnContrato== null)
                {
                    //MostrarMensaje(Resources.TextosJavaScript.TEXTO_NO_HAY_CODIGO_CONTRATO);//"No hay código de contrato");
                }
                else
                {
                    if (String.IsNullOrEmpty(email))
                    {
                       // MostrarMensaje(Resources.TextosJavaScript.NO_EMAIL_DESTINO);//"No hay email al que mandar el contrato para firmar digitalmente"
                    }
                    else
                    {
                        //TODO: Comprobar nomenclatura fichero 1010394220_20210122_SIE_CGC.pdf o 101345793920210122SIECGC_1013457939.pdf este ultimo es el que te pide al adjuntar por pantalla
                        string nombreFichero = PCamposPdfContratoGasConfort.GenerarContratoGasconfort(hdnContrato, Convert.ToDecimal(hdnIdSolicitud));
                        if (String.IsNullOrEmpty(nombreFichero))
                        {
                            //MostrarMensaje(Resources.TextosJavaScript.ERROR_GENERAR_CONTRATO_GC_DIGITAL);//"Error al generar el contrato digital. No enviado a Edatalia"
                        }
                        else
                        {
                            string refExternaEdatalia = hdnIdSolicitud + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string nombreCliente = String.Empty;
                            string dniCliente = String.Empty;
                            string movilCliente = String.Empty;
                            string idDocEdatalia = String.Empty;
                            IDataReader datosMantenimiento = Mantenimiento.ObtenerDatosMantenimientoPorcontrato(hdnContrato);
                            if (datosMantenimiento != null)
                            {
                                while (datosMantenimiento.Read())
                                {
                                    nombreCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NOM_TITULAR")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO1")).TrimEnd() + " " + ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "APELLIDO2")).TrimEnd();
                                    dniCliente = (String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "DNI");
                                    movilCliente = ((String)DataBaseUtils.GetDataReaderColumnValue(datosMantenimiento, "NUM_MOVIL")).TrimEnd();
                                }
                            }
                            //ThreadPool.QueueUserWorkItem((o) =>
                            //{
                                if (!String.IsNullOrEmpty(email))
                                {
                                    //Llamar a WS Edatalia para que envie un email al cliente para que firme el contrato de GC
                                    EscribirTXT("Llamada " + DateTime.Now.ToString("HH:mm:ss"));

                                    UtilidadesWebServices callWS = new UtilidadesWebServices();
                                    idDocEdatalia = callWS.llamadaWSEdataliaAltaDocumentoPorEmail(nombreFichero, refExternaEdatalia, nombreCliente, dniCliente, email, movilCliente);
                                    EscribirTXT("FIN Llamada " + DateTime.Now.ToString("HH:mm:ss") + " - " + idDocEdatalia);
                                }
                                if (!String.IsNullOrEmpty(idDocEdatalia))
                                {
                                    //Guardar en tabla DOCUMENTO
                                    DocumentoDTO documentoDto = new DocumentoDTO();
                                    documentoDto.CodContrato = hdnContrato;
                                    documentoDto.IdSolicitud = int.Parse(hdnIdSolicitud);
                                    documentoDto.NombreDocumento = nombreFichero;
                                    documentoDto.IdTipoDocumento = 1;
                                    documentoDto.EnviarADelta = false;
                                    documentoDto.IdEnvioEdatalia = refExternaEdatalia;
                                    documentoDto.FechaEnvioEdatalia = DateTime.Now;
                                    documentoDto.IdDocEdatalia = idDocEdatalia;
                                    documentoDto = Documento.Insertar(documentoDto, "E013104");
                                    //Mover fichero a RUTA_FICHEROS_EDATALIA_ENVIADOS
                                    ConfiguracionDTO rutaEdatalia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA);
                                    String ficheroOrigen = rutaEdatalia.Valor + nombreFichero;
                                    ConfiguracionDTO rutaEdataliaEnviados = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA_ENVIADOS);
                                    FileUtils.FileMoveRewrite(ManejadorFicheros.GetImpersonator(), ficheroOrigen, rutaEdataliaEnviados.Valor);
                                }
                                else
                                {
                                    // MostrarMensaje(Resources.TextosJavaScript.ERROR_ENVIO_CONTRATO_GC_DIGITAL); //"Error al enviar el contrato digital a Edatalia"
                                }
                            //});
                        }
                    }
                }
                EscribirTXT("Fin " + DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                EscribirTXT(ex.Message + " - " + DateTime.Now.ToString("HH:mm:ss"));
                //UtilidadesMail.EnviarMensajeError("Error al Generar contrato GC y enviarlo a Edatalia", "Se ha producido un error al Generar contrato GC y enviarlo a Edatalia. Contrato: " + hdnContrato + " IdSolicitud: " + hdnIdSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);

            }
            finally
            {
                //String Script = "<script language='javascript'>OcultarCapaEspera();</script>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OCULTARCAPA", Script, false);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////R#35436
                //UtilidadesWebServices uw = new UtilidadesWebServices();
                //string resultado = uw.llamadaWSobtenerValcred("14609664H","48014");
                //XmlNode datos = uw.StringAXMLValcred(resultado);
                //string valoracion = datos.InnerText;

                //R#35439
                //String resultadoLlamadaObtenerDatosContratoDelta = uw.llamadaWSobtenerDatosCtoOperaSMG("2011928066");
                //XmlNode datos = uw.StringAXMLRepairAndCare(resultadoLlamadaObtenerDatosContratoDelta, "2011928066");
                //Mantenimiento mantenimiento = new Mantenimiento();
                //MantenimientoDTO mantenimientoDTO = mantenimiento.DatosMantenimientoDesdeDeltaYAltaContrato(datos, true);


                string contrato = Request.QueryString["CONTRATO"];
                string user = Request.QueryString["USUARIO"];
                string expired = Request.QueryString["EXPIRED"];
                string strFirma = Request.QueryString["KEY"];

                Boolean desdeDelta = false;
                Boolean mostrarPaginaObras = false;
                ConfiguracionDTO confMostrarPaginaObras = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.MOSTRAR_PAGINA_OBRAS);
                if (confMostrarPaginaObras != null && !string.IsNullOrEmpty(confMostrarPaginaObras.Valor) && Boolean.Parse(confMostrarPaginaObras.Valor))
                {
                    mostrarPaginaObras = Boolean.Parse(confMostrarPaginaObras.Valor);
                }
                //Boolean mostrarPaginaObras = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.MOSTRAR_PAGINA_OBRAS);
                if (!mostrarPaginaObras)
                {
                    if (user == null)
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, false);
                        PonerFoco(this.txtUsuario);
                    }
                    else
                    {
                        // 21/03/2017 (Kintell).
                        // Venimos de Delta
                        DateTime fechaExpirar = DateTime.Parse(expired);
                        if (fechaExpirar > DateTime.Now)
                        {
                            bool valido = false;
                            //20200120 BGN MOD BEG [R#21764]: Habilitar/Deshabilitar el acceso por Delta a SMG
                            Boolean habilitarCertif = false;

                            CurrentSession.SetAttribute(CurrentSession.SESSION_BUSQUEDA_DESDE_DELTA, "SI");

                            ConfiguracionDTO configHabilitar = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.HABILITAR_CERTIFICADO_DELTA);
                            if (configHabilitar != null && !string.IsNullOrEmpty(configHabilitar.Valor) && Convert.ToBoolean(configHabilitar.Valor) == true)
                            {
                                habilitarCertif = true;
                            }

                            if (habilitarCertif)
                            {
                                try
                                {
                                    ConfiguracionDTO dbConfigDto = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.IT_PATH_CERTIFICADO_DELTA);
                                    if (dbConfigDto != null && !string.IsNullOrEmpty(dbConfigDto.Valor))
                                    {
                                        string urlOrigen = Request.Url.ToString();
                                        string urlParametros = urlOrigen.Split('?')[1];
                                        string[] parametros = urlParametros.Split('&');
                                        string textoAVerificar = string.Empty;
                                        foreach (string s in parametros)
                                        {
                                            if (!s.Contains("KEY="))
                                            {
                                                textoAVerificar = textoAVerificar + s + "&";
                                            }
                                        }
                                        textoAVerificar = textoAVerificar.Substring(0, textoAVerificar.Length - 1);
                                        valido = EncryptHelper.VerificarFirmaMD5(textoAVerificar, strFirma, dbConfigDto.Valor);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("Error VerificarFirmaMD5: " + Request.Url.ToString(), LogHelper.Category.UserInterface);
                                    LogHelper.Error("Message: " + ex.Message, LogHelper.Category.UserInterface);
                                    LogHelper.Error("StackTrace: " + ex.StackTrace, LogHelper.Category.UserInterface);
                                }
                            }
                            else
                            {
                                valido = true;
                            }
                            //20200120 BGN MOD END [R#21764]: Habilitar/Deshabilitar el acceso por Delta a SMG

                            if (valido)
                            {
                                this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>MostrarCapaEspera();</script>"));
                                accesoDesdeDelta(contrato, user, expired);
                            }
                            else
                            {
                                this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Certificado NO valido.');window.open('','_self','');window.close();</script>"));
                            }
                        }
                        else
                        {
                            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Fecha Expirada.');window.open('','_self','');window.close();</script>"));
                        }
                        desdeDelta = true;
                    }

                    if (!desdeDelta)
                    {
                        try
                        {
                            if (!this.IsPostBack)
                            {
                                this.txtNIF.Style.Value = "visibility:hidden";
                                this.lblNIF.Style.Value = "visibility:hidden";
                                this.txtNIFValidator.Style.Value = "visibility:hidden";


                                divAvisos.Visible = false;
                                ConfiguracionDTO confMostrarAviso = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.MOSTRAR_AVISO_EN_LOGIN);
                                if (confMostrarAviso != null && !string.IsNullOrEmpty(confMostrarAviso.Valor) && Boolean.Parse(confMostrarAviso.Valor))
                                {
                                    ConfiguracionDTO confContenidoAviso = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONTENIDO_AVISO_EN_LOGIN);
                                    if (confContenidoAviso != null && !string.IsNullOrEmpty(confContenidoAviso.Valor))
                                    {
                                        divAvisos.InnerHtml = confContenidoAviso.Valor;
                                        divAvisos.Visible = true;
                                    }
                                }

                                // Cargamos el tipo de logeo.
                                ConfiguracionDTO confTipoLogin = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_COMPROBACION_HABILITADA);
                                if (confTipoLogin != null && !String.IsNullOrEmpty(confTipoLogin.Valor))
                                {
                                    CurrentSession.SetAttribute(SessionVariables.SESSION_USUARIO_TIPO_LOGEO, confTipoLogin.Valor);
                                }

                                // Cargamos si se habilitan las condiciones especiales.
                                ConfiguracionDTO confCondiciones = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LDAP_COMPROBACION_CONDICIONES_HABILITADA);
                                if (confCondiciones != null && !String.IsNullOrEmpty(confCondiciones.Valor))
                                {
                                    CurrentSession.SetAttribute(SessionVariables.SESSION_USUARIO_CONDICIONES, confCondiciones.Valor);
                                }
                            }
                            else if (Convert.ToBoolean(CurrentSession.GetAttribute(SessionVariables.SESSION_USUARIO_CONDICIONES)))
                            {
                                if (string.IsNullOrEmpty(this.txtUsuario.Text) || Usuarios.EsUsuarioIberdrola(this.txtUsuario.Text) || Usuarios.EsUsuarioExcepcion(this.txtUsuario.Text))
                                {
                                    this.txtNIF.Style.Value = "visibility:hidden";
                                    this.lblNIF.Style.Value = "visibility:hidden";
                                    this.txtNIFValidator.Style.Value = "visibility:hidden";
                                }
                                else
                                {
                                    this.txtNIF.Style.Value = "visibility:visible";
                                    this.lblNIF.Style.Value = "visibility:visible";
                                    this.txtNIFValidator.Style.Value = "visibility:visible";

                                }
                            }

                            CurrentSession.SetAttribute(SessionVariables.SESSION_USUARIO_AUTENTIFICADO, false);
                            PonerFoco(this.txtUsuario);
                        }
                        catch (Exception)
                        {
                            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Error interno de la aplicación');window.open('','_self','');window.close();</script>"));
                            //this.MostrarPaginaError(ex);
                        }
                    }
                    //Label1.Attributes.Add("onclick","Button1_Click()");// "javascript:" + ImageButton1.ClientID + ".click()");
                    //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('1')</script>"));
                }
                else
                {
                    // Mostrar página de Obras.
                    // Response.Redirect("../Login_Obras.aspx");
                    ConfiguracionDTO confURLPaginaObras = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.URL_PAGINA_OBRAS);
                    string url = "Login_OBRAS.aspx";
                    if (confURLPaginaObras != null && !string.IsNullOrEmpty(confURLPaginaObras.Valor))
                    {
                        url = confURLPaginaObras.Valor;
                    }
                    HttpContext.Current.Response.Redirect(url, false);
                }

            }
            catch (Exception)
            {
                this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Error interno de la aplicación');window.open('','_self','');window.close();</script>"));
                //this.MostrarPaginaError(ex);
            }
        }

        private void accesoDesdeDelta(string contrato, string usuario, string expired)
        {
            Usuarios usuarios = new Usuarios();



            AppPrincipal appPrincipal = usuarios.GetPrincipal(usuario, "DESDEDELTA");
            HttpContext.Current.User = appPrincipal;


            CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, appPrincipal);
            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, true);
            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_VALIDO, usuario);

            // 08/07/2016 Kintell: Piyamos el idioma no del navegador, si no que de la tabla usuario.
            AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
            List<IdiomaDTO> idiomas = Paises.ObtenerIdiomas();

            foreach (IdiomaDTO idioma in idiomas)
            {
                if (idioma.Id.Equals(usuarioDTO.IdIdioma))//Usuarios.ObtenerIdiomaUsuarioLogeado()))
                //if (idioma.Cultura.ToString().IndexOf(usuarioDTO.Pais) >=0)//Usuarios.ObtenerIdiomaUsuarioLogeado()))
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_CULTURA, idioma.Cultura);
                }
            }


            LogHelper.Error("Usuario validado desde Delta: " + usuario, LogHelper.Category.UserInterface);


            //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));

            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>AbrirVentanaSolicitudesCalderasConContrato(" + contrato + ");</script>"));
            //ComprobarLogin(user, password);


        }

        private void PonerFoco(Control ctrl)
        {
            string focusScript = "<script language='JavaScript'>document.getElementById('" + ctrl.ClientID + "').focus();</script>";

            Page.RegisterStartupScript("FocusScript", focusScript);
        }


        protected override void InitializeCulture()
        {
            if (HttpContext.Current.Request.UserLanguages != null)
            {
                CultureInfo culture;
                culture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture.LCID);
                base.InitializeCulture();
                // SessionVariables session = new SessionVariables();
                // session.EstablecerIdiomaEnLasConstantes(culture.Name);
                // cargamos los textos multiidioma de JavaScript
                if (resourcesToJs == null)
                {
                    resourcesToJs = new GlobalResourcesToJavaScript();
                    resourcesToJs.JavaScriptObjectName = "Resources";

                    this.Controls.Add(resourcesToJs);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('1')</script>"));
            //TODO: validar que el usuario y la password tienen datos.

            //string strUsuario = this.txtUsuario.Text;
            //string strPassword = this.txtPassword.Text;

            //// 21/03/2017 (Kintell)
            //// Tema venir desde Delta.
            //ComprobarLogin(strUsuario, strPassword);
            txtNIF.Text = txtNIF.Text.ToString().ToUpper();
            txtUsuario.Text = txtUsuario.Text.ToString().ToUpper();

            BorrarConfirmacionAutenticacionUsuario();

            string clavePConfigMensaje = string.Empty;

            if (Page.IsValid)
            {
                //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                ConfiguracionDTO confActivarMsgPersonalizados = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.LOGIN_PAGE_MSG_ACTIVAR_MENSAJES_PERSONALIZADOS);
                bool bActivarMsgPersonalizados = false;
                if (confActivarMsgPersonalizados != null)
                    bActivarMsgPersonalizados = bool.Parse(confActivarMsgPersonalizados.Valor);
                //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                try
                {
                    string strUsuario = this.txtUsuario.Text;
                    bool ldap = true;

                    UsuarioDTO usuario = Usuarios.ObtenerUsuario(strUsuario);

                    //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                    if (usuario != null && !string.IsNullOrEmpty(usuario.Activo) && (bool)usuario.BAJA_AUTOMATICA) //
                    {
                        clavePConfigMensaje = "LOGIN_PAGE_MSG_BAJA_POR_INACTIVADAD";

                        throw new BLLException(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString());
                    }
                    else
                    {
                        //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login
                        UsuarioDB usuarioDb = new UsuarioDB();

                        if (usuarioDb.ExisteUsuario(strUsuario))
                        {
                            if (usuario != null)
                            {
                                LoginDTO lg = new LoginDTO();
                                lg.IdNumber = txtNIF.Text;
                                lg.UserId = txtUsuario.Text;
                                lg.Password = txtPassword.Text;

                                bool autenticado = false;
                                bool esUsuarioExcepcion = Usuarios.EsUsuarioExcepcion(usuario.Login);
                                // Si existe el objeto authentication en sesión es que ya se ha validado contra el LDAP y ya no es necesario
                                // sinó se valida
                                if (CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO) != null)
                                {
                                    autenticado = true;
                                }
                                else
                                {
                                    if (Convert.ToBoolean(CurrentSession.GetAttribute(SessionVariables.SESSION_USUARIO_TIPO_LOGEO)) && !esUsuarioExcepcion)
                                    {
                                        // el logeo es a través de LDAP.
                                        try
                                        {
                                            LdapHelper ldapH = new LdapHelper();
                                            autenticado = ldapH.IsAuthenticated(lg);
                                        }
                                        catch (LdapException ldapException)
                                        {
                                            if (ldapException.ErrorCode == 49) //La credencial proporcionada no es válida
                                            {
                                                CultureInfo culture = new CultureInfo("es-ES");
                                                string mensaje = Resources.MensajesError.ResourceManager.GetString(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString());

                                                //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                                                if (bActivarMsgPersonalizados)
                                                {
                                                    ConfiguracionDTO confMsgPersonalizado = Configuracion.ObtenerConfiguracion((Enumerados.Configuracion)Enum.Parse(typeof(Enumerados.Configuracion), "LOGIN_PAGE_MSG_ERROR_EN_LAS_CREDENCIALES"));

                                                    if (confMsgPersonalizado != null)
                                                        mensaje = confMsgPersonalizado.Valor;
                                                }
                                                //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                                                if (mensaje != null)
                                                {
                                                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
                                                    return;
                                                }
                                            }
                                        }
                                        catch (DirectoryOperationException dirOpException)
                                        {
                                            // Objeto no existe
                                            MostrarPaginaError(dirOpException);
                                        }
                                    }
                                    else
                                    {
                                        autenticado = true;
                                        ldap = false;

                                    }
                                }

                                if (autenticado)
                                {

                                    Usuarios usuarios = new Usuarios();
                                    AppPrincipal appPrincipal = usuarios.GetPrincipalLDAP(lg.UserId, lg.Password, ldap);
                                    usuario = (UsuarioDTO)((AppIdentity)appPrincipal.Identity).UserObject;
                                    HttpContext.Current.User = appPrincipal;

                                    if (Convert.ToBoolean(CurrentSession.GetAttribute(SessionVariables.SESSION_USUARIO_CONDICIONES))
                                        && !Usuarios.EsUsuarioIberdrola(lg.UserId.ToString()) && !esUsuarioExcepcion)
                                    {
                                        CondicionesGenerales cg = new CondicionesGenerales(lg.UserId, lg.IdNumber);

                                        bool esUsuarioNoPolitica = Usuarios.EsUsuarioNoPolitica(usuario.Login);

                                        if (cg.TieneNuevasCondiciones(esUsuarioNoPolitica))
                                        {
                                            Session["Login"] = lg.UserId;
                                            CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, appPrincipal);
                                            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, true);
                                            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_VALIDO, lg.UserId);
                                            CurrentSession.SetAttribute(SessionVariables.SESSION_AUTENTIFICACION_USUARIO, cg.Authentication);

                                            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentanaLocalizacion('UI/FrmModalCondicionesGenerales.aspx', '650', '450', 'Aceptación Condiciones Generales', 'Aceptación Condiciones Generales' ,'', false);</script>"));
                                        }
                                        else
                                        {
                                            bool bIgnorarErrores = false;

                                            string textoErrorNuevasCondiciones = "";

                                            if (esUsuarioNoPolitica)
                                            {
                                                //Si se salta las condiciones generales valida solo las condiciones de la tabla
                                                if (cg.FaultNuevasCondiciones != null)
                                                {
                                                    bIgnorarErrores = IgnorarErrorParaUsuarioNoPolitica(Convert.ToInt32(cg.FaultNuevasCondiciones.Detail.ChildNodes[0].SelectSingleNode("faultCode").InnerText.ToString()));
                                                }
                                            }

                                            if (!bIgnorarErrores)
                                            {
                                                if (cg.FaultNuevasCondiciones != null)
                                                {
                                                    textoErrorNuevasCondiciones = cg.ObtenerTextoErrorNuevasCondiciones();
                                                }
                                            }

                                            if (string.IsNullOrEmpty(textoErrorNuevasCondiciones))
                                            {
                                                Session["Login"] = lg.UserId;
                                                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, appPrincipal);
                                                CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, true);
                                                CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_VALIDO, strUsuario);

                                                LogHelper.Error("Usuario Correctamente válidado: " + strUsuario, LogHelper.Category.UserInterface);
                                                abrirVentana(appPrincipal, strUsuario);
                                                //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));
                                            }
                                            else
                                            {
                                                //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                                                if (bActivarMsgPersonalizados)
                                                {
                                                    ConfiguracionDTO confMsgPersonalizado = Configuracion.ObtenerConfiguracion((Enumerados.Configuracion)Enum.Parse(typeof(Enumerados.Configuracion), "LOGIN_PAGE_MSG_CONDICIONES_NO_FIRMADAS"));

                                                    if (confMsgPersonalizado != null)
                                                        textoErrorNuevasCondiciones = confMsgPersonalizado.Valor;
                                                }
                                                //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login


                                                this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + textoErrorNuevasCondiciones + "')</script>"));
                                            }
                                        }
                                    }
                                    else if (Usuarios.EsUsuarioIberdrola(lg.UserId.ToString()))
                                    {
                                        Session["Login"] = lg.UserId;
                                        // Se trata de un usuario de Iberdrola.
                                        abrirVentana(appPrincipal, strUsuario);
                                    }
                                    else
                                    {
                                        // Se trata de un usuario externo pero las condiciones generales no están activadas.
                                        if (ContraseniaCumpleValidaciones(lg.Password))
                                        {
                                            abrirVentana(appPrincipal, strUsuario);
                                        }
                                        else
                                        {
                                            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirCambioPassword(true, false);</script>"));
                                        }
                                    }
                                }
                                else
                                {
                                    //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                                    clavePConfigMensaje = "LOGIN_PAGE_MSG_ERROR_EN_LAS_CREDENCIALES";
                                    //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                                    throw new BLLException(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString());
                                }

                            }
                            else
                            {
                                //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                                clavePConfigMensaje = "LOGIN_PAGE_MSG_NO_REGISTRADO_APLICACION"; //Si existe en la tabla USUARIOS pero no existe en la tabla USUARIO_PAIS
                                                                                                 //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                                throw new BLLException(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString());
                            }
                        }
                        else
                        {
                            //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                            clavePConfigMensaje = "LOGIN_PAGE_MSG_NO_REGISTRADO_APLICACION";
                            //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                            throw new BLLException(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString());
                        }
                    }
                }
                catch (BaseException ex)
                {
                    HttpContext.Current.User = null;
                    LogHelper.Warn("Error validando el usuario en el login", LogHelper.Category.BussinessLogic, ex);

                    if (ex.ErrorCode.Equals(MessagesManager.CommonErrorMessages.ErrorLDAPCedenciales.GetHashCode().ToString()))
                    {
                        hdnNumReintentos.Value = (int.Parse(hdnNumReintentos.Value) + 1).ToString();
                    }

                    if (int.Parse(this.hdnNumReintentos.Value) >= Int32.Parse(Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.CONF_NUM_INTENTOS_PASSWORD).Valor))
                    {
                        Button1.Enabled = false;
                        this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Ha superado el número de intentos.');window.open('','_self','');</script>"));
                    }
                    else
                    {
                        String mensaje = Resources.MensajesError.ResourceManager.GetString(ex.ErrorCode);

                        //20211117 BUA BEG R#22788 - Cambiar los mensajes de error en la ventana del login
                        if (bActivarMsgPersonalizados)
                        {
                            ConfiguracionDTO confMsgPersonalizado = Configuracion.ObtenerConfiguracion((Enumerados.Configuracion)Enum.Parse(typeof(Enumerados.Configuracion), clavePConfigMensaje));

                            if (confMsgPersonalizado != null)
                                mensaje = confMsgPersonalizado.Valor;
                        }
                        //20211117 BUA END R#22788 - Cambiar los mensajes de error en la ventana del login

                        this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
                    }
                }
                catch (Exception ex)
                {
                    // Cuando se produce un error en el web service para consultar las nuevas condiciones.
                    this.MostrarPaginaError(ex);
                }
            }
            else
            {
                this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Corrija los campos inválidos antes de continuar')</script>"));
            }

        }

        private bool TieneEmail(UsuarioDTO usuario)
        {
            if (string.IsNullOrEmpty(usuario.Email))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Método para borrar la información referente a si el usuario está autenticado o no.
        /// </summary>
        private void BorrarConfirmacionAutenticacionUsuario()
        {
            if (CurrentSession.GetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO) != null)
            {
                CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, null);
            }
        }

        /// <summary>
        /// Indica si siendo un usuario que se tiene que saltar las políticas
        /// debe ignorar el error
        /// </summary>
        /// <param name="codError"></param>
        /// <returns></returns>
        private bool IgnorarErrorParaUsuarioNoPolitica(Int32 codError)
        {
            ErrorCondicionGeneralDTO errorCondicionGeneral = recogerError(codError);

            if (errorCondicionGeneral == null)
            {
                // Si el error no lo tenemos registrado, entonces no nos lo saltamos
                // ya que no sabemos si se lo puede saltar o no
                return false;
            }


            if (errorCondicionGeneral.CasoT10T11 == false)
            {
                // Si sí tenemos el error Registrado si tenemos false en CasoT10T11
                // quiere decir que sí nos lo podemos saltar, con lo que retornamos true.
                return true;
            }
            else
            {
                // si en campo CasoT10T11 tenemos un true, quiere decir
                // que ese error lo tenemos que tener en cuenta y por lo tanto no nos lo podemos saltar.
                return false;
            }
        }

        private ErrorCondicionGeneralDTO recogerError(Int32 codError)
        {
            if (codError != null && codError != 0 && codError != Int32.MinValue)
                return ErroresCondicionesGenerales.ObtenerError(codError);
            else
                return null;
        }

        private void abrirVentana(AppPrincipal appPrincipal, string strUsuario)
        {
            CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, appPrincipal);
            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, true);
            CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_VALIDO, strUsuario);

            UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)appPrincipal.Identity).UserObject;
            List<IdiomaDTO> idiomas = Paises.ObtenerIdiomas();
            foreach (IdiomaDTO idioma in idiomas)
            {
                if (idioma.Id.Equals(usuarioDTO.IdIdioma))
                {
                    CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_CULTURA, idioma.Cultura);
                }
            }
            this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));
        }

        private void ComprobarLogin(string strUsuario, string strPassword)
        {
            Usuarios usuarios = new Usuarios();

            try
            {
                AppPrincipal appPrincipal = usuarios.GetPrincipal(strUsuario, strPassword);
                HttpContext.Current.User = appPrincipal;


                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_USUARIO, appPrincipal);
                CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_AUTENTIFICADO, true);
                CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_VALIDO, strUsuario);

                // 08/07/2016 Kintell: Piyamos el idioma no del navegador, si no que de la tabla usuario.
                AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                List<IdiomaDTO> idiomas = Paises.ObtenerIdiomas();

                foreach (IdiomaDTO idioma in idiomas)
                {
                    if (idioma.Id.Equals(usuarioDTO.IdIdioma))//Usuarios.ObtenerIdiomaUsuarioLogeado()))
                    //if (idioma.Cultura.ToString().IndexOf(usuarioDTO.Pais) >=0)//Usuarios.ObtenerIdiomaUsuarioLogeado()))
                    {
                        CurrentSession.SetAttribute(CurrentSession.SESSION_USUARIO_CULTURA, idioma.Cultura);
                    }
                }


                LogHelper.Error("Usuario Correctamente validado: " + strUsuario, LogHelper.Category.UserInterface);

                if (!ContraseniaCumpleValidaciones(strPassword))
                {
                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirCambioPassword(true, false);</script>"));
                }
                else if (Usuarios.ContraseniaCaducada(strUsuario))
                {
                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirCambioPassword(false, true);</script>"));
                }
                else
                {
                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));
                }

                //this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>abrirVentana();</script>"));
            }

            catch (BaseException ex)
            {
                HttpContext.Current.User = null;
                LogHelper.Warn("Error validando el usuario en el login", LogHelper.Category.BussinessLogic, ex);

                if (ex.ErrorCode.Equals("2001"))
                {
                    hdnNumReintentos.Value = (int.Parse(hdnNumReintentos.Value) + 1).ToString();
                }

                if (int.Parse(this.hdnNumReintentos.Value) >= 3)
                {
                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + Resources.TextosJavaScript.NUMERO_DE_INTENTOS_SUPERADOS + "');window.open('','_self','');window.close();</script>"));
                }
                else
                {
                    String mensaje = Resources.MensajesError.ResourceManager.GetString(ex.ErrorCode);
                    this._PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('" + mensaje + "')</script>"));
                }

                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMail(ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_" + ex.ErrorCode + "_" + strUsuario + ":" + strPassword, "Error al Logearse.");
                UtilidadesMail.EnviarMensajeError("Error al Logearse.", ex.Message + " _Trace:" + ex.StackTrace + "_" + ex.Source + "_" + ex.Data + "_" + ex.ErrorCode + "_" + strUsuario + ":" + strPassword);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
        }

        private static bool ContraseniaCumpleValidaciones(string strPassword)
        {
            if (strPassword.Length == 0)
            {
                return false;
            }
            else if (strPassword.Length < PasswordUtils.PASSWORD_LENGHT)
            {
                return false;
            }
            else if (!PasswordUtils.ValidatePassword(strPassword, PasswordUtils.PASSWORD_NUMERIC_CHAR_COUNT))
            {
                return false;
            }
            else if (!PasswordUtils.ValidatePassword(strPassword, PasswordUtils.PASSWORD_UPPER_CASE_CHAR_COUNT, PasswordUtils.PASSWORD_LOWER_CASE_CHAR_COUNT))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Muestra la página de error de la excepción producida
        /// </summary>
        /// <param name="ex">Excepción con los datos a mostrar</param>
        protected void MostrarPaginaError(Exception ex)
        {
            if (typeof(BaseException) == ex.GetType().BaseType)
            {
                MostrarPaginaErrorBase((BaseException)ex);
            }
            else
            {
                LogHelper.Category logCategory;
                logCategory = LogHelper.Category.General;
                LogHelper.Error(ex.Message, logCategory, ex);

                //Error err = new Error(ex.Message);
                Error err = new Error("4002", Iberdrola.Commons.Web.Error.SeverityLevel.Error);

                Errors errs = new Errors();
                errs.Add(err);
                CurrentSession.SetAttribute(CurrentSession.SESSION_ERROR_VARIABLE, errs);

                //ManejadorMensajesError.EnviarMensajeExcepcion("Excepcion en la pantalla Login", ex);

                NavigationController.Forward("UI/MostrarErrorLogin.aspx");
            }
        }

        protected void MostrarPaginaErrorBase(BaseException be)
        {
            // Escribir el error en el Log
            LogHelper.Category logCategory;

            if (typeof(ArqException) == be.GetType())
            {
                logCategory = LogHelper.Category.Architecture;
            }
            else if (typeof(DALException) == be.GetType())
            {
                logCategory = LogHelper.Category.DataAccess;
            }
            else if (typeof(BLLException) == be.GetType())
            {
                logCategory = LogHelper.Category.BussinessLogic;
            }
            else if (typeof(UIException) == be.GetType())
            {
                logCategory = LogHelper.Category.UserInterface;
            }
            else
            {
                logCategory = LogHelper.Category.Architecture;
            }

            if (be.Recuperable)
            {
                LogHelper.Warn(be.Message, logCategory, be);
            }
            else
            {
                LogHelper.Error(be.Message, logCategory, be);
            }

            // Mostrar la página de error
            Error err = Iberdrola.Commons.Web.Error.GetError(be);
            Errors errs = new Errors();
            errs.Add(err);
            CurrentSession.SetAttribute(CurrentSession.SESSION_ERROR_VARIABLE, errs);
            //NavigationController.Forward("./FrmMostrarError.aspx", (FrmBase)this.ContentPlaceHolderContenido.Page);
            //NavigationController.Forward("./FrmMostrarError.aspx", this.ViewState);
            NavigationController.Forward("./MostrarErrorLogin.aspx");
        }

        #region "RECORDAR CONTRASEÑA"
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //int a = 0;
        }
        #endregion

        protected void Page_OnUnload(object sender, EventArgs e)
        {
            //String a = "";
        }

        /// <summary>
        /// Evento que se lanza al validar el Nif
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        public void OnNIF_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(this.txtUsuario.Text) && !Usuarios.EsUsuarioIberdrola(this.txtUsuario.Text)
            //        && Convert.ToBoolean(CurrentSession.GetAttribute(SessionVariables.SESSION_USUARIO_CONDICIONES)) && !Usuarios.EsUsuarioExcepcion(this.txtUsuario.Text))
            //    {
            //        e.IsValid = false;

            //        if (!string.IsNullOrEmpty(e.Value))
            //        {
            //            NIFValidator nifValidator = NIFValidator.CompruebaNif(e.Value);
            //            e.IsValid = nifValidator.EsCorrecto;
            //            if (!e.IsValid)
            //            {
            //                ((CustomValidator)sender).ToolTip = "NIF/NIE Incorrecto";
            //            }
            //        }
            //        else
            //        {
            //            ((CustomValidator)sender).ToolTip = "Campo No Informado";
            //        }
            //    }
            //    else
            //    {
            //        e.IsValid = true;
            //    }


            //    if (!e.IsValid)
            //    {
            //        // Comprobar DNI Portugues.
            //        string dni = e.Value;
            //        //DESCRIPCIÓN DE LA FUNCIONALIDAD
            //        //Se comprobará que Número de Documento es un numérico de 9 posiciones; en caso contrario, se rechazará el Número de Documento.
            //        //Se comprobará que el primer número sea diferente de 0 y 8; en caso contrario, se rechazará el Número de Documento.
            //        //Se cogen las primeras 8 posiciones del número; a cada uno de los dígitos se le multiplica por un factor en orden decreciente:
            //        //-	El primer dígito se multiplica por 9
            //        //-	El segundo dígito se multiplica por 8
            //        //-	El tercer dígito se multiplica por 7
            //        //-	El cuarto dígito se multiplica por 6
            //        //-	El quinto dígito se multiplica por 5
            //        //-	El sexto dígito se multiplica por 4
            //        //-	El séptimo dígito se multiplica por 3
            //        //-	El octavo dígito se multiplica por 2
            //        //Se suman los productos obtenidos, y al resultado se le calcula el módulo de 11. El resultado, a su vez, se resta a 11 (11 menos el resultado).
            //        //El número obtenido es el dígito de control, salvo que sea mayor que 9, en cuyo caso será 0.
            //        //Si el noveno dígito del Número de documento no coincide con el dígito de control, se rechazará el Número de Documento.

            //        if (dni.Length == 9 && NIFValidator.IsNumeric(dni))
            //        {
            //            if (NIFValidator.Left(dni, 1) != "0" && NIFValidator.Left(dni, 1) != "8")
            //            {
            //                char[] characters = dni.ToCharArray();
            //                int primerFactor= int.Parse(characters[0].ToString()) * 9;
            //                int segundoFactor = int.Parse(characters[1].ToString()) * 8;
            //                int tercerFactor = int.Parse(characters[2].ToString()) * 7;
            //                int cuartoFactor = int.Parse(characters[3].ToString()) * 6;
            //                int quintoFactor = int.Parse(characters[4].ToString()) * 5;
            //                int sextoFactor = int.Parse(characters[5].ToString()) * 4;
            //                int septimoFactor = int.Parse(characters[6].ToString()) * 3;
            //                int octavoFactor = int.Parse(characters[7].ToString()) * 2;

            //                int totFactores = primerFactor + segundoFactor + tercerFactor + cuartoFactor + quintoFactor + sextoFactor + septimoFactor + octavoFactor;
            //                int modulo = totFactores % 11;
            //                int digitoControl = 11 - modulo;
            //                if (digitoControl > 9) { digitoControl = 0; }
            //                if (int.Parse(characters[8].ToString()) == digitoControl)
            //                {
            //                    e.IsValid = true;
            //                }
            //                else 
            //                {
            //                    e.IsValid = false;
            //                    ((CustomValidator)sender).ToolTip = "NIF/NIE Incorrecto";
            //                }
            //            }
            //            else
            //            {
            //                e.IsValid = false;
            //                ((CustomValidator)sender).ToolTip = "NIF/NIE Incorrecto";
            //            }
            //        }
            //        else 
            //        { 
            //            e.IsValid = false;
            //            ((CustomValidator)sender).ToolTip = "NIF/NIE Incorrecto";
            //        }
            //    }
            //}
            //catch (ArgumentException ex)
            //{
            //    ((CustomValidator)sender).ToolTip = ex.Message;
            //}
            //catch (Exception)
            //{
            //}
            e.IsValid = true;
        }



        /// <summary>
        /// Evento que se lanza al validar
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        public void OnRequired_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Value))
                {
                    e.IsValid = true;
                }
                else
                {
                    e.IsValid = false;
                    ((CustomValidator)sender).ToolTip = "Campo No Informado";
                }
            }
            catch (ArgumentException ex)
            {
                ((CustomValidator)sender).ToolTip = ex.Message;
            }
            catch (Exception)
            {
            }
        }

    }
}
