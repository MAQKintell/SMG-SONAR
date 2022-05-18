using System;
using System.Web;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.UI
{
    public partial class MasterPageAdmin : MasterPageBase
    {
        public MasterPageAdmin()
        {

        }

        public override PlaceHolder PlaceHolderScript
        {
            get { return this._PlaceHolderScript; }
        }
     
        #region atributos
        private static string strVentanaActiva = "";
        #endregion

        #region propiedades
        public string VentanaActiva
        {
            set { MasterPageAdmin.strVentanaActiva = value; }
            get { return MasterPageAdmin.strVentanaActiva; }
        }

        #endregion

        #region metodos
        public bool EsVentanaActiva(string nombreVentana)
        {
            return MasterPageAdmin.strVentanaActiva.Equals(nombreVentana);
        }
        #endregion

        #region Métodos de los eventos
        private void ValidarUsuario()
        {
            if (Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO) != null)
            {
                Boolean UsuarioAutentificado = (Boolean)(Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO));
                if (UsuarioAutentificado == true)
                {
                    AppPrincipal usuarioPrincipal = (AppPrincipal)Iberdrola.Commons.Web.CurrentSession.GetAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    UsuarioDTO usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    lblUsuarioConectado.Text = (string) usuarioDTO.Nombre;
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {	
            if (!NavigationController.IsBackward())
            {
                // Lo primero que se hace es mirar si el usuario está autenticado
                // si no es así se redirige al login.
                ValidarUsuario();
                
                UsuarioDTO usuarioDTO = null;
                try
                {
                    AppPrincipal usuarioPrincipal = (AppPrincipal)CurrentSession.GetMandatoryAttribute(CurrentSession.SESSION_DATOS_USUARIO);
                    usuarioDTO = (UsuarioDTO)((AppIdentity)usuarioPrincipal.Identity).UserObject;
                    Boolean UsuarioAutentificado = (Boolean)(Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_USUARIO_AUTENTIFICADO));
                    if (UsuarioAutentificado == true)
                    {
                        lblUsuarioConectado.Text = (string)usuarioDTO.Nombre;
                    }       
                }
                catch (Exception)
                {
                    Response.Redirect("./Login.aspx");
                }
            }
        }
        
        protected void btnDesconectar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.User = null;

            CurrentSession.Clear();

            MasterPageAdmin.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../Login.aspx");
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            MasterPageAdmin.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../MenuAcceso.aspx");
        }
                
        #endregion

    }
}
