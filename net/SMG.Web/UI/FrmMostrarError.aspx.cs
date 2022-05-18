using System;
using Iberdrola.Commons.Web;
using System.Web;
using System.Web.UI;
namespace Iberdrola.SMG.UI
{
    public partial class FrmMostrarError : FrmBase
    {
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    AsignarCultura Cultur = new AsignarCultura();
            //    Cultur.Asignar(Page);
            //}
			
            Iberdrola.Commons.Web.Errors errs = (Iberdrola.Commons.Web.Errors) Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE);

            if (errs != null)
            {
                lblTextoMensajeError.Text = errs.ToString(true, true, true);
                CurrentSession.RemoveAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE);
            }
            else
            {
                lblTextoMensajeError.Text    = "Ha habido un error en la aplicación";  
            }
            

            Iberdrola.Commons.Web.CurrentSession.RemoveAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE);
        }
        protected void OnBtnVolver_Click(object sender, EventArgs e)
        {
            this.DeleteSessionData();
            //Iberdrola.Commons.Web.NavigationController.Backward();
            if (NavigationController.CanBackward())
            {
                NavigationController.Backward();
            }
            else
            {
                MasterPageMntoGas mp = (MasterPageMntoGas)this.Master;
                //mp.PlaceHolderScript.Controls.Add(new LiteralControl("<script>history.back();</script>"));

                Response.Redirect("../Login.aspx");


            }

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
    }
}
