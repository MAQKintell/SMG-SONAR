using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Modules.Authentication.BLL;
using Iberdrola.Commons.Modules.Authentication.DAL.DTO;
using Iberdrola.Commons.Web;

namespace Iberdrola.SMG.UI
{
    /// <summary>
    /// Pantalla para mostrar y añadir Observaciones
    /// </summary>
    public partial class FrmModalCondicionesGenerales : FrmBase
    {
        private AuthenticationDTO _authentication;

        /// <summary>
        /// Evento de inicialización de la página.
        /// Carga los datos de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.CargarPantalla();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        /// <summary>
        /// Evento del botón aceptar de la pantalla.
        /// Guarda las opciones de filtro en la sesión y cierra la ventana.
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        protected void OnBtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidatePage())
                {
                    CondicionesGenerales cg = new CondicionesGenerales(this._authentication);
                    if (cg.TieneNuevasCondiciones(false))
                    {
                        MasterPageModal mpm = (MasterPageModal)this.Master;
                        mpm.CerrarVentanaModal();

                        cg.AceptarCondiciones();

                        string textoErrorAceptarCondiciones = cg.ObtenerTextoErrorAceptarCondiciones();

                        if (string.IsNullOrEmpty(textoErrorAceptarCondiciones))
                        {
                            this.ShowMessage("Se han aceptado las condiciones");
                            mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>parent.document.getElementById('ctl00_ContentPlaceHolderContenido_btnIniciarSesion').click();</script>"));
                        }
                        else
                        {
                            this.ShowMessage(textoErrorAceptarCondiciones);
                        }
                    }
                    else
                    {
                        //Si se está en esta página es que se ha cargado con las condiciones generales descargadas antes y 
                        //por tanto no debería pasar por aquí.
                        MasterPageModal mpm = (MasterPageModal)this.Master;
                        mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Error interno de la aplicación, vuelva a intentarlo');</script>"));
                        mpm.CerrarVentanaModal();
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        /// <summary>
        /// Evento del botón cancelar de la pantalla.
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
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

        protected void CargarPantalla()
        {
            if (CurrentSession.GetAttribute(SessionVariables.SESSION_AUTENTIFICACION_USUARIO) != null)
            {
                this._authentication = (AuthenticationDTO)CurrentSession.GetAttribute(SessionVariables.SESSION_AUTENTIFICACION_USUARIO);

                this.txtCondicionesGenerales.Text = this._authentication.TextoCondField;
            }
            else
            {
                MasterPageModal mpm = (MasterPageModal)this.Master;
                mpm.PlaceHolderScript.Controls.Add(new LiteralControl("<script>alert('Error interno de la aplicación, vuelva a intentarlo');</script>"));
                mpm.CerrarVentanaModal();
            }
        }

        /// <summary>
        /// Evento que se lanza al validar si se han aceptado las condiciones generales
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evendo.</param>
        protected void OnChkAceptarCondiciones_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = chkAceptarCondiciones.Checked;
            if (!e.IsValid)
            {
                ((CustomValidator)sender).ToolTip = ((CustomValidator)sender).ToolTip = FormValidation.MENSAJE_CAMPO_INFORMADO;
            }
        }
        
        #region implementación métodos abstractos
        /// <summary>
        /// Función con las acciones a realizar para la carga de los datos de sesión en la ventana
        /// </summary>
        public override void LoadSessionData() { }
        /// <summary>
        /// Función con las acciones a realizar para guardar los datos en sesión de la ventana
        /// </summary>
        public override void SaveSessionData() { }
        /// <summary>
        /// Función para eliminar los datos de sesión de la ventana
        /// </summary>
        public override void DeleteSessionData() { }
        #endregion implementación métodos abstractos
    }
}
