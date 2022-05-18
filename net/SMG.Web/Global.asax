<%@ Application Language="C#" %>

<script runat="server">


    void Application_Start(object sender, EventArgs e)
    {
        // Código que se ejecuta al iniciarse la aplicación

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Código que se ejecuta cuando se cierra la aplicación
        HttpContext.Current.User = null;

        Iberdrola.Commons.Web.CurrentSession.Clear();

        Iberdrola.Commons.Web.NavigationController.ClearHistory();
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Código que se ejecuta al producirse un error no controlado

    }


    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        
        Iberdrola.SMG.DAL.DTO.ConfiguracionDTO confGuardarAccesos = Iberdrola.SMG.BLL.Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.GUARDAD_ACCESOS_PANTALLAS);
        bool bGuardarAcceso = false;
        if (confGuardarAccesos != null)
            bGuardarAcceso = bool.Parse(confGuardarAccesos.Valor);
        
        if (bGuardarAcceso)
        {
            // Evento lanzado por cada petición de un cliente, ya sea el acceso a una url, una imagen, etc...
            var request = ((System.Web.HttpApplication)sender).Request;
            if (request.CurrentExecutionFilePath.ToString() != "/Login.aspx" && request.CurrentExecutionFilePath.ToString().IndexOf(".aspx") >= 0)
            {
                try
                {
                    //HttpContext.Current.User
                    string usuario = HttpContext.Current.Session["Login"].ToString();

                    //Iberdrola.Commons.Security.AppPrincipal usuarioPrincipal = (Iberdrola.Commons.Security.AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetMandatoryAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_DATOS_USUARIO);
                    //Iberdrola.SMG.DAL.DTO.UsuarioDTO usuarioDT = (Iberdrola.SMG.DAL.DTO.UsuarioDTO)((Iberdrola.Commons.Security.AppIdentity)usuarioPrincipal.Identity).UserObject;
                    if (usuario != null)
                    {
                        Iberdrola.SMG.BLL.Consultas.InsertarAccesoPantalla(usuario, request.CurrentExecutionFilePath.ToString());
                    }
                }
                catch
                {
                    Iberdrola.SMG.BLL.Consultas.InsertarAccesoPantalla("", request.CurrentExecutionFilePath.ToString());
                }
            }
        }

    }


    void Session_Start(object sender, EventArgs e)
    {
        // Código que se ejecuta cuando se inicia una nueva sesión
        //Session.Timeout = 360;

        HttpContext.Current.User = null;

        Iberdrola.Commons.Web.CurrentSession.Clear();

        Iberdrola.Commons.Web.NavigationController.ClearHistory();

    }

    void Session_End(object sender, EventArgs e)
    {
        // Código que se ejecuta cuando finaliza una sesión. 
        // Nota: El evento Session_End se desencadena sólo con el modo sessionstate
        // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer 
        // o SQLServer, el evento no se genera.
        HttpContext.Current.User = null;

        Iberdrola.Commons.Web.CurrentSession.Clear();

        Iberdrola.Commons.Web.NavigationController.ClearHistory();
    }

    void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

</script>
