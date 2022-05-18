using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Iberdrola.Commons.Security;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Web;
using Iberdrola.Commons.Logging;
using Iberdrola.SMG.DAL.DTO;

using Iberdrola.SMG.BLL;
namespace Iberdrola.SMG.UI
{
    public partial class MasterPageMntoGasSinMenu : MasterPageBase
    {
        public MasterPageMntoGasSinMenu()
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
            set { MasterPageMntoGasSinMenu.strVentanaActiva = value; }
            get { return MasterPageMntoGasSinMenu.strVentanaActiva; }
        }

        #endregion

        #region metodos
        public bool EsVentanaActiva(string nombreVentana)
        {
            return MasterPageMntoGasSinMenu.strVentanaActiva.Equals(nombreVentana);
        }
        #endregion

        #region metodos abstractos
        //public abstract List<String> ValidateForm();
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
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            if (!NavigationController.IsBackward())
            {
                // Lo primero que se hace es mirar si el usuario está autenticado
                // si no es así se redirige al login.
                ValidarUsuario();
            }
        }

        protected void btnDesconectar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.User = null;

            CurrentSession.Clear();

            MasterPageMntoGasSinMenu.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../Login.aspx");
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            MasterPageMntoGasSinMenu.strVentanaActiva = "";
            NavigationController.ClearHistory();
            Response.Redirect("../MenuAcceso.aspx");
        }        

        #endregion
}
}
