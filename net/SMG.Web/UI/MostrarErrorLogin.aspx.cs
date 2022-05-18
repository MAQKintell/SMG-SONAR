using System;
using Iberdrola.Commons.Web;

namespace Iberdrola.SMG.UI
{
    /// <summary>
    /// Pantalla que muestra los errores producidos en la pantalla de Login.
    /// </summary>
    public partial class MostrarErrorLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Evento que se lanza al cargar la página
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Iberdrola.Commons.Web.Errors errs = (Iberdrola.Commons.Web.Errors)Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE);

                lblTextoMensajeError.Text = errs.ToString(true, true, true);

                Iberdrola.Commons.Web.CurrentSession.RemoveAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_ERROR_VARIABLE);
            }
            catch
            { 
                // Si pasa este error no hacemos nada, ya que ya estamos en la página de error y ya estamos 
                // mostrando un error anterior.
            }
        }

        /// <summary>
        /// Evento lanzado al pulsar sobre el botón de volver
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        protected void BtnVolverIniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationController.Backward();
            }
            catch (Exception ex)
            {
                Error err = new Error("4002", Iberdrola.Commons.Web.Error.SeverityLevel.Error);

                Errors errs = new Errors();
                errs.Add(err);
                CurrentSession.SetAttribute(CurrentSession.SESSION_ERROR_VARIABLE, errs);

                NavigationController.Forward("./MostrarErrorLogin.aspx");
            }
        }
    }
}
